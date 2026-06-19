using System.Windows;
using SM_TPS_.Data;

namespace SM_TPS_.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            UserRepository repository =
                new UserRepository();

            bool isValid =
                repository.Login(
                    txtUsername.Text,
                    TxtPassword.Password);

            if (isValid)
            {
                MainWindow main =
                    new MainWindow();

                main.Show();

                this.Close();
            }
            else
            {
                MessageBox.Show(
                    "Invalid Username or Password");
            }
        }
    }
}