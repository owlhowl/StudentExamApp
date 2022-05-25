using StudentExamClient.ViewModels;
using System.Windows;

namespace StudentExamClient.Views.EditWindows
{
    public partial class EditGroupWindow : Window
    {
        public EditGroupWindow(EditorViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
