using ModelsApi;
using StudentExamClient.ViewModels;
using System.Windows.Controls;

namespace StudentExamClient.Views
{
    public partial class ExamTakingPage : Page
    {
        public ExamTakingPage(MainViewModel mainVM, Exam exam)
        {
            DataContext = new ExamTakingViewModel(mainVM, exam);
            InitializeComponent();
        }
    }
}
