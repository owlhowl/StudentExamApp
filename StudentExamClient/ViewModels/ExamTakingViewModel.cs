using ModelsApi;
using StudentExamClient.Core;
using StudentExamClient.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Windows;

namespace StudentExamClient.ViewModels
{
    public class ExamTakingViewModel : BaseViewModel
    {
        private List<Question> allQuestions;
        private List<Answer> allAnswers;

        private List<Answer> submittedAnswers;

        public Exam CurrentExam { get; }
        public Question CurrentQuestion { get; set; }
        public List<Answer> CurrentQuestionAnswers { get => submittedAnswers.FindAll(a => a.QuestionId == CurrentQuestion.Id); }
        public int CurrentQuestionNumber { get; set; }
        public int QuestionCount { get => allQuestions.Count; }
        public string EnteredAnswerText { get; set; } = "";

        public Visibility AnswerBoxVisibility { get; set; }
        public Visibility AnswerChoiceVisibility { get; set; }

        public RelayCommand OpenNextQuestion { get; set; }
        public RelayCommand OpenPrevQuestion { get; set; }
        public RelayCommand SubmitExam { get; set; }

        public ExamTakingViewModel(MainViewModel mainVM, Exam exam)
        {
            CurrentExam = exam;
            mainVM.CurrentExam = CurrentExam;

            allQuestions = new List<Question>();
            allAnswers = new List<Answer>();
            submittedAnswers = new List<Answer>();

            CurrentQuestionNumber = 1;

            InitializeAsync();

            OpenNextQuestion = new RelayCommand(() => NextQuestion(), () => CurrentQuestionNumber < allQuestions.Count);
            OpenPrevQuestion = new RelayCommand(() => PrevQuestion(), () => CurrentQuestionNumber > 1);

            SubmitExam = new RelayCommand(() => { 
                mainVM.CurrentView = new ExamListPage(mainVM); 
                mainVM.CurrentExam = null; 
                Submit(); 
            }, 
            () => true);
        }

        private async void InitializeAsync()
        {
            allQuestions = (await Api.GetListAsync<List<Question>>("Question")).FindAll(q => q.ExamId == CurrentExam.Id && q.StatusId == 1);

            var answers1 = await Api.GetListAsync<List<Answer>>("Answer");
            foreach (var question in allQuestions)
            {
                allAnswers.AddRange(answers1.FindAll(a => a.QuestionId == question.Id));
            }

            var answers2 = await Api.GetListAsync<List<Answer>>("Answer");
            foreach (var question in allQuestions)
            {
                submittedAnswers.AddRange(answers2.FindAll(a => a.QuestionId == question.Id));
            }

            submittedAnswers.ForEach(a => a.IsCorrect = false);

            CurrentQuestion = allQuestions[0];

            CheckQuestionType();
            Refresh();
        }

        public void NextQuestion()
        {
            CheckTextAnswer();
            CurrentQuestionNumber++;
            CurrentQuestion = allQuestions[CurrentQuestionNumber - 1];
            CheckQuestionType();
            Refresh();
        }

        private void CheckTextAnswer()
        {
            if (CurrentQuestionAnswers.Count == 1)
            {
                var textAnswer = submittedAnswers.Find(a => a.QuestionId == CurrentQuestion.Id);
                var correctAnswer = allAnswers.Find(a => a.QuestionId == CurrentQuestion.Id);
                textAnswer.IsCorrect = EnteredAnswerText.ToLower() == correctAnswer.AnswerText.ToLower();
            }
        }

        public void PrevQuestion()
        {
            CurrentQuestionNumber--;
            CurrentQuestion = allQuestions[CurrentQuestionNumber - 1];
            CheckQuestionType();
            Refresh();
        }

        public void CheckQuestionType()
        {
            if (CurrentQuestionAnswers.Count == 1)
            {
                AnswerBoxVisibility = Visibility.Visible;
                AnswerChoiceVisibility = Visibility.Collapsed;
            }
            if (CurrentQuestionAnswers.Count > 1)
            {
                AnswerBoxVisibility = Visibility.Collapsed;
                AnswerChoiceVisibility = Visibility.Visible;
            }
            OnPropertyChanged(nameof(AnswerBoxVisibility));
            OnPropertyChanged(nameof(AnswerChoiceVisibility));
        }

        private void Refresh()
        {
            OnPropertyChanged(nameof(CurrentQuestion));
            OnPropertyChanged(nameof(CurrentQuestionAnswers));
            OnPropertyChanged(nameof(CurrentQuestionNumber));
            OnPropertyChanged(nameof(QuestionCount));
        }

        public async void Submit()
        {
            CheckTextAnswer();
            var result = new StudentExam();

            result.ExamId = CurrentExam.Id;
            result.DateTaken = DateTime.Now;
            result.StudentId = AppSettings.CurrentUserId;

            int countAll = allAnswers.Count(a => a.IsCorrect);
            int points = 0;
            float rightCount = 0;

            foreach (var question in allQuestions)
            {
                int countRight = 0;
                int countWrong = 0;

                foreach (var answer in submittedAnswers.FindAll(a => a.QuestionId == question.Id))
                {
                    var matchingAnswer = allAnswers.Find(a => a.Id == answer.Id);
                    if (matchingAnswer.IsCorrect && answer.IsCorrect)
                        countRight++;
                    if (!matchingAnswer.IsCorrect && answer.IsCorrect)
                        countWrong++;
                }

                int countAllRight = allAnswers.Count(a => a.QuestionId == question.Id && a.IsCorrect);

                if (countRight > countWrong)
                {
                    points += countRight - countWrong;
                    rightCount += (float)countRight / countAllRight;
                }
            }

            float score = (float)points / countAll * 100;
            result.Score = score > 0 ? (int)score : 0;

            string answers = JsonSerializer.Serialize(submittedAnswers, new JsonSerializerOptions { 
                PropertyNameCaseInsensitive = false,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)});
            result.Answers = answers;

            await Api.PostAsync(result, "StudentExam");

            MessageBox.Show($"{rightCount} из {QuestionCount} ({result.Score} баллов)", "Ваш результат"); 
        }
    }
}
