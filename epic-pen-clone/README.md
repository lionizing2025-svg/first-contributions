# Epic Pen Clone

A web-based screen annotation and drawing tool inspired by Epic Pen. This application allows you to draw, highlight, and annotate directly on your browser window.

## Features

### Basic Tools (Version 1.0)
- **Pen Tool**: Draw freehand lines with customizable colors
- **Highlighter Tool**: Semi-transparent highlighting for emphasizing content
- **Eraser Tool**: Remove unwanted strokes
- **Color Picker**: Choose any color for your drawings
- **Brush Size Control**: Adjust stroke thickness from 1 to 50 pixels
- **Undo/Redo**: Step backward and forward through your drawing history (up to 50 states)
- **Clear Canvas**: Remove all annotations with a single click
- **Save Image**: Export your creation as a PNG file
- **Keyboard Shortcuts**: 
  - `Ctrl+Z` for Undo
  - `Ctrl+Y` for Redo
- **Touch Support**: Works on mobile devices and touchscreens
- **Responsive Design**: Adapts to different screen sizes

## Getting Started

### Prerequisites
- A modern web browser (Chrome, Firefox, Safari, Edge)
- No installation required!

### Running the Application

1. **Option 1: Open directly in browser**
   ```bash
   # Navigate to the epic-pen-clone directory
   cd epic-pen-clone
   
   # Open index.html in your default browser
   # On Linux:
   xdg-open index.html
   
   # On macOS:
   open index.html
   
   # On Windows:
   start index.html
   ```

2. **Option 2: Use a local server**
   ```bash
   # Using Python 3
   python3 -m http.server 8000
   
   # Then open http://localhost:8000 in your browser
   ```

3. **Option 3: Use Live Server extension (VS Code)**
   - Install the Live Server extension in VS Code
   - Right-click on `index.html` and select "Open with Live Server"

## Project Structure

```
epic-pen-clone/
├── index.html      # Main HTML structure
├── styles.css      # Styling and layout
├── script.js       # Drawing logic and interactivity
└── README.md       # Documentation
```

## How to Use

1. **Select a Tool**: Click on Pen, Highlighter, or Eraser in the toolbar
2. **Choose a Color**: Use the color picker to select your desired color
3. **Adjust Size**: Use the slider to set brush thickness
4. **Draw**: Click and drag on the canvas to draw
5. **Undo/Redo**: Use the buttons or keyboard shortcuts to navigate history
6. **Save**: Click the save button to download your drawing as PNG

## Future Improvements

Here are some features we can add in future versions:

### Version 2.0 Ideas
- [ ] Shape tools (rectangle, circle, line, arrow)
- [ ] Text annotation tool
- [ ] Multiple layers support
- [ ] Custom brush patterns
- [ ] Opacity control for all tools
- [ ] Fill tool (paint bucket)
- [ ] Zoom and pan functionality

### Version 3.0 Ideas
- [ ] Screen capture integration
- [ ] Annotation over any window (desktop app)
- [ ] Presentation mode
- [ ] Real-time collaboration
- [ ] Export to PDF
- [ ] Template backgrounds (grid, dots, lines)
- [ ] Pressure sensitivity (for stylus users)

### Advanced Features
- [ ] AI-powered shape recognition
- [ ] Smart highlighting (auto-detect text regions)
- [ ] Recording/drawing replay
- [ ] Cloud storage integration
- [ ] Plugin system for custom tools

## Technologies Used

- **HTML5 Canvas**: For drawing and rendering
- **CSS3**: For styling and responsive design
- **Vanilla JavaScript**: For all interactive functionality
- No external libraries or frameworks required!

## Browser Compatibility

- ✅ Chrome/Chromium (Recommended)
- ✅ Firefox
- ✅ Safari
- ✅ Edge
- ✅ Opera

## Contributing

This is a learning project. Feel free to:
1. Fork the repository
2. Add new features
3. Improve existing functionality
4. Fix bugs
5. Submit pull requests

## License

MIT License - Feel free to use this for learning and personal projects!

## Credits

Inspired by Epic Pen - https://epicpen.com/

This is a clone created for educational purposes to demonstrate HTML5 Canvas capabilities and build a foundation for more advanced drawing applications.

---

**Happy Drawing! 🎨**
