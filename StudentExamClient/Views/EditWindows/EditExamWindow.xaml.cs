using ModelsApi;
using StudentExamClient.ViewModels;
using System.Windows;

namespace StudentExamClient.Views.EditWindows
{
    public partial class EditExamWindow : Window
    {
        public EditExamWindow(Exam exam)
        {
            InitializeComponent();
            DataContext = new EditExamViewModel(exam);
        }
    }
}
