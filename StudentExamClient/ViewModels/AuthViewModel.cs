using ModelsApi;
using StudentExamClient.Core;
using StudentExamClient.Views;
using StudentExamClient.Views.AddWindows;
using StudentExamClient.Views.EditWindows;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace StudentExamClient.ViewModels
{
    public class AuthViewModel : BaseViewModel
    {
        private List<Group> allGroups = new List<Group>();
        private List<Student> allStudents = new List<Student>();
        private Group selectedGroup;
        private string adminPassword;

        public List<Group> Groups { get => allGroups; set { allGroups = value; OnPropertyChanged(); } }
        public Group SelectedGroup
        {
            get => selectedGroup;
            set
            {
                selectedGroup = value;
                SelectedStudent = null;
                OnPropertyChanged(nameof(Students));
            }
        }
        public List<Student> Students { get => allStudents.FindAll(s => s.GroupId == SelectedGroup.Id && s.StatusId == 1); }
        public Student SelectedStudent { get; set; }

        public bool IsConnected { get; set; }
        public bool IsSettingsEnabled { get; set; }

        public string NewConnectionString { get; set; }

        public RelayCommand OpenSettingsWindow { get; set; }
        public RelayCommand SetNewConnectionString { get; set; }

        public AuthViewModel()
        {
            InitializeAsync();

            OpenSettingsWindow = new RelayCommand(
                () =>
                {
                    NewConnectionString = AppSettings.GetConfig().ApiConnectionString;
                    new ConnectionSettingsWindow(this).Show();
                },
                () => IsSettingsEnabled);

            SetNewConnectionString = new RelayCommand(
                () =>
                {
                    AppSettings.SetApiConnectionString(NewConnectionString);
                    InitializeAsync();
                },
                () => !string.IsNullOrWhiteSpace(NewConnectionString));
        }

        private async void InitializeAsync()
        {
            try
            {
                IsConnected = false;
                IsSettingsEnabled = false;

                allGroups = (await Api.GetListAsync<List<Group>>("Group")).FindAll(g => g.StatusId == 1);
                allStudents = Task.Run(() => Api.GetListAsync<List<Student>>("Student")).Result.FindAll(s => s.StatusId == 1);

                var admin = allStudents.Find(s => s.Id == 1);
                adminPassword = admin.Password;
                SelectedStudent = admin;
                allStudents.Remove(admin);

                Groups = new List<Group>(allGroups);

                IsConnected = true;
                OnPropertyChanged(nameof(IsConnected));
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось подключиться к серверу.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                IsSettingsEnabled = true;
                OnPropertyChanged(nameof(IsSettingsEnabled));
            }
        }

        public void LogInExplicit(int userId)
        {
            AppSettings.CurrentUserId = userId;
            new MainWindow().Show();
            LoggedIn?.Invoke();
        }

        public void LogInUser(string password)
        {
            if (string.IsNullOrEmpty(SelectedStudent.Password))
            {
                new NewPasswordWindow(this).Show();
                return;
            }

            if (password != SelectedStudent.Password)
            {
                MessageBox.Show("Неверный пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.None);
                return;
            }

            LogInExplicit(SelectedStudent.Id);
        }

        public void LogInAdmin(string password)
        {
            if (adminPassword != password)
            {
                MessageBox.Show("Неверный пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.None);
                return;
            }

            LogInExplicit(1);
        }

        public async void SetNewPassword(string password)
        {
            SelectedStudent.Password = password;
            await Api.PutAsync(SelectedStudent, "Student");
            LogInExplicit(SelectedStudent.Id);
        }

        public void SetLater() => LogInExplicit(SelectedStudent.Id);

        public event AnyHandler LoggedIn;
    }
}
