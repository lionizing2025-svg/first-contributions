# Changelog

All notable changes to Epic Pen Clone will be documented in this file.

## [2.0.0] - 2024

### Added
- ⏱️ **Auto-Erase Feature**: Strokes automatically disappear after a configurable delay (1-60 seconds)
  - Toggle on/off with checkbox in toolbar
  - Adjustable delay timer via numeric control
  - FIFO (First-In-First-Out) removal order
  - Each stroke disappears individually based on when it was drawn
  
- 📝 **Stroke Timestamp Tracking**: Each stroke now records creation time for future enhancements

- 🧹 **Timer Cleanup**: Proper disposal of timers on form close and canvas clear

### Improved
- Enhanced toolbar layout to accommodate new auto-erase controls
- Better resource management with timer lifecycle handling
- Updated documentation with usage examples for PowerPoint and PDF applications

### Changed
- Version bumped to 2.0.0 to reflect major feature addition
- README updated with comprehensive troubleshooting section

---

## [1.0.0] - Initial Release

### Features
- ✏️ Pen tool with customizable color and size
- 🖍️ Highlighter tool with transparency
- 🧹 Eraser tool
- ↩️ Undo/Redo support (up to 50 states)
- 🗑️ Clear canvas functionality
- 🎨 Color picker
- 📏 Brush size adjustment (1-50px)
- 🖥️ Full-screen transparent overlay
- 🎯 Top-most window positioning
