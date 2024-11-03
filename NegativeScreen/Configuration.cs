//Copyright 2023 Josiah Bergen

//This file is part of NegativeScreen.

//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace NegativeScreen
{
	class Configuration
	{

        public int configFileExists = 0; 
        public string configFilePath = "";

        public int DefaultInversionMethod { get; set; }
        public int InvertOnLaunch { get; set; }

        private string defaultConfigText = "# NegativeScreen Configuration File\n\n\n\n# ------ Instructions ------\n\n# Use this file to modify some of the behaviors of the program.\n# Hotkey customizations coming soon!\n# Comments begin with a hashtag.\n\n#True/false are represented my 0/1\n\n# Inversion modes:\n\n# 0 - Negative\n# 1 - NegativeHueShift180 (high saturation, good pure colors)\n# 2 - NegativeHueShift180Variation1 (overall desaturated, yellows and blue plain bad. actually relaxing and very usable)\n# 3 - NegativeHueShift180Variation2 (high saturation. yellows and blues plain bad. actually quite readable)\n# 4 - NegativeHueShift180Variation3 (not so readable, good colors. CMY colors a bit desaturated, still more saturated than normal)\n# 5 - NegativeHueShift180Variation4 (no description)\n# 6 - NegativeSepia\n# 7 - NegativeGrayScale\n# 8 - NegativeRed\n# 9 - Red\n\n\n# Whether the program will invert the screen right away or wait for user input (true/false)\ninvert_on_launch = 1\n\n#The default inversion mode.\ndefault_inversion_mode = 0\n\n# Hotkey customization coming soon(ish)!";

        public Configuration() 
        {
            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appDataPath = Path.Combine(appDataFolder, "NegativeScreen", "negativescreen.conf");
            string exePath = AppDomain.CurrentDomain.BaseDirectory;

            // get the number of screens connected 
            foreach (var item in Screen.AllScreens)
			{
                defaultConfigText += "";
            }

            if (File.Exists(appDataPath))
            {
                configFilePath = appDataPath;
                ReadConfigurationFile(configFilePath);
            }
            else
            {
                if (File.Exists(exePath))
                {
                    configFilePath = exePath;
                    ReadConfigurationFile(configFilePath);
                }
                else
                {
                    CreateConfigurationFile();
                }
            }
        }

        public bool ReadConfigurationFile(string filePath)
        {
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while (!reader.EndOfStream)
                    {
                        line = reader.ReadLine()?.Trim();
                        if (!string.IsNullOrEmpty(line) && !line.StartsWith("#"))
                        {
                            string[] parts = line.Split('=');
                            if (parts.Length == 2)
                            {
                                string key = parts[0].Trim();
                                string value = parts[1].Trim();

                                // Parse and set the configuration values based on the key
                                if (key == "default_inversion_mode")
                                {
                                    if (int.TryParse(value, out int intValue))
                                    {
                                        DefaultInversionMethod = intValue;
                                    }
                                }

                                if (key == "invert_on_launch")
                                {
                                    if (int.TryParse(value, out int intValue))
                                    {
                                        InvertOnLaunch = intValue;
                                    }
                                }

                                // more keys can go here
                            }
                        }
                    }
                    return true;
                }
            }
            catch
            {
                MessageBox.Show("An error occured.", "File Content", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }

        public bool CreateConfigurationFile() 
        {
            string directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "NegativeScreen");
            string filePath = Path.Combine(directoryPath, "negativescreen.conf");

            // create negativescreen directory
            try
            {
                Directory.CreateDirectory(directoryPath);

                try
                {
                    File.WriteAllText(filePath, defaultConfigText);
                    return true;
                }
                catch
                {
                    MessageBox.Show("Failed to create a configuration file", "File Status", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch
            {
                MessageBox.Show("Failed to create a configuration file directory.", "File Status", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }
    }
}

