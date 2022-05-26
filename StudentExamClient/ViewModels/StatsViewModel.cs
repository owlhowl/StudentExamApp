using ModelsApi;
using StudentExamClient.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ClosedXML.Excel;
using System.Windows;

namespace StudentExamClient.ViewModels
{
    public class StatsViewModel : BaseViewModel
    {
        private List<StudentExam> studentExams = new List<StudentExam>();
        private StudentExam selectedMyExam;

        private List<Group> allGroups;
        private List<Student> allStudents;
        private List<Subject> allSubjects;
        private List<Exam> allExams;

        private double averageScore;
        private double averageScoreMonth;
        private double averageScorePeriod;

        private Group selectedGroupFilter;
        private Student selectedStudentFilter;
        private Subject selectedSubjectFilter;
        private Exam selectedExamFilter;

        private List<Group> groupsFilter;
        private List<Student> studentsFilter;
        private List<Subject> subjectsFilter;
        private List<Exam> examsFilter;

        public double AverageScore { get => averageScore; set { averageScore = value; OnPropertyChanged(); } }
        public double AverageScoreMonth { get => averageScoreMonth; set { averageScoreMonth = value; OnPropertyChanged(); } }
        public double AverageScorePeriod { get => averageScorePeriod; set { averageScorePeriod = value; OnPropertyChanged(); } }

        public ObservableCollection<StudentExam> FilteredExams { get; set; } = new ObservableCollection<StudentExam>();
        public ObservableCollection<StudentExam> FilteredMyExams { get; set; } = new ObservableCollection<StudentExam>();
        public StudentExam SelectedMyExam { get => selectedMyExam; set { selectedMyExam = value; OnPropertyChanged(); } }

        public List<Group> GroupsFilter
        {
            get => groupsFilter;
            set { groupsFilter = value; OnPropertyChanged(); }
        }
        public Group SelectedGroupFilter
        {
            get => selectedGroupFilter;
            set
            {
                selectedGroupFilter = value;
                StudentsFilter = allStudents.FindAll(s => s.GroupId == selectedGroupFilter.Id || s.Id == 0);
                SelectedStudentFilter = StudentsFilter[0];
                OnPropertyChanged();
            }
        }

        public List<Student> StudentsFilter
        {
            get => studentsFilter;
            set { studentsFilter = value; OnPropertyChanged(); }
        }
        public Student SelectedStudentFilter
        {
            get => selectedStudentFilter;
            set { selectedStudentFilter = value; OnPropertyChanged(); }
        }

        public List<Subject> SubjectsFilter
        {
            get => subjectsFilter;
            set { subjectsFilter = value; OnPropertyChanged(); }
        }
        public Subject SelectedSubjectFilter
        {
            get => selectedSubjectFilter;
            set
            {
                selectedSubjectFilter = value;
                ExamsFilter = allExams.FindAll(e => e.SubjectId == selectedSubjectFilter.Id || e.Id == 0);
                SelectedExamFilter = ExamsFilter[0];
                OnPropertyChanged();
            }
        }

        public List<Exam> ExamsFilter
        {
            get => examsFilter;
            set { examsFilter = value; OnPropertyChanged(); }
        }
        public Exam SelectedExamFilter
        {
            get => selectedExamFilter;
            set { selectedExamFilter = value; OnPropertyChanged(); }
        }

        public DateTime DateStart { get; set; } = DateTime.Today.AddMonths(-1);
        public DateTime DateEnd { get; set; } = DateTime.Today;

        public RelayCommand Filter { get; set; }
        public RelayCommand ClearFilter { get; set; }
        public RelayCommand ExportToExcel { get; set; }

