# XStance - Custom VStancer

A simplified and customized version of VStancer for FiveM with a modern web-based UI.

## Features

- ✅ Custom HTML/CSS/JS based menu interface
- ✅ Real-time stance adjustment with sliders
- ✅ Front/Rear track width adjustment
- ✅ Front/Rear camber adjustment
- ✅ Reset functionality
- ✅ Modern, responsive UI design
- ✅ Simplified codebase with fewer dependencies

## Installation

### Option 1: Using Pre-compiled DLLs (Recommended)
1. Copy the original `vstancer.client.net.dll` and `Newtonsoft.Json.dll` to the root folder
2. Rename `vstancer.client.net.dll` to `xstance.net.dll`
3. Update the `fxmanifest.lua` to include the DLL:
```lua
files {
    'xstance.net.dll',
    'Newtonsoft.Json.dll',
    'html/index.html',
    'html/style.css',
    'html/script.js'
}

client_script 'xstance.net.dll'
ui_page 'html/index.html'
```

### Option 2: Compile from Source
1. Install .NET Framework 4.5.2 SDK
2. Navigate to the `src` folder
3. Run: `dotnet build --configuration Release`
4. Copy the compiled `xstance.net.dll` from `bin/Release/net452/` to the root folder
5. Copy `Newtonsoft.Json.dll` from the packages folder to the root folder

## Usage

### Controls
- **F6** (default) - Toggle stance menu (configurable in `config.lua`)
- **ESC** - Close menu
- **Command**: `/xstance` - Toggle menu via command

### Menu Features
- **Track Width Sliders**: Adjust wheel spacing (wider/narrower stance)
- **Camber Sliders**: Adjust wheel tilt angle
- **Reset Button**: Reset all values to default
- **Save Button**: Save current preset (future feature)

## Configuration

Edit `config.lua` to customize:

```lua
Config.ToggleMenuControl = 167  -- F6 key (change to your preference)
Config.FloatStep = 0.01         -- Adjustment precision
Config.WheelLimits = {          -- Maximum adjustment limits
    FrontTrackWidth = 0.25,
    RearTrackWidth = 0.25,
    FrontCamber = 0.20,
    RearCamber = 0.20,
}
```

## File Structure

```
xstance/
├── fxmanifest.lua          # Resource manifest
├── config.lua              # Configuration settings
├── client/
│   └── main.lua           # Client-side Lua script
├── html/
│   ├── index.html         # Menu HTML structure
│   ├── style.css          # Menu styling
│   └── script.js          # Menu functionality
├── src/                   # C# source code (optional)
│   ├── XStance.csproj
│   └── XStanceMain.cs
└── README.md
```

## Differences from Original VStancer

### Simplified Features
- Custom HTML/CSS/JS menu instead of MenuAPI
- Focused on core stance functionality only
- Removed server-side components
- Removed complex preset system
- Streamlined codebase

### Modern UI
- Responsive web-based interface
- Real-time slider adjustments
- Modern design with gradients and animations
- Mobile-friendly layout

### Reduced Dependencies
- No MenuAPI dependency
- Minimal file count
- Easier to customize and maintain

## Customization

### Changing the UI
Edit files in the `html/` folder:
- `style.css` - Modify colors, layout, animations
- `index.html` - Change structure, add elements
- `script.js` - Modify functionality, add features

### Adding New Features
The C# source code in `src/` can be modified to add:
- Server synchronization
- Preset saving/loading
- Advanced wheel modifications
- Additional vehicle parameters

## Troubleshooting

### Menu Not Opening
1. Check F6 key binding in `config.lua`
2. Ensure you're in the driver seat of a vehicle
3. Check console for errors

### Stance Not Applying
1. Verify the DLL files are present
2. Check that the vehicle supports stance modifications
3. Ensure decorators are enabled on your server

### UI Issues
1. Clear browser cache (if using in-game browser)
2. Check HTML/CSS/JS files for syntax errors
3. Verify file paths in `fxmanifest.lua`

## Credits

Based on the original VStancer by carmineos
- GitHub: https://github.com/carmineos/fivem-vstancer
- Simplified and customized for specific use cases

## License

This is a derivative work based on VStancer. Please respect the original license terms.