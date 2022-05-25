using StudentExamClient.ViewModels;
using System.Windows;

namespace StudentExamClient.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var vm = new MainViewModel();
            vm.LoggedOut += Close;
            DataContext = vm;
            InitializeComponent();
        }
    }
}
