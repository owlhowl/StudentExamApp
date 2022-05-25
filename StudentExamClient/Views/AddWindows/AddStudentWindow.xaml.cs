using StudentExamClient.ViewModels;
using System.Windows;

namespace StudentExamClient.Views.AddWindows
{
    public partial class AddStudentWindow : Window
    {
        public AddStudentWindow(EditorViewModel editorVM)
        {
            InitializeComponent();
            DataContext = editorVM;
        }
    }
}
