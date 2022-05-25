using ModelsApi;
using StudentExamClient.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using StudentExamClient.Views.AddWindows;
using System.Windows;
using StudentExamClient.Views.EditWindows;
using System.Linq;

namespace StudentExamClient.ViewModels
{
    public class EditorViewModel : BaseViewModel
    {
        private List<Group> allGroups;
        private List<Student> allStudents;
        private List<Subject> allSubjects;
        private List<Exam> allExams;
        private Group selectedGroup;
        private Subject selectedSubject;
        private ObservableCollection<Group> groups = new ObservableCollection<Group>();

        public ObservableCollection<Group> Groups { get => groups; set { value.OrderBy(g => g.Name); groups = value; } }
        public Group SelectedGroup
        {
            get => selectedGroup;
            set
            {
                selectedGroup = value;
                SelectedStudent = null;
                Students.Clear();
                allStudents.ForEach(s => { if (selectedGroup is not null && s.GroupId == selectedGroup.Id && s.StatusId == 1) Students.Add(s); });
            }
        }

        public ObservableCollection<Student> Students { get; set; } = new ObservableCollection<Student>();
        public Student SelectedStudent { get; set; }
        public ObservableCollection<Subject> Subjects { get; set; } = new ObservableCollection<Subject>();
        public Subject SelectedSubject
        {
            get => selectedSubject;
            set
            {
                selectedSubject = value;
                SelectedExam = null;
                Exams.Clear();
                allExams.ForEach(e => { if (selectedSubject is not null && e.SubjectId == SelectedSubject.Id && e.StatusId == 1) Exams.Add(e); });
            }
        }
        public ObservableCollection<Exam> Exams { get; set; } = new ObservableCollection<Exam>();
        public Exam SelectedExam { get; set; }

        public Group NewGroup { get; set; } = new Group();
        public Student NewStudent { get; set; } = new Student();
        public Subject NewSubject { get; set; } = new Subject();
        public Exam NewExam { get; set; } = new Exam();

        public RelayCommand OpenAddGroupWindow { get; set; }
        public RelayCommand OpenAddStudentWindow { get; set; }
        public RelayCommand OpenAddSubjectWindow { get; set; }
        public RelayCommand OpenAddExamWindow { get; set; }

        public RelayCommand AddGroup { get; set; }
        public RelayCommand AddStudent { get; set; }
        public RelayCommand AddSubject { get; set; }
        public RelayCommand AddExam { get; set; }

        public RelayCommand EditGroup { get; set; }
        public RelayCommand EditStudent { get; set; }
        public RelayCommand EditSubject { get; set; }
        public RelayCommand EditExam { get; set; }

        public RelayCommand RemoveGroup { get; set; }
        public RelayCommand RemoveStudent { get; set; }
        public RelayCommand RemoveSubject { get; set; }
        public RelayCommand RemoveExam { get; set; }

        public RelayCommand OpenEditGroupWindow { get; set; }
        public RelayCommand OpenEditStudentWindow { get; set; }
        public RelayCommand OpenEditSubjectWindow { get; set; }
        public RelayCommand OpenEditExamWindow { get; set; }

