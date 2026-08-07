using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Timers;

namespace EpicPenClone
{
    public partial class MainForm : Form
    {
        private bool _isDrawing = false;
        private Point _lastPoint;
        private Color _currentColor = Color.Red;
        private int _brushSize = 5;
        private ToolType _currentTool = ToolType.Pen;
        private List<Stroke> _strokes = new List<Stroke>();
        private Stack<List<Stroke>> _undoStack = new Stack<List<Stroke>>();
        private Stack<List<Stroke>> _redoStack = new Stack<List<Stroke>>();
        
        // Auto-erase feature
        private System.Timers.Timer? _autoEraseTimer;
        private int _autoEraseDelaySeconds = 5; // Default 5 seconds
        private CheckBox? _chkAutoErase;
        private NumericUpDown? _numAutoEraseDelay;
        
        private Panel? _toolbarPanel;
        private Button? _btnPen;
        private Button? _btnHighlighter;
        private Button? _btnEraser;
        private Button? _btnColorPicker;
        private NumericUpDown? _numBrushSize;
        private Button? _btnUndo;
        private Button? _btnRedo;
        private Button? _btnClear;
        private Button? _btnExit;

        public MainForm()
        {
            InitializeComponents();
            SetupToolbar();
            SetupAutoEraseTimer();
            
            // Set form properties for full-screen transparent overlay
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;
            this.BackColor = Color.FromArgb(0, 255, 255); // Cyan key color
            this.TransparencyKey = Color.FromArgb(0, 255, 255);
            this.DoubleBuffered = true;
            
            // Allow click-through when not drawing
            this.MouseDown += MainForm_MouseDown;
            this.MouseMove += MainForm_MouseMove;
            this.MouseUp += MainForm_MouseUp;
            this.Paint += MainForm_Paint;
        }

        private void InitializeComponents()
        {
            this.SuspendLayout();
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1920, 1080);
            this.Name = "MainForm";
            this.Text = "Epic Pen Clone";
            this.ResumeLayout(false);
        }

        private void SetupToolbar()
        {
            if (_toolbarPanel == null)
            {
                _toolbarPanel = new Panel
                {
                    Height = 60,
                    BackColor = Color.FromArgb(240, 240, 240),
                    Location = new Point((this.Width - 500) / 2, 5)
                };
                
                int buttonWidth = 50;
                int buttonHeight = 45;
                int spacing = 5;
                int startX = 10;
                
                // Pen button
                _btnPen = CreateToolButton("Pen", startX, 5, buttonWidth, buttonHeight, ToolType.Pen);
                _btnPen.BackColor = Color.LightBlue;
                
                // Highlighter button
                _btnHighlighter = CreateToolButton("High", startX + (buttonWidth + spacing), 5, buttonWidth, buttonHeight, ToolType.Highlighter);
                
                // Eraser button
                _btnEraser = CreateToolButton("Erase", startX + (buttonWidth + spacing) * 2, 5, buttonWidth, buttonHeight, ToolType.Eraser);
                
                // Color picker button
                _btnColorPicker = new Button
                {
                    Text = "Color",
                    Location = new Point(startX + (buttonWidth + spacing) * 3, 5),
                    Size = new Size(buttonWidth, buttonHeight),
                    FlatStyle = FlatStyle.Flat
                };
                _btnColorPicker.Click += BtnColorPicker_Click;
                
                // Brush size numeric up/down
                Label lblSize = new Label
                {
                    Text = "Size:",
                    Location = new Point(startX + (buttonWidth + spacing) * 4 + 5, 15),
                    AutoSize = true
                };
                
                _numBrushSize = new NumericUpDown
                {
                    Location = new Point(startX + (buttonWidth + spacing) * 4 + 40, 10),
                    Size = new Size(50, 25),
                    Minimum = 1,
                    Maximum = 50,
                    Value = 5
                };
                _numBrushSize.ValueChanged += (s, e) => _brushSize = (int)_numBrushSize!.Value;
                
                // Undo button
                _btnUndo = CreateActionButton("Undo", startX + (buttonWidth + spacing) * 5 + 20, 5, buttonWidth, buttonHeight);
                _btnUndo.Click += (s, e) => Undo();
                
                // Redo button
                _btnRedo = CreateActionButton("Redo", startX + (buttonWidth + spacing) * 6 + 20, 5, buttonWidth, buttonHeight);
                _btnRedo.Click += (s, e) => Redo();
                
                // Clear button
                _btnClear = CreateActionButton("Clear", startX + (buttonWidth + spacing) * 7 + 20, 5, buttonWidth, buttonHeight);
                _btnClear.Click += (s, e) => ClearCanvas();
                
                // Exit button
                _btnExit = CreateActionButton("Exit", startX + (buttonWidth + spacing) * 8 + 20, 5, buttonWidth, buttonHeight);
                _btnExit.BackColor = Color.IndianRed;
                _btnExit.Click += (s, e) => Application.Exit();
                
                // Auto-erase checkbox
                _chkAutoErase = new CheckBox
                {
                    Text = "Auto-Erase",
                    Location = new Point(startX + (buttonWidth + spacing) * 9 + 20, 15),
                    AutoSize = true,
                    Checked = false
                };
                _chkAutoErase.CheckedChanged += ChkAutoErase_CheckedChanged;
                
                // Auto-erase delay numeric up/down
                Label lblAutoEraseDelay = new Label
                {
                    Text = "Delay (s):",
                    Location = new Point(startX + (buttonWidth + spacing) * 10 + 30, 15),
                    AutoSize = true
                };
                
                _numAutoEraseDelay = new NumericUpDown
                {
                    Location = new Point(startX + (buttonWidth + spacing) * 10 + 95, 10),
                    Size = new Size(40, 25),
                    Minimum = 1,
                    Maximum = 60,
                    Value = 5
                };
                _numAutoEraseDelay.ValueChanged += NumAutoEraseDelay_ValueChanged;
                
                _toolbarPanel.Controls.AddRange(new Control[] { 
                    _btnPen, _btnHighlighter, _btnEraser, _btnColorPicker, 
                    lblSize, _numBrushSize, _btnUndo, _btnRedo, _btnClear, _btnExit,
                    _chkAutoErase, lblAutoEraseDelay, _numAutoEraseDelay
                });
                
                this.Controls.Add(_toolbarPanel);
            }
        }
        
