using ModelsApi;
using StudentExamClient.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace StudentExamClient.ViewModels
{
    public class QuestionEdit : Question
    {
        public int CountId { get; set; }
        public List<Answer> Answers { get; set; } = new List<Answer>();
    }

    public class ExamEditViewModel : BaseViewModel
    {
        private List<QuestionEdit> allQuestions;

        public Exam Exam { get; set; }
        public ObservableCollection<QuestionEdit> Questions { get; set; }

        public RelayCommand SaveExam { get; set; }
        public RelayCommand AddQuestion { get; set; }
        public RelayCommandParameter RemoveQuestion { get; set; }
        public RelayCommandParameter AddAnswer { get; set; }
        public RelayCommandParameter RemoveAnswer { get; set; }


        public ExamEditViewModel(Exam exam)
        {
            Exam = exam;
            Questions = new ObservableCollection<QuestionEdit>();

            AddQuestion = new RelayCommand(
                () => 
                {
                    var question = new QuestionEdit() { Answers = new List<Answer>(), ExamId = Exam.Id, CountId = Questions.Count + 1, StatusId = 1 };
                    Questions.Add(question);
                    allQuestions.Add(question);
                },
                () => true);

            RemoveQuestion = new RelayCommandParameter(
                q => 
                {
                    ((QuestionEdit)q).StatusId = 2;
                    UpdateCollection();
                },
                () => true);

            AddAnswer = new RelayCommandParameter(
                a =>
                {
                    var question = (QuestionEdit)a;
                    question.Answers.Add(new Answer() { StatusId = 1 });
                    UpdateCollection();
                },
                () => true);

            RemoveAnswer = new RelayCommandParameter(
                a =>
                {
                    ((Answer)a).StatusId = 2;
                    UpdateCollection();
                },
                () => true);

            SaveExam = new RelayCommand(
                () =>
                {
                    PostExamAsync();
                    MessageBox.Show("Изменения сохранены", "Успешно", MessageBoxButton.OK);
                },
                () => true);

            GetExamAsync();
        }

        private async void PostExamAsync()
        {
            var questionsApi = await Api.GetListAsync<List<Question>>("Question");
            int lastId = questionsApi.Last().Id;

            foreach (var question in allQuestions)
            {
                var questionApi = new Question() { Id = question.Id, ExamId = Exam.Id, QuestionText = question.QuestionText, StatusId = question.StatusId };

                if (questionApi.Id == 0) // if new
                {
                    if (questionApi.StatusId == 2 || string.IsNullOrWhiteSpace(questionApi.QuestionText)) // if new & deleted | empty
                        break;

                    await Api.PostAsync(questionApi, "Question");

                    lastId++;

                    foreach (var answer in question.Answers)
                    {
                        answer.QuestionId = lastId;
                        await Api.PostAsync(answer, "Answer");
                    }
                }
                else // if edit
                {
                    await Api.PutAsync(questionApi, "Question");

                    foreach (var answer in question.Answers)
                    {
                        answer.QuestionId = questionApi.Id;

                        if (answer.Id == 0) // if new
                        {
                            if (answer.StatusId == 2 || string.IsNullOrWhiteSpace(answer.AnswerText)) // if new & deleted | empty
                                break;
                            await Api.PostAsync(answer, "Answer");
                        }    
                        else // if edit
                            await Api.PutAsync(answer, "Answer");
                    }
                }
            }
        }

        private async void GetExamAsync()
        {
            List<Question> questions = (await Api.GetListAsync<List<Question>>("Question")).FindAll(q => q.ExamId == Exam.Id && q.StatusId == 1);

            foreach (var question in questions)
            {
                List<Answer> answers = (await Api.GetListAsync<List<Answer>>("Answer")).FindAll(a => a.QuestionId == question.Id && a.StatusId == 1);
                Questions.Add(new QuestionEdit() { Id = question.Id, CountId = Questions.Count + 1, ExamId = Exam.Id, QuestionText = question.QuestionText, Answers = answers, StatusId = question.StatusId });
            }

            allQuestions = new List<QuestionEdit>(Questions);
        }

        private void UpdateCollection()
        {
            List<QuestionEdit> questions = allQuestions.FindAll(q => q.StatusId == 1);

            foreach (var question in questions)
                question.CountId = Questions.IndexOf(question) + 1;

            Questions.Clear();
            questions.ForEach(q => Questions.Add(q));
        }
    }
}
