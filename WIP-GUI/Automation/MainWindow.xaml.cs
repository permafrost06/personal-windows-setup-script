using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Reflection;

namespace Automation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        static void SaveAndUpdateReg(string key, string valueName, string value)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey(key, true);
            string oldValue = (string)rk.GetValue(valueName);
            try
            {
                rk.SetValue(valueName, value);
            }
            //debug
            catch(Exception e)
            {
                Console.WriteLine("Exception: {0}", e);
            }

            string saveKey = key+"\\"+valueName;
            if (!CheckSettingExist(saveKey))
            { 
                AddUpdateAppSettings(saveKey, oldValue);
                Console.WriteLine("Saved: {0}", saveKey);   //debug
            }
            else
                Console.WriteLine("Key Exists: {0}", saveKey);  //debug
            rk.Close();
        }

        static void SaveAndUpdateRegDword(string key, string valueName, string value)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey(key, true);
            string oldValue = rk.GetValue(valueName).ToString();
            UInt32.TryParse(value, out uint DwordValue);
            try
            {
                rk.SetValue(valueName, DwordValue, RegistryValueKind.DWord);
            }
            //debug
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e);
            }

            string saveKey = key + "\\" + valueName;
            if (!CheckSettingExist(saveKey))
            {
                AddUpdateAppSettings(saveKey, "DWORD,"+oldValue);
                //debug
                Console.WriteLine("Saved: {0}", saveKey);
            }
            else
                //debug
                Console.WriteLine("Key Exists: {0}", saveKey);
            rk.Close();
        }

        static void ReadAllSettings()
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;

                if (appSettings.Count == 0)
                {
                    Console.WriteLine("AppSettings is empty.");
                }
                else
                {
                    foreach (var key in appSettings.AllKeys)
                    {
                        Console.WriteLine("Key: {0} Value: {1}", key, appSettings[key]);
                    }
                }
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
            }
        }

        static bool CheckSettingExist(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                if (appSettings[key] == null)
                    return false;
                else
                    return true;
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
                return false;
            }
        }

        static void AddUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }

        static void RunCMDCommand(string command)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C " + command;
            process.StartInfo = startInfo;
            process.Start();
        }

        private void ClockButton_Click(object sender, RoutedEventArgs e)
        {
            SaveAndUpdateReg("Control Panel\\International", "sShortTime", "HH:mm");
            SaveAndUpdateReg("Control Panel\\International", "sTimeFormat", "HH:mm:ss");
            SaveAndUpdateReg("Control Panel\\International", "iTime", "1");
            SaveAndUpdateReg("Control Panel\\International", "iTLZero", "1");
            SaveAndUpdateReg("Control Panel\\International", "iTimePrefix", "0");
            SaveAndUpdateReg("Control Panel\\International", "sTime", ":");
        }

        private void SetSaturdayButton_Click(object sender, RoutedEventArgs e)
        {
            SaveAndUpdateReg("Control Panel\\International", "iFirstDayOfWeek", "5");
        }

        private void InputOrderButton_Click(object sender, RoutedEventArgs e)
        {
            SaveAndUpdateReg("Keyboard Layout\\Preload", "1", "00000409");
            SaveAndUpdateReg("Keyboard Layout\\Preload", "2", "d0010409");
        }

        private void InputHotkeyButton_Click(object sender, RoutedEventArgs e)
        {
            SaveAndUpdateReg("Keyboard Layout\\Toggle", "Language Hotkey", "3");
            SaveAndUpdateReg("Keyboard Layout\\Toggle", "Hotkey", "3");
            SaveAndUpdateReg("Keyboard Layout\\Toggle", "Layout Hotkey", "1");
        }

        private void HideRecycleBinButton_Click(object sender, RoutedEventArgs e)
        {
            SaveAndUpdateRegDword("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\HideDesktopIcons\\NewStartPanel", "{645FF040-5081-101B-9F08-00AA002F954E}", "1");
        }

        private void PermanentDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            //todo
        }

        private void DeleteConfirmationButton_Click(object sender, RoutedEventArgs e)
        {
            SaveAndUpdateRegDword("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", "ConfirmFileDelete", "1");
        }

        private void ShowExtensionsButton_Click(object sender, RoutedEventArgs e)
        {
            SaveAndUpdateRegDword("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced", "HideFileExt", "0");
        }

        private void ThisPCButton_Click(object sender, RoutedEventArgs e)
        {
            SaveAndUpdateRegDword("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced", "LaunchTo", "1");
        }

        private void SmallIconsButton_Click(object sender, RoutedEventArgs e)
        {
            SaveAndUpdateRegDword("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced", "TaskbarSmallIcons", "1");
        }

        private void ShowLabelsButton_Click(object sender, RoutedEventArgs e)
        {
            SaveAndUpdateRegDword("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced", "TaskbarGlomLevel", "1");
        }

        private void HideSearchButton_Click(object sender, RoutedEventArgs e)
        {
            SaveAndUpdateRegDword("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced", "ShowCortanaButton", "0");
        }

        private void HideTaskViewButton_Click(object sender, RoutedEventArgs e)
        {
            SaveAndUpdateRegDword("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced", "ShowTaskViewButton", "0");
        }

        private void HideSearchboxButton_Click(object sender, RoutedEventArgs e)
        {
            SaveAndUpdateRegDword("Software\\Microsoft\\Windows\\CurrentVersion\\Search", "SearchboxTaskbarMode", "0");
        }

        private void ApplyAllButton_Click(object sender, RoutedEventArgs e)
        {
            ClockButton_Click(sender, e);
            SetSaturdayButton_Click(sender, e);
            InputOrderButton_Click(sender, e);
            InputHotkeyButton_Click(sender, e);
            HideRecycleBinButton_Click(sender, e);
            PermanentDeleteButton_Click(sender, e);
            DeleteConfirmationButton_Click(sender, e);
            ShowExtensionsButton_Click(sender, e);
            ThisPCButton_Click(sender, e);
            SmallIconsButton_Click(sender, e);
            ShowLabelsButton_Click(sender, e);
            HideSearchButton_Click(sender, e);
            HideTaskViewButton_Click(sender, e);
            HideSearchboxButton_Click(sender, e);
            RunCMDCommand("taskkill /f /im explorer.exe && start explorer.exe");
        }

        private void ApplyPresetButton_Click(object sender, RoutedEventArgs e)
        {
            ReadAllSettings();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void RevertButton_Click(object sender, RoutedEventArgs e)
        {
            var appSettings = ConfigurationManager.AppSettings;

            if (appSettings.Count == 0)
            {
                Console.WriteLine("AppSettings is empty."); //debug
            }
            else
            {
                foreach (var key in appSettings.AllKeys)
                {
                    //Console.WriteLine("Key: {0} Value: {1}", key, appSettings[key]);    //debug
                    string[] param = appSettings[key].Split(',');
                    string[] exp = key.Split('\\');
                    string valueName = exp[exp.Length - 1];
                    string newkey = key.Replace("\\"+valueName, "");
                    
                    if (param[0] == "DWORD")
                    {
                        
                        SaveAndUpdateRegDword(newkey, valueName, param[1]);
                    }
                    else
                    {
                        SaveAndUpdateReg(newkey, valueName, param[0]);
                    }
                }
            }
            RunCMDCommand("taskkill /f /im explorer.exe && start explorer.exe");
        }
    }
}