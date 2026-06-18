using SM_TPS_.Commands;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SM_TPS_.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; set; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(ExecuteLogin);
        }

        private void ExecuteLogin(object obj)
        {
            // الـ obj هنا هو الـ PasswordBox اللي جاي من الـ View
            var passwordBox = obj as PasswordBox;
            if (passwordBox == null) return;

            string password = passwordBox.Password;
            string trimmedUser = Username?.Trim();

            // تحقق الأمان (عدله حسب بياناتك)
            if (trimmedUser == "admin" && password == "admin")
            {
                // الحصول على نافذة اللوجين الحالية لإغلاقها بأمان
                Window currentWindow = Window.GetWindow(passwordBox);

                // فتح الشاشة الرئيسية للمبيعات
                Views.MainWindow salesWindow = new Views.MainWindow();
                Application.Current.MainWindow = salesWindow;
                salesWindow.Show();

                // إغلاق شاشة اللوجين
                currentWindow?.Close();
            }
            else
            {
                MessageBox.Show("اسم المستخدم أو كلمة المرور غير صحيحة!",
                                "فشل تسجيل الدخول",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }
    }
}
