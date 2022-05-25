using StudentExamClient.ViewModels;
using System.Windows.Controls;

namespace StudentExamClient.Views
{
    public partial class SettingsPage : Page
    {
        public SettingsPage(MainViewModel vm)
        {
            InitializeComponent();
            DataContext = new SettingsViewModel(vm);
        }
    }
}
