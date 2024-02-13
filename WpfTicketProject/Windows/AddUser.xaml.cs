using TicketProject.Classes;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;
using System.Net.Http.Json;


namespace WpfTicketProject.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy AddUser.xaml
    /// </summary>
    public partial class AddUser : Window
    {
        HttpClient client;
        public AddUser()
        {
            InitializeComponent();
            client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5298");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        }
        private async void AddUserBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newUser = new
                {
                    name = NameTextBox.Text,
                    surname = SurnameTextBox.Text,
                    email = EmailTextBox.Text

                };

                var response = await client.PostAsJsonAsync("/api/Users", newUser);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Student has been added.");

                }
                else
                {
                    MessageBox.Show("Failed to add student.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

    }
}
