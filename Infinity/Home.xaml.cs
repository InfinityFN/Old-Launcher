using Infinity.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Infinity
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        public Home()
        {
            InitializeComponent();
        }

        private async void Home_Loaded(object sender, RoutedEventArgs e)
        {
            UsernameBox.Content = Infinity.Helpers.Globals.displayName;
            VbucksCounter.Content = Infinity.Helpers.Globals.vbucksAmount;
            //MessageBox.Show(Infinity.Helpers.Globals.AthenaCharacterIcn);

            var client = new HttpClient();
            var url = "APIENDPOINT";

            var response = await client.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();

            //Console.WriteLine($"Response status code: {response.StatusCode}");
            //Console.WriteLine($"Response content: {responseContent}");

            OnlineCounter.Content = responseContent;

            try
            {
                using (var webClient = new System.Net.WebClient())
                {
                    var data = await webClient.DownloadDataTaskAsync(Infinity.Helpers.Globals.AthenaCharacterIcn);
                    using (var stream = new System.IO.MemoryStream(data))
                    {
                        var bitmapImage = new System.Windows.Media.Imaging.BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
                        bitmapImage.StreamSource = stream;
                        bitmapImage.EndInit();

                        AthenaIcon.Source = bitmapImage;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur while downloading or displaying the image
            }
        }

        private async void Home_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var client = new HttpClient();
            var url = "APIENDPOINT";

            var response = await client.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();
        }

        private void SettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings();
            settings.Show();
        }

        private void LaunchFNBtn_Click(object sender, RoutedEventArgs e)
        {
            //Process process = ProcessHelper.StartProcess(Infinity.Helpers.Globals.FortnitePath + "\\FortniteGame\\Binaries\\Win64\\FortniteLauncher.exe", true, "");
            //Process process2 = ProcessHelper.StartProcess(Infinity.Helpers.Globals.FortnitePath + "\\FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping_BE.exe", true, "");
            //Process process4 = ProcessHelper.StartProcess(Infinity.Helpers.Globals.FortnitePath + "\\FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping_EAC.exe", true, "");
            //Process process3 = ProcessHelper.StartProcess(Infinity.Helpers.Globals.FortnitePath + "\\FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping.exe", false, "-AUTH_TYPE=epic -AUTH_LOGIN=\"" + Infinity.Helpers.Globals.email + "\" -AUTH_PASSWORD=\"" + Infinity.Helpers.Globals.password + "\" -SKIPPATCHCHECK");
            //process3.WaitForInputIdle();
            //ProcessHelper.InjectDll(process3.Id, System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "InfinityRedirect.dll"));
            //process3.WaitForExit();
            //process.Close();
            //process2.Close();
            //process3.Close();
            //process4.Close();
        }
    }
}
