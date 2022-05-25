namespace ModelsApi
{
    public partial class Exam : ApiBaseType
    {
        public string Title { get; set; } = null!;
        public int SubjectId { get; set; }
        public int StatusId { get; set; } = 1;
    }
}
