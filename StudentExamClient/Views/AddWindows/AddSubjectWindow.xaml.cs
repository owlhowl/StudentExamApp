using StudentExamClient.ViewModels;
using System.Windows;

namespace StudentExamClient.Views.AddWindows
{
    public partial class AddSubjectWindow : Window
    {
        public AddSubjectWindow(EditorViewModel editorVM)
        {
            InitializeComponent();
            DataContext = editorVM;
        }
    }
}
