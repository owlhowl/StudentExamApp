using ModelsApi;
using StudentExamClient.Core;
using StudentExamClient.Views.EditWindows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentExamClient.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private Student currentStudent;

        public Student CurrentStudent { get => currentStudent; set { currentStudent = value; OnPropertyChanged(); } }
        public string NewPassword1 { get; set; }
        public string NewPassword2 { get; set; }

        public RelayCommand OpenEditPasswordWindow { get; set; }

        public SettingsViewModel(MainViewModel mainVM)
        {
            InitializeAsync();

            OpenEditPasswordWindow = new RelayCommand(
                () => new EditPasswordWindow(this).Show(),
                () => true);
        }

        private async void InitializeAsync()
        {
            CurrentStudent = await Api.GetAsync<Student>(AppSettings.CurrentUserId, "Student");
        }

        public async void SetNewPassword(string password)
        {
            CurrentStudent.Password = password;
            await Api.PutAsync(CurrentStudent, "Student");
        }
    }
}

