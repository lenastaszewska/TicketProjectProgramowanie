using TicketProject.Classes;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;



namespace WpfTicketProject.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        HttpClient _httpClient;
        ObservableCollection<User> users = new();
        public AdminWindow()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5298/api");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            LoadUsersToDataGrid();
            LoadTransactionToDataGrid();
        }

     

        public async void LoadUsersToDataGrid()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5298/api/Users");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                users = JsonConvert.DeserializeObject<ObservableCollection<User>>(responseBody);

                userDataGrid.ItemsSource = users;


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas pobierania danych: {ex.Message}");
            }
        }

        public async void LoadTransactionToDataGrid()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5298/api/Transactions");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                ObservableCollection<Transactions> transaction = JsonConvert.DeserializeObject<ObservableCollection<Transactions>>(responseBody);

                transactionDataGrid.ItemsSource = transaction;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas pobierania danych: {ex.Message}");
            }
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            AddUser addUser = new AddUser();
            addUser.ShowDialog();
            LoadUsersToDataGrid();
            
        }
         private void UpdateUserDoubleClick(object sender, RoutedEventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                if (dataGrid.SelectedItem is User selectedUser)
                {

                    UpdateUser updateStudentWindow = new UpdateUser(selectedUser);
                    updateStudentWindow.ShowDialog();
                    LoadUsersToDataGrid();

                }
            }
        }

        private void UpdateTransactionDoubleClick(object sender,System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                if (dataGrid.SelectedItem is Transactions selectedTransactions)
                {

                    UpdateTicket updateTicket = new UpdateTicket(selectedTransactions, users);
                    updateTicket.ShowDialog();
                    LoadTransactionToDataGrid();

                }
            }
        }

        private async void btnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (userDataGrid.SelectedItem is User selectedUser)
            {

                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        HttpResponseMessage response = await client.DeleteAsync($"http://localhost:5298/api/Users/{selectedUser.UserName}");
                        response.EnsureSuccessStatusCode();
                        MessageBox.Show("Student został usunięty.");
                        LoadUsersToDataGrid();
                    }
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show($"Wystąpił błąd podczas usuwania studenta: {ex.Message}");
                }

            }
        }

        private void btnAddTicket_Click(object sender, RoutedEventArgs e)
        {
            AddTicket addTicket = new AddTicket(users);
            addTicket.ShowDialog();
            LoadTransactionToDataGrid();
        }

        private async void btnDeleteTicket_Click(object sender, RoutedEventArgs e)
        {
            if (transactionDataGrid.SelectedItem is Transactions selectedTransactions)
            {

                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        HttpResponseMessage response = await client.DeleteAsync($"http://localhost:5298/api/Transactions/{selectedTransactions.UserNameID}");
                        response.EnsureSuccessStatusCode();
                        MessageBox.Show("Grade has been removed.");
                        LoadTransactionToDataGrid();
                    }
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

    }
}
