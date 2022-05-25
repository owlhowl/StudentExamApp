using Microsoft.EntityFrameworkCore;
using ModelsApi;

namespace StudentExamApi.DB
{
    public partial class StudentExamDBContext : DbContext
    {
        public StudentExamDBContext()
        {
        }

        public StudentExamDBContext(DbContextOptions<StudentExamDBContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<Answer> Answers { get; set; } = null!;
        public virtual DbSet<Exam> Exams { get; set; } = null!;
        public virtual DbSet<Group> Groups { get; set; } = null!;
        public virtual DbSet<Question> Questions { get; set; } = null!;
        public virtual DbSet<Status> Statuses { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;
        public virtual DbSet<StudentExam> StudentExams { get; set; } = null!;
        public virtual DbSet<Subject> Subjects { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Integrated Security = True; Server=(localdb)\\MSSQLLocalDB; Initial Catalog=StudentExamDB;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Answer>(entity =>
            {
                entity.HasIndex(e => e.QuestionId, "IX_Answers_QuestionId");

                entity.Property(e => e.AnswerText).HasColumnType("ntext");

                entity.Property(e => e.StatusId).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Exam>(entity =>
            {
                entity.HasIndex(e => e.StatusId, "IX_Exams_StatusId");

                entity.HasIndex(e => e.SubjectId, "IX_Exams_SubjectId");

                entity.Property(e => e.Title).HasMaxLength(100);
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.HasIndex(e => e.StatusId, "IX_Groups_StatusId");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.HasIndex(e => e.ExamId, "IX_Questions_ExamId");

                entity.Property(e => e.QuestionText).HasColumnType("ntext");

                entity.Property(e => e.StatusId).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("Status");

                entity.Property(e => e.Name).HasMaxLength(20);
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasIndex(e => e.GroupId, "IX_Students_GroupId");

                entity.HasIndex(e => e.StatusId, "IX_Students_StatusId");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(50);
            });

            modelBuilder.Entity<StudentExam>(entity =>
            {
                entity.HasIndex(e => e.ExamId, "IX_StudentExams_ExamId");

                entity.HasIndex(e => e.StudentId, "IX_StudentExams_StudentId");

                entity.Property(e => e.Answers).HasColumnType("ntext");

                entity.Property(e => e.DateTaken).HasColumnType("datetime");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.HasIndex(e => e.StatusId, "IX_Subjects_StatusId");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
