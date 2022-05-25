using ModelsApi;
using StudentExamClient.Core;
using StudentExamClient.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace StudentExamClient.ViewModels
{
    public class ExamListViewModel : BaseViewModel
    {
        private Subject selectedSubject;
        private Exam selectedExam;
        private List<Exam> allExams;
        private List<Question> allQuestions;
        private List<Subject> allSubjects;

        public ObservableCollection<Subject> Subjects { get; set; } = new ObservableCollection<Subject>();
        public Subject SelectedSubject
        {
            get => selectedSubject;
            set 
            { 
                selectedSubject = value; 
                selectedExam = null;
                Exams.Clear();
                Questions.Clear();
                allExams.ForEach(e => { if (e.StatusId == 1 && e.SubjectId == selectedSubject.Id) Exams.Add(e); });
            }
        }
        public ObservableCollection<Exam> Exams { get; set; } = new ObservableCollection<Exam>();
        public Exam SelectedExam
        {
            get => selectedExam;
            set { 
                selectedExam = value;
                Questions.Clear();
                OnPropertyChanged();
                if (selectedExam is null)
                    return;
                allQuestions.ForEach(q => { if (q.StatusId == 1 && q.ExamId == selectedExam.Id) Questions.Add(q); });
            }
        }
        public ObservableCollection<Question> Questions { get; set; } = new ObservableCollection<Question>();

        public RelayCommand BeginExam { get; set; }

        public ExamListViewModel(MainViewModel mainVM)
        {
            InitializeAsync();

            if (AppSettings.CurrentUserId == 1)
            {
                mainVM.UserVisibility = Visibility.Collapsed;
                mainVM.AdminVisibility = Visibility.Visible;
            }
            if (AppSettings.CurrentUserId > 1)
            {
                mainVM.UserVisibility = Visibility.Visible;
                mainVM.AdminVisibility = Visibility.Collapsed;
            }

            BeginExam = new RelayCommand(
                () => mainVM.CurrentView = new ExamTakingPage(mainVM, SelectedExam), 
                () => SelectedExam is not null
            );
        }

        private async void InitializeAsync()
        {
            allSubjects = await Api.GetListAsync<List<Subject>>("Subject");
            allExams = await Api.GetListAsync<List<Exam>>("Exam");
            allQuestions = await Api.GetListAsync<List<Question>>("Question");

            allSubjects.ForEach(s => Subjects.Add(s));
        }
    }
}
