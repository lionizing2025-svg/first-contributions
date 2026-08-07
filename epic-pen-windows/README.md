# Epic Pen Clone - Windows Desktop App

A C# Windows Forms application that clones the functionality of Epic Pen - a screen annotation tool.

## Features

### Current Implementation (v2.0)
- ✏️ **Pen Tool** - Draw freehand with any color
- 🖍️ **Highlighter Tool** - Semi-transparent highlighting 
- 🧹 **Eraser Tool** - Remove strokes
- 🎨 **Color Picker** - Choose from millions of colors
- 📏 **Size Control** - Adjust brush size (1-50px)
- ↩️ **Undo/Redo** - Full history support (up to 50 states)
- 🗑️ **Clear Canvas** - Start fresh
- ⏱️ **Auto-Erase** - Strokes automatically disappear after configurable delay (1-60 seconds)
- 🚪 **Exit** - Close the application
- 🖥️ **Full-Screen Overlay** - Transparent window that sits on top of all applications
- 🎯 **Smart Toolbar** - Floating toolbar at the top of the screen

### Works With Any Application!
✅ PowerPoint Presentations  
✅ PDF Readers (Adobe Acrobat, Edge, etc.)  
✅ Web Browsers  
✅ Desktop Applications  
✅ Videos & Media Players  
✅ Most Fullscreen Applications  

## Requirements

- Windows 10/11
- .NET 8.0 SDK or later
- Visual Studio 2022 (optional, for development)

## How to Build

### Option 1: Using Command Line
```bash
cd epic-pen-windows
dotnet restore
dotnet build
```

### Option 2: Using Visual Studio
1. Open `EpicPenClone.csproj` in Visual Studio 2022
2. Build the solution (Ctrl+Shift+B)

## How to Run

### Option 1: Using Command Line
```bash
dotnet run
```

### Option 2: Using the Executable
After building, run:
```bash
./bin/Debug/net8.0-windows/EpicPenClone.exe
```

### Option 3: Publish as Standalone EXE
```bash
dotnet publish -c Release -r win-x64 --self-contained true
```
The executable will be in `bin/Release/net8.0-windows/win-x64/publish/`

## Usage

1. **Launch the Application**: The app will start in full-screen transparent mode
2. **Select a Tool**: Click on Pen, Highlighter, or Eraser in the toolbar
3. **Choose Color**: Click the "Color" button to pick your drawing color
4. **Adjust Size**: Use the numeric up/down to change brush size
5. **Enable Auto-Erase** (Optional): 
   - Check the "Auto-Erase" checkbox to enable automatic stroke removal
   - Set the delay time (1-60 seconds) using the "Delay (s)" control
   - Each stroke will automatically disappear after the specified time
6. **Draw**: Click and drag anywhere on the screen (outside the toolbar) to draw
   - Works over PowerPoint, PDFs, browsers, videos, and most applications!
7. **Undo/Redo**: Use the toolbar buttons to undo/redo actions
8. **Clear**: Click "Clear" to remove all drawings immediately
9. **Exit**: Click "Exit" or press Alt+F4 to close the application

### Using with PowerPoint Presentations
1. Start your PowerPoint slideshow (F5)
2. Launch Epic Pen Clone
3. Select your tool and start annotating directly on your slides
4. Annotations appear on top of the presentation
5. Use Auto-Erase for temporary notes that disappear automatically

### Using with PDF Documents
1. Open your PDF in any reader (Adobe Acrobat, Edge, Chrome, etc.)
2. Launch Epic Pen Clone
3. Draw directly on the PDF
4. Perfect for highlighting, underlining, or adding notes during reviews

## Architecture

### Key Components

- **MainForm.cs**: Main application window with transparent overlay and auto-erase functionality
- **Program.cs**: Application entry point
- **Stroke Class**: Stores drawing data (points, color, size, tool type, creation time)
- **ToolType Enum**: Defines available tools (Pen, Highlighter, Eraser)
- **Auto-Erase Timer**: System.Timers.Timer that removes strokes after configurable delay

### Technical Details

- **Transparency**: Uses `TransparencyKey` to create a click-through effect
- **Double Buffering**: Enabled to prevent flickering during drawing
- **Anti-Aliasing**: Smooth lines using `SmoothingMode.AntiAlias`
- **TopMost**: Window stays above all other applications
- **Borderless**: No window borders for seamless overlay experience
- **Auto-Erase**: FIFO queue system removes oldest strokes first after user-defined delay
- **Timer Management**: Proper cleanup on form close and canvas clear

## Future Enhancements

Planned features for future versions:

1. **Shape Tools**
   - Rectangles
   - Circles/Ellipses
   - Arrows
   - Lines
   
2. **Text Annotations**
   - Add text boxes anywhere on screen
   - Font customization
   
3. **Advanced Features**
   - Multiple layers
   - Custom brushes/patterns
   - Fill tool (paint bucket)
   - Zoom and pan
   
4. **UI Improvements**
   - Draggable toolbar
   - Collapsible/minimizable toolbar
   - Keyboard shortcuts customization
   - System tray integration
   
5. **File Operations**
   - Save annotations as image
   - Load previous sessions
   - Export to PDF
   
6. **Performance**
   - Hardware acceleration
   - Optimized rendering for large drawings

## Troubleshooting

### Issue: Application doesn't start
- Ensure .NET 8.0 runtime is installed
- Check if Windows Forms is supported on your system

### Issue: Drawing appears laggy
- Reduce brush size
- Close other resource-intensive applications
- Disable Auto-Erase if not needed

### Issue: Can't draw on certain applications
- Some fullscreen games/applications may block overlay
- Try running Epic Pen as Administrator
- For PowerPoint, ensure you're in Slideshow mode (F5)

### Issue: Toolbar is not visible
- Move your mouse to the top of the screen
- The toolbar should appear at the top center
- If using multiple monitors, the toolbar appears on the primary monitor

### Issue: Auto-Erase not working
- Ensure the "Auto-Erase" checkbox is checked
- Verify the delay time is set correctly (1-60 seconds)
- Note: Each stroke disappears individually after the set delay from when it was drawn

### Issue: Strokes don't disappear in order
- Strokes are removed in FIFO order (First In, First Out)
- The oldest stroke disappears first, then the next oldest, etc.

## License

This is an educational project created as a clone of Epic Pen for learning purposes.

## Contributing

Feel free to fork this repository and submit pull requests with improvements!

---

**Note**: This is a basic version. More features will be added gradually as we improve the application.
