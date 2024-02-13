using TicketProject.Classes;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;
using System.Net.Http.Json;



namespace WpfTicketProject.Windows
{
    public partial class UpdateUser : Window
    {
        private User selectedUser;
        private AdminWindow adminWindow = new AdminWindow();

        public UpdateUser(User selectedUser)
        {
            InitializeComponent();
            this.selectedUser = selectedUser;
            NameTextBox.Text = selectedUser.name;
            SurnameTextBox.Text = selectedUser.surname;
            EmailTextBox.Text = selectedUser.email;
        }

        private async void UpdateUserBtn_Click(object sender, RoutedEventArgs e)
        {
            selectedUser.name = NameTextBox.Text;
            selectedUser.surname = SurnameTextBox.Text;
            selectedUser.email = EmailTextBox.Text;

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.PutAsJsonAsync($"http://localhost:5298/api/Users/{selectedUser.UserName}", selectedUser);
                    response.EnsureSuccessStatusCode();

                    MessageBox.Show("Dane studenta zostały zaktualizowane pomyślnie.");

                    this.Close();
                    adminWindow.LoadUsersToDataGrid();
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show($"Wystąpił błąd podczas aktualizacji danych: {ex.Message}");
                }
            }
        }
    }
}