        public EditorViewModel(MainViewModel mainVM)
        {
            InitializeAsync();

            #region Команды удаления

            RemoveGroup = new RelayCommand(
                async () =>
                {
                    if (MessageBox.Show("Удалить выбранную группу?", "Группы", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                        return;
                    SelectedGroup.StatusId = 2;
                    await Api.PutAsync(SelectedGroup, "Group");
                    Groups.Remove(SelectedGroup);
                },
                () => SelectedGroup is not null);

            RemoveStudent = new RelayCommand(
                async () =>
                {
                    if (MessageBox.Show("Удалить выбранного студента?", "Студенты", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                        return;
                    SelectedStudent.StatusId = 2;
                    await Api.PutAsync(SelectedStudent, "Student");
                    Students.Remove(SelectedStudent);
                },
                () => SelectedStudent is not null);

            RemoveSubject = new RelayCommand(
                async () =>
                {
                    if (MessageBox.Show("Удалить выбранный предмет?", "Предметы", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                        return;
                    SelectedSubject.StatusId = 2;
                    await Api.PutAsync(SelectedSubject, "Subject");
                    Subjects.Remove(SelectedSubject);
                },
                () => SelectedSubject is not null);

            RemoveExam = new RelayCommand(
                async () =>
                {
                    if (MessageBox.Show("Удалить выбранный тест?", "Тесты", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                        return;
                    SelectedExam.StatusId = 2;
                    await Api.PutAsync(SelectedExam, "Exam");
                    Exams.Remove(SelectedExam);
                },
                () => SelectedExam is not null);

            #endregion

            #region Команды открытия окон добавления

            OpenAddGroupWindow = new RelayCommand(
                () =>
                {
                    NewGroup = new Group();
                    new AddGroupWindow(this).Show();
                },
                () => true);

            OpenAddStudentWindow = new RelayCommand(
                () =>
                {
                    NewStudent = new Student() { GroupId = SelectedGroup.Id };
                    new AddStudentWindow(this).Show();
                },
                () => SelectedGroup is not null);

            OpenAddSubjectWindow = new RelayCommand(
                () =>
                {
                    NewSubject = new Subject();
                    new AddSubjectWindow(this).Show();
                },
                () => true);

            OpenAddExamWindow = new RelayCommand(
                () =>
                {
                    NewExam = new Exam() { SubjectId = SelectedSubject.Id };
                    new AddExamWindow(this).Show();
                },
                () => SelectedSubject is not null);

            #endregion

            #region Команды добавления

            AddGroup = new RelayCommand(
                async () =>
                {
                    NewGroup.Name = NewGroup.Name.Trim();

                    if (allGroups.Exists(g => g.Name == NewGroup.Name && g.StatusId == 2))
                    {
                        await Api.PutAsync(NewGroup, "Group");
                        Groups.Add(NewGroup);
                        allGroups.Add(NewGroup);
                        MessageBox.Show($"Группа {NewGroup.Name} восстановлена", "Успешно", MessageBoxButton.OK);
                    }
                    else if (allGroups.Exists(g => g.Name == NewGroup.Name))
                    {
                        MessageBox.Show($"Группа {NewGroup.Name} уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        await Api.PostAsync(NewGroup, "Group");
                        Groups.Add(NewGroup);
                        allGroups.Add(NewGroup);
                        MessageBox.Show($"Группа {NewGroup.Name} добавлена", "Успешно", MessageBoxButton.OK);
                    }
                },
                () => !string.IsNullOrWhiteSpace(NewGroup.Name));

            AddStudent = new RelayCommand(
                async () =>
                {
                    NewStudent.Name = NewStudent.Name.Trim();

                    if (allStudents.Exists(s => s.Name == NewStudent.Name && s.StatusId == 2))
                    {
                        await Api.PutAsync(NewStudent, "Student");
                        Students.Add(NewStudent);
                        allStudents.Add(NewStudent);
                        MessageBox.Show($"Студент {NewStudent.Name} восстановлен", "Успешно", MessageBoxButton.OK);
                    }
                    else if (allStudents.Exists(s => s.Name == NewStudent.Name))
                    {
                        MessageBox.Show($"Студент {NewStudent.Name} уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        await Api.PostAsync(NewStudent, "Student");
                        Students.Add(NewStudent);
                        allStudents.Add(NewStudent);
                        MessageBox.Show($"Студент {NewStudent.Name} добавлен", "Успешно", MessageBoxButton.OK);
                    }
                },
                () => !string.IsNullOrWhiteSpace(NewStudent.Name));

            AddSubject = new RelayCommand(
                async () =>
                {
                    NewSubject.Name = NewSubject.Name.Trim();

                    if (allSubjects.Exists(s => s.Name == NewSubject.Name && s.StatusId == 2))
                    {
                        await Api.PutAsync(NewSubject, "Subject");
                        Subjects.Add(NewSubject);
                        allSubjects.Add(NewSubject);
                        MessageBox.Show($"Предмет {NewSubject.Name} восстановлен", "Успешно", MessageBoxButton.OK);
                    }
                    else if (allSubjects.Exists(s => s.Name == NewSubject.Name))
                    {
                        MessageBox.Show($"Предмет {NewSubject.Name} уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        await Api.PostAsync(NewSubject, "Subject");
                        Subjects.Add(NewSubject);
                        allSubjects.Add(NewSubject);
                        MessageBox.Show($"Предмет {NewSubject.Name} добавлен", "Успешно", MessageBoxButton.OK);
                    }
                },
                () => !string.IsNullOrWhiteSpace(NewSubject.Name));

            AddExam = new RelayCommand(
                async () =>
                {
                    NewExam.Title = NewExam.Title.Trim();

                    if (allExams.Exists(e => e.Title == NewExam.Title && e.StatusId == 2))
                    {
                        await Api.PutAsync(NewExam, "Exam");
                        Exams.Add(NewExam);
                        allExams.Add(NewExam);
                        MessageBox.Show($"Тест {NewExam.Title} восстановлен", "Успешно", MessageBoxButton.OK);
                    }
                    else if (allSubjects.Exists(s => s.Name == NewExam.Title))
                    {
                        MessageBox.Show($"Тест {NewExam.Title} уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        await Api.PostAsync(NewExam, "Exam");
                        Exams.Add(NewExam);
                        allExams.Add(NewExam);
                        //MessageBox.Show($"Тест {NewExam.Title} добавлен", "Успешно", MessageBoxButton.OK);
                        new EditExamWindow(NewExam).Show();
                    }
                },
                () => !string.IsNullOrWhiteSpace(NewExam.Title));

            #endregion

            #region Команды открытия окон редактирования

            OpenEditGroupWindow = new RelayCommand(
                () =>
                {
                    NewGroup = new Group() { Id = SelectedGroup.Id, Name = SelectedGroup.Name };
                    new EditGroupWindow(this).Show();
                },
                () => SelectedGroup is not null);

            OpenEditStudentWindow = new RelayCommand(
                () =>
                {
                    NewStudent = new Student() { Id = SelectedStudent.Id, Name = SelectedStudent.Name, Password = SelectedStudent.Password };
                    new EditStudentWindow(this).Show();
                },
                () => SelectedStudent is not null);

            OpenEditSubjectWindow = new RelayCommand(
                () =>
                {
                    NewSubject = new Subject() { Id = SelectedSubject.Id, Name = SelectedSubject.Name };
                    new EditSubjectWindow(this).Show();
                },
                () => SelectedSubject is not null);

            OpenEditExamWindow = new RelayCommand(
                () => new EditExamWindow(SelectedExam).Show(),
                () => SelectedExam is not null);

            #endregion

            #region Команды редактирования

            EditGroup = new RelayCommand(
                async () =>
                {
                    var group = allGroups.Find(g => g.Id == NewGroup.Id);
                    group.Name = NewGroup.Name;
                    await Api.PutAsync(group, "Group");
                    MessageBox.Show("Изменения применены", "Успешно");
                },
                () => !string.IsNullOrWhiteSpace(NewGroup.Name));

            EditStudent = new RelayCommand(
                async () =>
                {
                    var student = allStudents.Find(s => s.Id == NewStudent.Id);
                    student.Name = NewStudent.Name;
                    student.Password = NewStudent.Password;
                    await Api.PutAsync(student, "Student");
                    MessageBox.Show("Изменения применены", "Успешно");
                },
                () => !string.IsNullOrWhiteSpace(NewStudent.Name) && !string.IsNullOrWhiteSpace(NewStudent.Password));

            EditSubject = new RelayCommand(
                async () =>
                {
                    var subject = allGroups.Find(s => s.Id == NewSubject.Id);
                    subject.Name = NewSubject.Name;
                    await Api.PutAsync(subject, "Subject");
                    MessageBox.Show("Изменения применены", "Успешно");
                },
                () => !string.IsNullOrWhiteSpace(NewSubject.Name));
            #endregion
        }


        private async void InitializeAsync()
        {
            allGroups = await Api.GetListAsync<List<Group>>("Group");
            allStudents = await Api.GetListAsync<List<Student>>("Student");
            allSubjects = await Api.GetListAsync<List<Subject>>("Subject");
            allExams = await Api.GetListAsync<List<Exam>>("Exam");

            allGroups.ForEach(g => { if (g.StatusId == 1) Groups.Add(g); });

            allSubjects.ForEach(s => { if (s.StatusId == 1) Subjects.Add(s); });

            var admin = allStudents.Find(s => s.Id == 1);
            allStudents.Remove(admin);
        }
    }
}
