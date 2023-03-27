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
using System.Windows.Navigation;
//using System.Windows.Shapes;
using System.Net.Http;
using System.Text.Json;
using System.Security.Cryptography;
using System.IO;

namespace Infinity
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

        private async void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            // Get the text from the PasswordBox
            var secureString = PasswordBox.SecurePassword;
            var password = new System.Net.NetworkCredential(string.Empty, secureString).Password;

            var client = new HttpClient();
            var url = "APIENDPOINT" + EmailBox.Text + "/" + password;

            var response = await client.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();

            //Console.WriteLine($"Response status code: {response.StatusCode}");
            //Console.WriteLine($"Response content: {responseContent}");

            if(responseContent == "Err:404")
            {
                MessageBox.Show("This account does not exist");
                return;
            } else if(responseContent == "Err:401")
            {
                MessageBox.Show("Email or Password is incorrect");
            } else
            {
                // Deserialize the response content into a JSON object
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var jsonObject = JsonSerializer.Deserialize<JsonElement>(responseContent, options);

                var displayName = jsonObject.GetProperty("displayName").GetString();
                //MessageBox.Show("Welcome, " + displayName);

                // Access the items property of the character object
                var AthenaCharacter = jsonObject.GetProperty("profile")
                                     .GetProperty("character")
                                     .GetProperty("items").GetString();

                var Vbucks = jsonObject.GetProperty("profile")
                                     .GetProperty("vbucks").GetInt32();

                string VbucksString = string.Format("{0:N0}", Vbucks);

                var parts = AthenaCharacter.Split(':');

                Infinity.Helpers.Globals.displayName = displayName;
                Infinity.Helpers.Globals.password = password;
                if (parts.Length > 1)
                {
                    Infinity.Helpers.Globals.currentCID = parts[1];
                } else
                {
                    Infinity.Helpers.Globals.currentCID = "CID_001_Athena_Commando_F_Default";
                }
                
                Infinity.Helpers.Globals.email = EmailBox.Text;
                Infinity.Helpers.Globals.vbucksAmount = VbucksString;

                var newCID = "";

                // get skin icon
                if (parts.Length > 1)
                {
                    newCID = parts[1];
                } else
                {
                    newCID = "CID_001_Athena_Commando_F_Default";
                }
                

                var client2 = new HttpClient();
                var url2 = "https://fortnite-api.com/v2/cosmetics/br/" + newCID;

                var response2 = await client2.GetAsync(url2);
                var responseContent2 = await response2.Content.ReadAsStringAsync();


                // Deserialize the response content into a JSON object
                var options2 = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var jsonObject2 = JsonSerializer.Deserialize<JsonElement>(responseContent2, options2);

                var Icon = jsonObject2.GetProperty("data")
                                         .GetProperty("images")
                                         .GetProperty("smallIcon").GetString();

                Infinity.Helpers.Globals.AthenaCharacterIcn = Icon;

                Home home = new Home();
                home.Show();
                this.Close();
            }
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
            var credentials = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(json);

            // Set the email and password inputs
            EmailBox.Text = credentials.Email;
            PasswordBox.Password = credentials.Password;
            Infinity.Helpers.Globals.FortnitePath = credentials.FNPath;

            //MessageBox.Show("Configuration loaded successfully.");
        }
    }
}
