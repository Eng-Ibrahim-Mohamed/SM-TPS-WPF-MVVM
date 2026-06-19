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
using System.Windows.Shapes;


namespace SM_TPS_.Views
{
    public partial class ChangePasswordWindow : Window
    {
        public string NewPassword { get; private set; }

        public ChangePasswordWindow()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (txtNewPassword.Password !=
                txtConfirmPassword.Password)
            {
                MessageBox.Show(
                    "Passwords do not match!");

                return;
            }

            if (string.IsNullOrWhiteSpace(
                txtNewPassword.Password))
            {
                MessageBox.Show(
                    "Password cannot be empty!");

                return;
            }

            NewPassword =
                txtNewPassword.Password;

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(
            object sender,
            RoutedEventArgs e)
        {
            Close();
        }
    }
}
