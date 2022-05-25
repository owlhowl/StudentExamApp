namespace ModelsApi
{
    public partial class StudentExam : ApiBaseType
    {
        public int ExamId { get; set; }
        public int StudentId { get; set; }
        public DateTime DateTaken { get; set; }
        public int Score { get; set; }
        public string Answers { get; set; } = null!;
    }
}
