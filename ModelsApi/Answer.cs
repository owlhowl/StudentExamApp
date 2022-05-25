namespace ModelsApi
{
    public class Answer : ApiBaseType
    {
        public string AnswerText { get; set; } = null!;
        public bool IsCorrect { get; set; }
        public int QuestionId { get; set; }
        public int StatusId { get; set; }
    }
}
