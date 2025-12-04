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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Data.Sqlite;
using SQLitePCL;

namespace LoginDemo.Views
{
    /// <summary>
    /// Логика взаимодействия для LoginControl.xaml
    /// </summary>
    public partial class LoginControl : UserControl
    {
        private string dbPath = @"Data Source=C:\Program Files (x86)\DB Browser for SQLite\authdemo.db";
        public LoginControl()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string login = InputLogin.Text;
            string password = InputPassword.Password;

            using var connection = new SqliteConnection(dbPath);
            connection.Open();

            using var command = new SqliteCommand(
                "SELECT Role FROM Users WHERE Login = $l AND Password = $p", connection);
            command.Parameters.AddWithValue("$l", login);
            command.Parameters.AddWithValue("$p", password);

            var role = command.ExecuteScalar()?.ToString();

            if (role == null)
            {
                InfoText.Text = "Неверный логин или пароль.";
                return;
            }
            else if (role == "Admin")
            {
                (Window.GetWindow(this) as MainWindow)?.SwitchToAdmin();
            }
            else
            {
                (Window.GetWindow(this) as MainWindow)?.SwitchToUser();
            }
        }
    }
}
