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
using System.Windows.Shapes;
using Microsoft.Data.Sqlite;
using System.Data;


namespace LoginDemo.Views
{
    /// <summary>
    /// Логика взаимодействия для AdminControl.xaml
    /// </summary>
    public partial class AdminControl : UserControl
    {
        private string dbPath = @"Data Source=C:\Program Files (x86)\DB Browser for SQLite\authdemo.db";
        public AdminControl()
        {
            InitializeComponent();
            LoadUsers();
        }
        private void LoadUsers()
        {
            using var connection = new SqliteConnection(dbPath);
            connection.Open();
            using var command = new SqliteCommand(
                "SELECT * FROM Users", connection);
            using var reader = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            UsersGrid.ItemsSource = dt.DefaultView;
        }

        private void UsersGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e) //AddUser_Click
        {
            string login = InputLogin.Text;
            string password = InputPassword.Text;
            string role = (InputRole.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Заполните все поля.");
                return;
            }
            using var connection = new SqliteConnection(dbPath);
            connection.Open();
            using var command = new SqliteCommand(
                "INSERT INTO Users (Login, Password, Role) VALUES ($l, $p, $r)", connection);
            command.Parameters.AddWithValue("$l", login);
            command.Parameters.AddWithValue("$p", password);
            command.Parameters.AddWithValue("$r", role);
            command.ExecuteNonQuery();
            InputLogin.Clear();
            InputPassword.Clear();
            InputRole.SelectedIndex = -1;
            LoadUsers();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) //DeleteUser_Click
        {
            if (sender is Button btn && btn.DataContext is DataRowView row)
            {
                string login = row["Login"].ToString();
                string password = row["Password"].ToString();
                int id = Convert.ToInt32(row["ID"]);

                if (login == "admin" && password == "admin")
                {
                    MessageBox.Show("Главного администратора удалить нельзя!");
                    return;
                }
                if (MessageBox.Show($"Удалить пользователя ID={id}?", "Удаление", MessageBoxButton.YesNo) != MessageBoxResult.Yes) {
                    return;
                }
                using var connection = new SqliteConnection(dbPath);
                connection.Open();
                using var command = new SqliteCommand(
                    "DELETE FROM Users WHERE Id = $id", connection);
                command.Parameters.AddWithValue("$id", id);
                command.ExecuteNonQuery();
                LoadUsers();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e) //back
        {
            (Window.GetWindow(this) as MainWindow)?.SwitchToLogin();
        }
    }
}
