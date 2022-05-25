using StudentExamClient.ViewModels;
using System.Windows;

namespace StudentExamClient.Views
{
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();
            var vm = new AuthViewModel();
            vm.LoggedIn += Close;
            DataContext = vm;

            logInUser.Click += (o, e) => vm.LogInUser(userPassword.Password);
            logInAdmin.Click += (o, e) => vm.LogInAdmin(adminPassword.Password);
        }
    }
}
