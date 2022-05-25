using Microsoft.EntityFrameworkCore;
using ModelsApi;

namespace StudentExamApi.DB
{
    public partial class StudentExamDBContext : DbContext
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Status>().HasData(
                new Status { Id = 1, Name = "Активно"},
                new Status { Id = 2, Name = "Удалено"});

            modelBuilder.Entity<Group>().HasData(
                new Group { Id = 1, Name = "1135", StatusId = 1},
                new Group { Id = 2, Name = "1125", StatusId = 1 },
                new Group { Id = 3, Name = "1241", StatusId = 1 });

            modelBuilder.Entity<Student>().HasData(
                new Student { Id = 1, Name = "Администратор", Password = "0000", GroupId = 1, StatusId = 1 },
                new Student { Id = 2, Name = "Студент Номер Один", Password = "1111", GroupId = 1, StatusId = 1 },
                new Student { Id = 3, Name = "Студент Номер Два", Password = "2222", GroupId = 2, StatusId = 1 });

            modelBuilder.Entity<Subject>().HasData(
                new Subject { Id = 1, Name = "Математика", StatusId = 1 },
                new Subject { Id = 2, Name = "Информатика", StatusId = 1 });

            modelBuilder.Entity<Exam>().HasData(
                new Exam { Id = 1, SubjectId = 1, Title = "Тест по математике", StatusId = 1 },
                new Exam { Id = 2, SubjectId = 2, Title = "Тест по информатике", StatusId = 1 });

            modelBuilder.Entity<Question>().HasData(
                new Question { Id = 1, ExamId = 1, QuestionText = "Выберите верный вариант (из 3), правильный 2"},
                new Question { Id = 2, ExamId = 1, QuestionText = "Выберите два верных варианта (из 4), правильные 2 и 4"},
                new Question { Id = 3, ExamId = 1, QuestionText = "Напишите свой ответ в поле, правильный - \"Пушкин\""});

            modelBuilder.Entity<Answer>().HasData(
                new Answer { Id = 1, QuestionId = 1, AnswerText = "Ответ 1", IsCorrect = false},
                new Answer { Id = 2, QuestionId = 1, AnswerText = "Ответ 2", IsCorrect = true},
                new Answer { Id = 3, QuestionId = 1, AnswerText = "Ответ 3", IsCorrect = false},

                new Answer { Id = 4, QuestionId = 2, AnswerText = "Ответ 1", IsCorrect = false },
                new Answer { Id = 5, QuestionId = 2, AnswerText = "Ответ 2", IsCorrect = true },
                new Answer { Id = 6, QuestionId = 2, AnswerText = "Ответ 3", IsCorrect = false },
                new Answer { Id = 7, QuestionId = 2, AnswerText = "Ответ 4", IsCorrect = true },

                new Answer { Id = 8, QuestionId = 3, AnswerText = "Пушкин", IsCorrect = true });
        }
    }
}
