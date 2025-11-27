# Windows Start Menu Programs Shortcut Maker

A simple Windows utility that allows you to quickly create shortcuts in your Start Menu → Programs folder for any file or folder on your system.

## What is this program?

This program provides an easy way to add shortcuts to your Windows Start Menu Programs folder. Instead of manually creating shortcuts or navigating through system folders, you can simply right-click any file or folder and use the "Send To" menu to create a shortcut instantly.

## Features

- **Easy Installation**: One-time setup adds the program to your "Send To" menu
- **Quick Access**: Right-click any file or folder to create a Start Menu shortcut
- **Automatic Naming**: Shortcuts are automatically named based on the file or folder name
- **Duplicate Handling**: Automatically handles duplicate names by appending numbers
- **Works with Files and Folders**: Create shortcuts for both executable files and directories

## How to Use

### Step 1: Initial Setup (One-time)

1. Run the program by double-clicking `Add to Start Menu Programs.exe`
2. A message will appear confirming that the program has been installed to your "Send To" menu
3. The program will now appear as **"Start Menu - Programs (create shortcut)"** in your Send To menu

### Step 2: Creating Shortcuts

1. Navigate to any file or folder you want to add to your Start Menu
2. Right-click on the file or folder
3. Select **Send to** → **Start Menu - Programs (create shortcut)**
4. The shortcut will be created in your Start Menu → Programs folder
5. You can now access it from the Start Menu

### Example Usage

- **Add an application**: Right-click an `.exe` file → Send to → Start Menu - Programs (create shortcut)
- **Add a folder**: Right-click a folder → Send to → Start Menu - Programs (create shortcut)
- **Add a document**: Right-click any file → Send to → Start Menu - Programs (create shortcut)

## How It Works

- **First Run**: When you run the program without any arguments, it installs itself into your Windows "Send To" folder (`%APPDATA%\Microsoft\Windows\SendTo`)
- **Creating Shortcuts**: When you use "Send To" with a file or folder, Windows passes the selected path(s) as arguments to the program, which then creates the appropriate shortcut(s) in your Start Menu Programs folder

## Requirements

- Windows operating system
- .NET Framework 4.7.2 or higher

## Notes

- Shortcuts are created in your user's Start Menu Programs folder (not the system-wide folder)
- If a shortcut with the same name already exists, the program will automatically append a number (e.g., "MyApp (1).lnk")
- The shortcut name is based on the file or folder name (without extension)
- You can run the setup again if needed - it will detect if it's already installed

## Troubleshooting

- **"Failed to install" error**: Make sure you have write permissions to your Send To folder
- **Shortcut not appearing in Start Menu**: Try refreshing the Start Menu or restarting Windows Explorer
- **"Target file or directory not found"**: The file or folder you're trying to create a shortcut for may have been moved or deleted

