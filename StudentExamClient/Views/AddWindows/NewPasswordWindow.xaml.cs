using StudentExamClient.ViewModels;
using System.Windows;

namespace StudentExamClient.Views.AddWindows
{
    public partial class NewPasswordWindow : Window
    {
        public NewPasswordWindow(AuthViewModel authVM)
        {
            InitializeComponent();
            DataContext = authVM;

            setPassword.Click += SetPassword;
            setLater.Click += (o, e) => { authVM.SetLater(); Close(); };
        }

        private void SetPassword(object sender, RoutedEventArgs e)
        {
            if (password1.Password != password2.Password)
            {
                MessageBox.Show("Пароли не совпадают!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            MessageBox.Show("Пароль изменен", "Успешно", MessageBoxButton.OK);
            (DataContext as AuthViewModel).SetNewPassword(password1.Password);
            Close();
        }
    }
}
