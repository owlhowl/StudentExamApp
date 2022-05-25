using ModelsApi;
using StudentExamClient.Core;
using StudentExamClient.Views;
using System;
using System.Windows;
using System.Windows.Controls;

namespace StudentExamClient.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private Page currentView;
        private Exam currentExam;
        private Visibility userVisibility;
        private Visibility adminVisibility;

        public Page CurrentView
        {
            get => currentView;
            set { currentView = value; OnPropertyChanged(); }
        }

        public Exam CurrentExam { 
            get => currentExam; 
            set { 
                currentExam = value;
                if (value is not null)
                {
                    AdminVisibility = Visibility.Collapsed;
                    UserVisibility = Visibility.Collapsed;
                }
                OnPropertyChanged(); 
            } 
        }

        public Visibility UserVisibility { get => userVisibility; set { userVisibility = value; OnPropertyChanged(); } }
        public Visibility AdminVisibility { get => adminVisibility; set { adminVisibility = value; OnPropertyChanged(); } }

        public RelayCommand OpenExamListPage { get; set; }
        public RelayCommand OpenMyStatsPage { get; set; }
        public RelayCommand OpenAllStatsPage { get; set; }
        public RelayCommand OpenEditorPage { get; set; }
        public RelayCommand OpenSettingsPage { get; set; }
        public RelayCommand LogOut { get; set; }
        public RelayCommand QuitExam { get; set; }

        public MainViewModel()
        {
            CurrentView = new ExamListPage(this);

            OpenExamListPage = new RelayCommand(() => CurrentView = new ExamListPage(this), () => true);
            OpenAllStatsPage = new RelayCommand(() => CurrentView = new AllStatsPage(this), () => true);
            OpenMyStatsPage = new RelayCommand(() => CurrentView = new MyStatsPage(this), () => true);
            OpenEditorPage = new RelayCommand(() => CurrentView = new EditorPage(this), () => true);
            OpenSettingsPage = new RelayCommand(() => CurrentView = new SettingsPage(this), () => true);

            LogOut = new RelayCommand(
                () => 
                {
                    new AuthWindow().Show();
                    LoggedOut?.Invoke();
                    AppSettings.CurrentUserId = 0;
                }, 
                () => true);

            QuitExam = new RelayCommand(
                () => 
                {
                    CurrentView = new ExamListPage(this);
                    CurrentExam = null; 
                }, 
                () => true);
        }

        public event AnyHandler LoggedOut;
    }
}