        private void SetupAutoEraseTimer()
        {
            _autoEraseTimer = new System.Timers.Timer(_autoEraseDelaySeconds * 1000);
            _autoEraseTimer.Elapsed += AutoEraseTimer_Elapsed;
            _autoEraseTimer.AutoReset = false; // Only fire once per stroke
        }
        
        private void ChkAutoErase_CheckedChanged(object? sender, EventArgs e)
        {
            if (_chkAutoErase != null && _autoEraseTimer != null)
            {
                _autoEraseTimer.Enabled = _chkAutoErase.Checked;
            }
        }
        
        private void NumAutoEraseDelay_ValueChanged(object? sender, EventArgs e)
        {
            if (_numAutoEraseDelay != null && _autoEraseTimer != null)
            {
                _autoEraseDelaySeconds = (int)_numAutoEraseDelay.Value;
                _autoEraseTimer.Interval = _autoEraseDelaySeconds * 1000;
            }
        }
        
        private void AutoEraseTimer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            // Invoke on UI thread to remove the oldest stroke
            this.Invoke(new Action(() =>
            {
                if (_strokes.Count > 0)
                {
                    // Save state for undo before auto-erasing
                    SaveStateForUndo();
                    
                    // Remove the oldest stroke (FIFO - first in, first out)
                    _strokes.RemoveAt(0);
                    this.Invalidate();
                    
                    // If there are more strokes and timer should continue, restart it
                    if (_strokes.Count > 0 && _chkAutoErase != null && _chkAutoErase.Checked)
                    {
                        _autoEraseTimer?.Start();
                    }
                }
            }));
        }
        
        private Button CreateToolButton(string text, int x, int y, int width, int height, ToolType toolType)
        {
            var button = new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(width, height),
                FlatStyle = FlatStyle.Flat,
                Tag = toolType
            };
            button.Click += ToolButton_Click;
            return button;
        }
        
        private Button CreateActionButton(string text, int x, int y, int width, int height)
        {
            return new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(width, height),
                FlatStyle = FlatStyle.Flat
            };
        }
        
        private void ToolButton_Click(object? sender, EventArgs e)
        {
            if (sender is Button button && button.Tag is ToolType toolType)
            {
                _currentTool = toolType;
                
                // Reset all button colors
                if (_btnPen != null) _btnPen.BackColor = Color.Transparent;
                if (_btnHighlighter != null) _btnHighlighter.BackColor = Color.Transparent;
                if (_btnEraser != null) _btnEraser.BackColor = Color.Transparent;
                
                // Set active button color
                button.BackColor = Color.LightBlue;
            }
        }
        
        private void BtnColorPicker_Click(object? sender, EventArgs e)
        {
            using (var colorDialog = new ColorDialog())
            {
                colorDialog.Color = _currentColor;
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    _currentColor = colorDialog.Color;
                    if (_btnColorPicker != null)
                        _btnColorPicker.BackColor = _currentColor;
                }
            }
        }

        private void MainForm_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && !IsPointInToolbar(e.Location))
            {
                _isDrawing = true;
                _lastPoint = e.Location;
                
                // Start a new stroke
                var stroke = new Stroke
                {
                    Color = _currentTool == ToolType.Eraser ? Color.Transparent : _currentColor,
                    Size = _brushSize,
                    Tool = _currentTool,
                    CreatedTime = DateTime.Now
                };
                stroke.Points.Add(e.Location);
                _strokes.Add(stroke);
            }
        }

        private void MainForm_MouseMove(object? sender, MouseEventArgs e)
        {
            if (_isDrawing && !IsPointInToolbar(e.Location))
            {
                if (_strokes.Count > 0)
                {
                    var currentStroke = _strokes[_strokes.Count - 1];
                    currentStroke.Points.Add(e.Location);
                }
                
                // Draw line from last point to current point
                using (Graphics g = this.CreateGraphics())
                {
                    DrawStroke(g, _strokes[_strokes.Count - 1], _lastPoint, e.Location);
                }
                
                _lastPoint = e.Location;
            }
        }

        private void MainForm_MouseUp(object? sender, MouseEventArgs e)
        {
            if (_isDrawing)
            {
                _isDrawing = false;
                SaveStateForUndo();
                
                // Start auto-erase timer if enabled
                if (_chkAutoErase != null && _chkAutoErase.Checked && _autoEraseTimer != null)
                {
                    _autoEraseTimer.Stop();
                    _autoEraseTimer.Start();
                }
                
                this.Invalidate(); // Redraw everything
            }
        }
        
        private void MainForm_Paint(object? sender, PaintEventArgs e)
        {
            foreach (var stroke in _strokes)
            {
                if (stroke.Points.Count < 2) continue;
                
                using (var pen = new Pen(stroke.Color, stroke.Size))
                {
                    if (stroke.Tool == ToolType.Highlighter)
                    {
                        pen.Color = Color.FromArgb(100, stroke.Color.R, stroke.Color.G, stroke.Color.B);
                    }
                    else if (stroke.Tool == ToolType.Eraser)
                    {
                        pen.Color = this.TransparencyKey;
                    }
                    
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    
                    for (int i = 0; i < stroke.Points.Count - 1; i++)
                    {
                        e.Graphics.DrawLine(pen, stroke.Points[i], stroke.Points[i + 1]);
                    }
                }
            }
        }
        
        private bool IsPointInToolbar(Point point)
        {
            if (_toolbarPanel == null) return false;
            
            var toolbarRect = new Rectangle(_toolbarPanel.Location, _toolbarPanel.Size);
            return toolbarRect.Contains(point);
        }
        
        private void DrawStroke(Graphics g, Stroke stroke, Point start, Point end)
        {
            using (var pen = new Pen(stroke.Color, stroke.Size))
            {
                if (stroke.Tool == ToolType.Highlighter)
                {
                    pen.Color = Color.FromArgb(100, stroke.Color.R, stroke.Color.G, stroke.Color.B);
                }
                else if (stroke.Tool == ToolType.Eraser)
                {
                    pen.Color = this.TransparencyKey; // Use transparency key as eraser
                }
                
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                
                g.DrawLine(pen, start, end);
            }
        }
        
        private void SaveStateForUndo()
        {
            // Create a copy of current strokes
            var strokesCopy = new List<Stroke>();
            foreach (var stroke in _strokes)
            {
                var newStroke = new Stroke
                {
                    Color = stroke.Color,
                    Size = stroke.Size,
                    Tool = stroke.Tool
                };
                newStroke.Points.AddRange(stroke.Points);
                strokesCopy.Add(newStroke);
            }
            
            _undoStack.Push(strokesCopy);
            _redoStack.Clear(); // Clear redo stack on new action
            
            // Limit undo stack size
            while (_undoStack.Count > 50)
            {
                _undoStack.RemoveAt(_undoStack.Count - 1);
            }
        }
        
        private void Undo()
        {
            if (_undoStack.Count > 0)
            {
                // Save current state for redo
                var currentState = CloneStrokes(_strokes);
                _redoStack.Push(currentState);
                
                // Restore previous state
                _strokes = _undoStack.Pop();
                this.Invalidate();
            }
        }
        
        private void Redo()
        {
            if (_redoStack.Count > 0)
            {
                // Save current state for undo
                var currentState = CloneStrokes(_strokes);
                _undoStack.Push(currentState);
                
                // Restore next state
                _strokes = _redoStack.Pop();
                this.Invalidate();
            }
        }
        
        private List<Stroke> CloneStrokes(List<Stroke> strokes)
        {
            var clone = new List<Stroke>();
            foreach (var stroke in strokes)
            {
                var newStroke = new Stroke
                {
                    Color = stroke.Color,
                    Size = stroke.Size,
                    Tool = stroke.Tool
                };
                newStroke.Points.AddRange(stroke.Points);
                clone.Add(newStroke);
            }
            return clone;
        }
        
        private void ClearCanvas()
        {
            if (_strokes.Count > 0)
            {
                SaveStateForUndo();
                _strokes.Clear();
                _redoStack.Clear();
                
                // Stop auto-erase timer if running
                if (_autoEraseTimer != null)
                {
                    _autoEraseTimer.Stop();
                }
                
                this.Invalidate();
            }
        }
        
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Clean up the timer when form closes
            _autoEraseTimer?.Stop();
            _autoEraseTimer?.Dispose();
            base.OnFormClosing(e);
        }
    }
    
    enum ToolType
    {
        Pen,
        Highlighter,
        Eraser
    }
    
    class Stroke
    {
        public List<Point> Points { get; set; } = new List<Point>();
        public Color Color { get; set; }
        public int Size { get; set; }
        public ToolType Tool { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.Now; // Track when stroke was created
    }
}
