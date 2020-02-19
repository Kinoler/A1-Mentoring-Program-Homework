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

namespace FrameworkForms
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var userName = UserNameTextBox.Text;

            if (string.IsNullOrWhiteSpace(userName))
            {
                MessageBox.Show("Please, write existing name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var welcomeHandler = new WelcomeHandler.WelcomeHandler();
            MessageBox.Show(welcomeHandler.WelcomeWrapper(userName), "Welcome");
        }
    }
}
