using StudentExamClient.ViewModels;
using System.Windows;

namespace StudentExamClient.Views.EditWindows
{
    public partial class EditPasswordWindow : Window
    {
        public EditPasswordWindow(SettingsViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;

            setPassword.Click += SetPassword;
        }

        private void SetPassword(object sender, RoutedEventArgs e)
        {
            if (password1.Password != password2.Password)
            {
                MessageBox.Show("Пароли не совпадают!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            MessageBox.Show("Пароль изменен", "Успешно", MessageBoxButton.OK);
            (DataContext as SettingsViewModel).SetNewPassword(password1.Password);
            Close();
        }
    }
}
