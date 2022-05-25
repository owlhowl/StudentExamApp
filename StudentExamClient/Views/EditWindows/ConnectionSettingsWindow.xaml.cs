using StudentExamClient.ViewModels;
using System.Windows;

namespace StudentExamClient.Views.EditWindows
{
    public partial class ConnectionSettingsWindow : Window
    {
        public ConnectionSettingsWindow(AuthViewModel authVM)
        {
            InitializeComponent();
            DataContext = authVM;
            connect.Click += (o, e) => Close();
        }
    }
}
