using StudentExamClient.ViewModels;
using System.Windows;

namespace StudentExamClient.Views.EditWindows
{
    public partial class EditSubjectWindow : Window
    {
        public EditSubjectWindow(EditorViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
