// Canvas setup
const canvas = document.getElementById('drawingCanvas');
const ctx = canvas.getContext('2d');

// Tool buttons
const toolBtns = document.querySelectorAll('.tool-btn');
const colorPicker = document.getElementById('colorPicker');
const sizeSlider = document.getElementById('sizeSlider');
const sizeValue = document.getElementById('sizeValue');
const clearBtn = document.getElementById('clearBtn');
const undoBtn = document.getElementById('undoBtn');
const redoBtn = document.getElementById('redoBtn');
const saveBtn = document.getElementById('saveBtn');
const instructions = document.getElementById('instructions');
const closeInstructions = document.getElementById('closeInstructions');

// State variables
let isDrawing = false;
let currentTool = 'pen';
let currentColor = '#ff0000';
let currentSize = 3;
let lastX = 0;
let lastY = 0;
let history = [];
let historyStep = -1;

// Resize canvas to fill window
function resizeCanvas() {
    // Save current content
    const tempCanvas = document.createElement('canvas');
    const tempCtx = tempCanvas.getContext('2d');
    tempCanvas.width = canvas.width;
    tempCanvas.height = canvas.height;
    tempCtx.drawImage(canvas, 0, 0);
    
    // Resize
    canvas.width = window.innerWidth;
    canvas.height = window.innerHeight;
    
    // Restore content
    ctx.drawImage(tempCanvas, 0, 0);
    
    // Set default styles
    updateContextStyles();
}

// Update context styles based on current tool
function updateContextStyles() {
    ctx.strokeStyle = currentTool === 'eraser' ? '#ffffff' : currentColor;
    ctx.lineWidth = currentSize;
    ctx.lineCap = 'round';
    ctx.lineJoin = 'round';
    
    if (currentTool === 'highlighter') {
        ctx.globalAlpha = 0.3;
    } else {
        ctx.globalAlpha = 1.0;
    }
}

// Initialize canvas
resizeCanvas();
updateContextStyles();
saveState();

// Event Listeners
window.addEventListener('resize', resizeCanvas);

// Tool selection
toolBtns.forEach(btn => {
    btn.addEventListener('click', () => {
        toolBtns.forEach(b => b.classList.remove('active'));
        btn.classList.add('active');
        currentTool = btn.dataset.tool;
        updateContextStyles();
    });
});

// Color picker
colorPicker.addEventListener('input', (e) => {
    currentColor = e.target.value;
    if (currentTool !== 'eraser') {
        updateContextStyles();
    }
});

// Size slider
sizeSlider.addEventListener('input', (e) => {
    currentSize = e.target.value;
    sizeValue.textContent = currentSize;
    updateContextStyles();
});

// Drawing functions
function startDrawing(e) {
    isDrawing = true;
    [lastX, lastY] = [e.clientX, e.clientY];
}

function draw(e) {
    if (!isDrawing) return;
    
    ctx.beginPath();
    ctx.moveTo(lastX, lastY);
    ctx.lineTo(e.clientX, e.clientY);
    ctx.stroke();
    
    [lastX, lastY] = [e.clientX, e.clientY];
}

function stopDrawing() {
    if (isDrawing) {
        isDrawing = false;
        saveState();
    }
}

// Mouse events
canvas.addEventListener('mousedown', startDrawing);
canvas.addEventListener('mousemove', draw);
canvas.addEventListener('mouseup', stopDrawing);
canvas.addEventListener('mouseout', stopDrawing);

// Touch events for mobile support
canvas.addEventListener('touchstart', (e) => {
    e.preventDefault();
    const touch = e.touches[0];
    const mouseEvent = new MouseEvent('mousedown', {
        clientX: touch.clientX,
        clientY: touch.clientY
    });
    canvas.dispatchEvent(mouseEvent);
});

canvas.addEventListener('touchmove', (e) => {
    e.preventDefault();
    const touch = e.touches[0];
    const mouseEvent = new MouseEvent('mousemove', {
        clientX: touch.clientX,
        clientY: touch.clientY
    });
    canvas.dispatchEvent(mouseEvent);
});

canvas.addEventListener('touchend', () => {
    const mouseEvent = new MouseEvent('mouseup', {});
    canvas.dispatchEvent(mouseEvent);
});

// Save state for undo/redo
function saveState() {
    historyStep++;
    history[historyStep] = canvas.toDataURL();
    // Remove any redo states
    history = history.slice(0, historyStep + 1);
    // Limit history size
    if (history.length > 50) {
        history.shift();
        historyStep--;
    }
}

// Undo function
function undo() {
    if (historyStep > 0) {
        historyStep--;
        restoreState();
    }
}

// Redo function
function redo() {
    if (historyStep < history.length - 1) {
        historyStep++;
        restoreState();
    }
}

// Restore state from history
function restoreState() {
    const img = new Image();
    img.src = history[historyStep];
    img.onload = () => {
        ctx.clearRect(0, 0, canvas.width, canvas.height);
        ctx.drawImage(img, 0, 0);
        updateContextStyles();
    };
}

// Clear canvas
function clearCanvas() {
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    saveState();
}

// Save canvas as image
function saveCanvas() {
    const link = document.createElement('a');
    link.download = 'epic-pen-drawing.png';
    link.href = canvas.toDataURL();
    link.click();
}

// Button event listeners
clearBtn.addEventListener('click', clearCanvas);
undoBtn.addEventListener('click', undo);
redoBtn.addEventListener('click', redo);
saveBtn.addEventListener('click', saveCanvas);

// Keyboard shortcuts
document.addEventListener('keydown', (e) => {
    if (e.ctrlKey && e.key === 'z') {
        e.preventDefault();
        undo();
    }
    if (e.ctrlKey && e.key === 'y') {
        e.preventDefault();
        redo();
    }
});

// Instructions panel
closeInstructions.addEventListener('click', () => {
    instructions.style.display = 'none';
});

// Show toolbar hide/show on mouse movement
let toolbarTimeout;
const toolbar = document.getElementById('toolbar');

document.addEventListener('mousemove', () => {
    toolbar.style.opacity = '1';
    clearTimeout(toolbarTimeout);
    toolbarTimeout = setTimeout(() => {
        if (!toolbar.matches(':hover')) {
            toolbar.style.opacity = '0.7';
        }
    }, 2000);
});

toolbar.addEventListener('mouseenter', () => {
    toolbar.style.opacity = '1';
    clearTimeout(toolbarTimeout);
});

toolbar.addEventListener('mouseleave', () => {
    toolbarTimeout = setTimeout(() => {
        toolbar.style.opacity = '0.7';
    }, 500);
});

console.log('Epic Pen Clone initialized! Start drawing!');
