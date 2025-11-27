using System;
using System.IO;
using System.Windows.Forms;
using IWshRuntimeLibrary;

namespace start_menu_programs_shortcut_maker
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // If run directly (double-click, etc.), install into SendTo
            if (args == null || args.Length == 0)
            {
                try
                {
                    InstallToSendTo();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Failed to install into 'Send to' menu:\n\n" + ex.Message,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }

                return;
            }

            // If run with arguments, treat them as selected items from Send To
            foreach (var inputPath in args)
            {
                try
                {
                    AddToStartMenuPrograms(inputPath);
                }
                catch (Exception ex)
                {
                    // Show one error per file (or you could just ignore errors)
                    MessageBox.Show(
                        $"Error processing '{inputPath}':\n\n{ex.Message}",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
        }

        /// <summary>
        /// Creates a shortcut to this EXE in the user's SendTo folder.
        /// Copies the EXE to a standard location first so the user doesn't need to keep the original.
        /// </summary>
        private static void InstallToSendTo()
        {
            string exePath = Application.ExecutablePath; // path of this EXE
            string exeFileName = Path.GetFileName(exePath);

            // Copy EXE to LocalAppData\Programs\Start Menu Programs Shortcut Maker\
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            if (string.IsNullOrEmpty(localAppData))
                throw new InvalidOperationException("Could not locate the LocalAppData folder.");

            string installFolder = Path.Combine(localAppData, "Programs", "Start Menu Programs Shortcut Maker");
            if (!Directory.Exists(installFolder))
                Directory.CreateDirectory(installFolder);

            string installedExePath = Path.Combine(installFolder, exeFileName);

            // Copy the EXE if it doesn't exist or if the source is newer
            bool needsCopy = true;
            if (System.IO.File.Exists(installedExePath))
            {
                DateTime sourceTime = System.IO.File.GetLastWriteTime(exePath);
                DateTime destTime = System.IO.File.GetLastWriteTime(installedExePath);
                needsCopy = sourceTime > destTime;
            }

            if (needsCopy)
            {
                System.IO.File.Copy(exePath, installedExePath, overwrite: true);
            }

            // Create shortcut in SendTo folder
            string sendToFolder = Environment.GetFolderPath(Environment.SpecialFolder.SendTo);
            if (string.IsNullOrEmpty(sendToFolder))
                throw new InvalidOperationException("Could not locate the 'SendTo' folder.");

            if (!Directory.Exists(sendToFolder))
                Directory.CreateDirectory(sendToFolder);

            string shortcutName = "Start Menu - Programs (create shortcut).lnk";
            string shortcutPath = Path.Combine(sendToFolder, shortcutName);

            // Check if shortcut exists and points to the installed location
            bool shortcutExists = System.IO.File.Exists(shortcutPath);
            var shell = new WshShell();
            
            if (shortcutExists)
            {
                // Verify it points to the correct location
                IWshShortcut existingShortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
                if (existingShortcut.TargetPath.Equals(installedExePath, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show(
                        "The 'Send to → Start Menu - Programs (create shortcut)' entry is already installed.",
                        "Already installed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                    return;
                }
            }

            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);

            shortcut.TargetPath = installedExePath;
            shortcut.WorkingDirectory = Path.GetDirectoryName(installedExePath);
            shortcut.Description = "Create a shortcut in Start Menu → Programs for the selected file.";
            shortcut.IconLocation = installedExePath;

            shortcut.Save();

            MessageBox.Show(
                "The 'Send to → Start Menu - Programs (create shortcut)' entry has been installed successfully.\n\n" +
                "You can now delete this installer if you wish.",
                "Installed",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        /// <summary>
        /// Creates a shortcut in Start Menu\Programs pointing to the given target.
        /// </summary>
        private static void AddToStartMenuPrograms(string targetPath)
        {
            if (string.IsNullOrWhiteSpace(targetPath))
                throw new ArgumentException("Target path is empty.", nameof(targetPath));

            targetPath = Path.GetFullPath(targetPath);

            if (!System.IO.File.Exists(targetPath) && !Directory.Exists(targetPath))
                throw new FileNotFoundException("Target file or directory not found.", targetPath);

            // Start Menu\Programs for current user
            string startMenu = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
            string programsFolder = Path.Combine(startMenu, "Programs");

            if (!Directory.Exists(programsFolder))
                Directory.CreateDirectory(programsFolder);

            // Shortcut name based on file/folder name (no extension)
            string name = Path.GetFileNameWithoutExtension(targetPath);
            if (string.IsNullOrWhiteSpace(name))
                name = "Shortcut";

            string shortcutPath = Path.Combine(programsFolder, name + ".lnk");

            // Ensure unique name
            int counter = 1;
            while (System.IO.File.Exists(shortcutPath))
            {
                shortcutPath = Path.Combine(programsFolder, $"{name} ({counter}).lnk");
                counter++;
            }

            var shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);

            shortcut.TargetPath = targetPath;
            shortcut.WorkingDirectory = Path.GetDirectoryName(targetPath);
            shortcut.WindowStyle = 1;
            shortcut.Description = "Shortcut created via Send To → Start Menu/Programs";
            shortcut.IconLocation = targetPath; // or a custom .ico, if you prefer

            shortcut.Save();
        }
    }
}
