using StudentExamClient.ViewModels;
using System.Windows;

namespace StudentExamClient.Views.AddWindows
{
    public partial class AddGroupWindow : Window
    {
        public AddGroupWindow(EditorViewModel editorVM)
        {
            InitializeComponent();
            DataContext = editorVM;
        }
    }
}
