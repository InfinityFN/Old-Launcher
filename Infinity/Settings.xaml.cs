using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.IO;
using Ookii.Dialogs.Wpf;
using Newtonsoft.Json;

namespace Infinity
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void FNPathClick(object sender, RoutedEventArgs e)
        {
            var dialog = new VistaFolderBrowserDialog();

            if (dialog.ShowDialog() == true)
            {
                FortnitePathBox.Text = dialog.SelectedPath;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Infinity.Helpers.Globals.FortnitePath = FortnitePathBox.Text;
        }

        private void FortniteConsoleCheck(object sender, RoutedEventArgs e)
        {
            Infinity.Helpers.Globals.injectConsole = true;
        }

        private void FortniteConsoleUncheck(object sender, RoutedEventArgs e)
        {
            Infinity.Helpers.Globals.injectConsole = false;
        }

        private void SaveConfigBtn_Click(object sender, RoutedEventArgs e)
        {
            // Get the directory where the executable is located
            string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Construct the path of the JSON file
            string jsonPath = Path.Combine(exeDirectory, "config.json");

            // Create an anonymous object to store the email and password
            var credentials = new { Email = Infinity.Helpers.Globals.email, Password = Infinity.Helpers.Globals.password, FNPath = Infinity.Helpers.Globals.FortnitePath };

            // Serialize the object to JSON
            string json = JsonConvert.SerializeObject(credentials);

            // Write the JSON to the file
            File.WriteAllText(jsonPath, json);

            MessageBox.Show("Configuration saved successfully.");
        }

        private void ResetConfigBtn_Click(object sender, RoutedEventArgs e)
        {
            // Get the directory where the executable is located
            string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Construct the path of the JSON file
            string jsonPath = Path.Combine(exeDirectory, "config.json");

            File.Delete(jsonPath);

            MessageBox.Show("Configuration reset successfully!");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Get the directory where the executable is located
            string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Construct the path of the JSON file
            string jsonPath = Path.Combine(exeDirectory, "config.json");

            // Check if the JSON file exists
            if (!File.Exists(jsonPath))
            {
                return;
            }

            // Read the JSON from the file
            string json = File.ReadAllText(jsonPath);

            // Deserialize the JSON to an object
            var credentials = JsonConvert.DeserializeObject<dynamic>(json);

            // Set the email and password inputs
            FortnitePathBox.Text = credentials.FNPath;

            //MessageBox.Show("Configuration loaded successfully.");
        }
    }
}
