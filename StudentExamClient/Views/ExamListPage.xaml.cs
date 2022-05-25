using StudentExamClient.ViewModels;
using System.Windows.Controls;

namespace StudentExamClient.Views
{
    public partial class ExamListPage : Page
    {
        public ExamListPage(MainViewModel mainVM)
        {
            InitializeComponent();
            DataContext = new ExamListViewModel(mainVM);
        }
    }
}