        public StatsViewModel(MainViewModel mainVM)
        {
            InitializeAsync();

            Filter = new RelayCommand(
                () =>
                {
                    FilteredExams.Clear();

                    List<StudentExam> exams = new List<StudentExam>();
                    foreach (var exam in studentExams)
                    {
                        if (allStudents.Find(s => s.Id == exam.StudentId).GroupId == SelectedGroupFilter.Id || SelectedGroupFilter.Id == 0)
                            if (exam.StudentId == SelectedStudentFilter.Id || SelectedStudentFilter.Id == 0)
                                if (allExams.Find(s => s.Id == exam.ExamId).SubjectId == SelectedSubjectFilter.Id || SelectedSubjectFilter.Id == 0)
                                    if (exam.ExamId == SelectedExamFilter.Id || SelectedExamFilter.Id == 0)
                                        exams.Add(exam);
                    }
                    foreach (var exam in exams)
                    {
                        if (exam.DateTaken.Date >= DateStart && exam.DateTaken.Date <= DateEnd)
                            FilteredExams.Add(exam);
                    }
                    GetAverageScores(exams);
                },
                () => true);

            ClearFilter = new RelayCommand(
                () =>
                {
                    AverageScorePeriod = 0;
                    FilteredExams.Clear();
                    studentExams.ForEach(e => FilteredExams.Add(e));

                    SelectedGroupFilter = GroupsFilter[0];
                    SelectedStudentFilter = StudentsFilter[0];
                    SelectedSubjectFilter = SubjectsFilter[0];
                    selectedExamFilter = ExamsFilter[0];

                    GetAverageScores(studentExams);
                },
                () => true);

            ExportToExcel = new RelayCommand(
                () =>
                {
                    var workbook = new XLWorkbook();
                    var ws = workbook.AddWorksheet("Отчет");
                    ws.ColumnWidth = 20;
                    ws.Column(1).Width = 30;

                    int row = 1;
                    ws.Cell("A" + row.ToString()).Value = "Фильтры:";
                    ws.Cell("A" + row.ToString()).Style.Font.SetBold();
                    row++;
                    ws.Cell("A" + row.ToString()).Value = "Группа";
                    ws.Cell("A" + row.ToString()).Style.Font.SetBold();
                    ws.Cell("B" + row.ToString()).Value = "Студент";
                    ws.Cell("B" + row.ToString()).Style.Font.SetBold();
                    ws.Cell("C" + row.ToString()).Value = "Предмет";
                    ws.Cell("C" + row.ToString()).Style.Font.SetBold();
                    ws.Cell("D" + row.ToString()).Value = "Тест";
                    ws.Cell("D" + row.ToString()).Style.Font.SetBold();
                    row++;
                    ws.Cell("A" + row.ToString()).Value = SelectedGroupFilter.Name;
                    ws.Cell("B" + row.ToString()).Value = SelectedStudentFilter.Name;
                    ws.Cell("C" + row.ToString()).Value = SelectedSubjectFilter.Name;
                    ws.Cell("D" + row.ToString()).Value = SelectedExamFilter.Title;
                    row++;
                    row++;
                    ws.Cell("A" + row.ToString()).Value = "Студент";
                    ws.Cell("A" + row.ToString()).Style.Font.SetBold();
                    ws.Cell("B" + row.ToString()).Value = "Дата и время";
                    ws.Cell("B" + row.ToString()).Style.Font.SetBold();
                    ws.Cell("C" + row.ToString()).Value = "Тест";
                    ws.Cell("C" + row.ToString()).Style.Font.SetBold();
                    ws.Cell("D" + row.ToString()).Value = "Кол-во баллов";
                    ws.Cell("D" + row.ToString()).Style.Font.SetBold();
                    foreach (var exam in FilteredExams)
                    {
                        row++;
                        ws.Cell("A" + row.ToString()).Value = allStudents.Find(s => s.Id == exam.StudentId).Name;
                        ws.Cell("B" + row.ToString()).Value = exam.DateTaken;
                        ws.Cell("C" + row.ToString()).Value = allExams.Find(e => e.Id == exam.ExamId).Title;
                        ws.Cell("D" + row.ToString()).Value = exam.Score;
                    }

                    ws.Cells("A1:D3").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Cells("A5:D" + row.ToString()).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
                    sfd.DefaultExt = ".xlsx";
                    sfd.Filter = "Microsoft Excel (*.xlsx)|*.xlsx";
                    var result = sfd.ShowDialog();
                    if (result == false) 
                        return;
                    try
                    {
                        workbook.SaveAs(sfd.FileName);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Не удалось сохранить файл", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                },
                () => FilteredExams.Any());
            
        }

        private void GetAverageScores(List<StudentExam> exams)
        {
            AverageScore = exams.Any() ? Math.Round(exams.Average(e => e.Score), 1) : 0;
            AverageScoreMonth = exams.Any() ? Math.Round((double)exams.Average(e => { return e.DateTaken.Month == DateTime.Today.Month ? e.Score : null; }), 1) : 0;
            AverageScorePeriod = FilteredExams.Any() ? Math.Round(FilteredExams.Average(e => e.Score), 1) : 0;
        }

        private async void InitializeAsync()
        {
            studentExams = (await Api.GetListAsync<List<StudentExam>>("StudentExam")).FindAll(e => e.StudentId != 1);
            studentExams.Sort((a, b) => b.DateTaken.CompareTo(a.DateTaken));

            studentExams.ForEach(e => FilteredExams.Add(e));
            studentExams.ForEach(e => { if (e.StudentId == AppSettings.CurrentUserId) FilteredMyExams.Add(e); });

            GetAverageScores(studentExams);

            allGroups = (await Api.GetListAsync<List<Group>>("Group")).FindAll(i => i.StatusId == 1);
            allStudents = (await Api.GetListAsync<List<Student>>("Student")).FindAll(i => i.StatusId == 1 && i.Id != 1);
            allSubjects = (await Api.GetListAsync<List<Subject>>("Subject")).FindAll(i => i.StatusId == 1);
            allExams = (await Api.GetListAsync<List<Exam>>("Exam")).FindAll(i => i.StatusId == 1);

            allGroups.Insert(0, new Group() { Id = 0, Name = "Все группы" });
            allStudents.Insert(0, new Student() { Id = 0, Name = "Все студенты" });
            allSubjects.Insert(0, new Subject() { Id = 0, Name = "Все предметы" });
            allExams.Insert(0, new Exam() { Id = 0, Title = "Все тесты" });

            GroupsFilter = new List<Group>(allGroups);
            SubjectsFilter = new List<Subject>(allSubjects);

            SelectedGroupFilter = allGroups[0];
            SelectedSubjectFilter = allSubjects[0];
        }
    }
}
