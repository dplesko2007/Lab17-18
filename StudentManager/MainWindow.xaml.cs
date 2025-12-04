using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using Microsoft.Data.Sqlite;

namespace StudentManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string ConnectionString = @"Data Source=C:\Program Files (x86)\DB Browser for SQLite\students2.db";
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}","Ошибка",MessageBoxButton.OK,MessageBoxImage.Error);
                throw;
            }
        }

        private void LoadData()
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var command = new SqliteCommand("SELECT ID, Name FROM Students ORDER BY ID",connection);
            using var reader = command.ExecuteReader();
            var dt = new DataTable();
            dt.Load(reader);
            DataGridPeople.ItemsSource = dt.DefaultView;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var name = InputName.Text?.Trim();
            if (string.IsNullOrEmpty(name) )
            {
                MessageBox.Show(
                    $"Введите имя перед добавлением",
                    "Внимание",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                    );
            }
            try
            {
                using var connection = new SqliteConnection(ConnectionString);
                connection.Open();
                using var command = new SqliteCommand($"INSERT INTO Students (Name) VALUES ($name);", connection);
                command.Parameters.AddWithValue("name", name);
                command.ExecuteNonQuery();
                LoadData();
                InputName.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ошибка при добавлении: {ex.Message}",
                    "Внимание",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                    );
                throw;
            }
            
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridPeople.SelectedItem is not DataRowView row)
            {
                MessageBox.Show("Выберите запись для удаления","Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            long idLong;
            try
            {
                idLong = Convert.ToInt64(row["ID"]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Не удалось прочитать ID выбранной записи.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var answer = MessageBox.Show($"Удалить запись с ID = {idLong}?", "Пдтвердить удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer != MessageBoxResult.Yes) return;
            try
            {
                using var connection = new SqliteConnection(ConnectionString);
                connection.Open();
                var command = new SqliteCommand($"DELETE FROM Students WHERE ID = {idLong};", connection);
                command.Parameters.AddWithValue("$id", idLong);
            }
            catch (Exception ex)
            {

                throw;
            }
            LoadData();
            

            //var row = (DataRowView)DataGridPeople.SelectedItem;
            //long ID = Convert.ToInt64(row["ID"]);
            //using var connection = new SqliteConnection(ConnectionString);
            //connection.Open();
            //var command = new SqliteCommand($"DELETE FROM Students WHERE ID = {ID};", connection);
            //command.ExecuteNonQuery();
        }
    }
}