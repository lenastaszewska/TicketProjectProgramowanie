using TicketProject.Classes;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;
using System.Net.Http.Json;

namespace WpfTicketProject.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy AddTicket.xaml
    /// </summary>
    public partial class AddTicket : Window
    {
        HttpClient client;
        public AddTicket(ObservableCollection<User> users)
        {
            InitializeComponent();
            client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5298");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            UserNameComboBox.ItemsSource = users.Select(user => user.UserName);
        }

        private async void AddTicketBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Int32.TryParse(UserNameComboBox.Text, out int selectedUserName))
                {
                    var newTransactions = new
                    {
                        UserName = selectedUserName,
                        price = Convert.ToDouble(priceTextBox.Text),
                        place = placeTextBox.Text,
                        artist = artistTextBox.Text
                    };

                    var response = await client.PostAsJsonAsync("/api/Transactions", newTransactions);
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Transaction has been added.");
                    }
                    else
                    {
                        MessageBox.Show($"Failed to add transaction. Error: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a valid data.");
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"HttpRequestException: {ex.Message}");
                if (ex.InnerException != null)
                {
                    MessageBox.Show($"Inner Exception: {ex.InnerException.Message}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
