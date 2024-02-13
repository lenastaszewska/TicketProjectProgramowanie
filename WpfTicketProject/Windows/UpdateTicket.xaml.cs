using TicketProject.Classes;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;
using System.Net.Http.Json;

namespace WpfTicketProject.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy UpdateTicket.xaml
    /// </summary>
    public partial class UpdateTicket : Window
    {
        private Transactions selectedTransactions;
        private ObservableCollection<User> users;
        private AdminWindow adminWindow = new AdminWindow();
        public UpdateTicket(Transactions selectedTransactions, ObservableCollection<User> users)
        {
            InitializeComponent();
            UserNameComboBox.ItemsSource = selectedTransactions.UserName.ToString();
            PriceTextBox.Text = selectedTransactions.price.ToString();
            PlaceTextBox.Text = selectedTransactions.place;
            ArtistTextBox.Text = selectedTransactions.artist;
            this.selectedTransactions = selectedTransactions;
            this.users = users;
            UserNameComboBox.ItemsSource = users.Select(user => user.UserName); ;

            UserNameComboBox.Loaded += (s, e) =>
            {
                // Sprawdzenie, czy selectedGrade.index znajduje się na liście
                if (users.Any(user => user.UserName == selectedTransactions.UserName))
                {
                    UserNameComboBox.Text = selectedTransactions.UserName.ToString();
                }
            };
        }
        private async void UpdateTicketBtn_Click(object sender, RoutedEventArgs e)
        {


            selectedTransactions.UserName = (int)UserNameComboBox.SelectedItem;
            selectedTransactions.price = Int32.Parse(PriceTextBox.Text);
            selectedTransactions.place = PlaceTextBox.Text;
            selectedTransactions.artist = ArtistTextBox.Text;

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.PutAsJsonAsync($"http://localhost:5298/api/Transactions/{selectedTransactions.UserNameID}", selectedTransactions);
                    response.EnsureSuccessStatusCode();

                    MessageBox.Show("Ocena została zaktualizowana pomyślnie.");

                    this.Close();
                    adminWindow.LoadTransactionToDataGrid();
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show($"Wystąpił błąd podczas aktualizacji danych: {ex.Message}");
                }
            }
        }
    }
}
