using StudentExamClient.ViewModels;
using System.Windows.Controls;

namespace StudentExamClient.Views
{
    public partial class EditorPage : Page
    {
        public EditorPage(MainViewModel mainVM)
        {
            InitializeComponent();
            DataContext = new EditorViewModel(mainVM);
        }
    }
}
