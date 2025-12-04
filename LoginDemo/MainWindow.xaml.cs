using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows;
using LoginDemo.Views;

namespace LoginDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainContent.Content = new LoginControl();
        }
        public void SwitchToAdmin()
        {
            MainContent.Content = new AdminControl();
        }
        public void SwitchToUser()
        {
            MainContent.Content = new UsersControl();
        }
        public void SwitchToLogin()
        {
            MainContent.Content = new LoginControl();
        }
        public void Login_Click()
        {

        }
    }
}