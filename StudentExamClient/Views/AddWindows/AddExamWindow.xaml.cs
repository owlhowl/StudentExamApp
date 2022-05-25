using StudentExamClient.ViewModels;
using System.Windows;

namespace StudentExamClient.Views.AddWindows
{
    public partial class AddExamWindow : Window
    {
        public AddExamWindow(EditorViewModel editorVM)
        {
            InitializeComponent();
            DataContext = editorVM;
            addExam.Click += (o, e) => Close();
        }
    }
}
