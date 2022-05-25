using StudentExamClient.ViewModels;
using System.Windows;

namespace StudentExamClient.Views.EditWindows
{
    public partial class EditStudentWindow : Window
    {
        public EditStudentWindow(EditorViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
