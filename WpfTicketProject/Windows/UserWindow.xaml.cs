using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;
using Newtonsoft.Json;
using TicketProject.Classes;

namespace WpfTicketProject.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        private readonly HttpClient _httpClient;

        public UserWindow()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5298/api/Transactions");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        private async void ShowTicketsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int userName;
                if (!int.TryParse(LoginTextBox.Text, out userName))
                {
                    MessageBox.Show("Wprowadź poprawny indeks.");
                    return;
                }

                HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5298/api/Transactions/{userName}");

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    List<Transactions> transactions = JsonConvert.DeserializeObject<List<Transactions>>(responseBody);

                    if (transactions != null && transactions.Count > 0)
                    {
                        ticketsDataGrid.ItemsSource = transactions;
                    }
                    else
                    {
                        MessageBox.Show("Brak danych dla podanego użytkownika.");
                    }
                }
                else
                {
                    MessageBox.Show($"Wystąpił błąd podczas pobierania danych. Kod błędu: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas pobierania danych: {ex.Message}");
            }
        }
    }
}