namespace ModelsApi
{
    public partial class Question : ApiBaseType
    {
        public string QuestionText { get; set; } = null!;
        public int ExamId { get; set; }
        public int StatusId { get; set; }
    }
}
