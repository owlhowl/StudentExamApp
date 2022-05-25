namespace ModelsApi
{
    public partial class Subject : ApiBaseType
    {
        public string Name { get; set; } = null!;
        public int StatusId { get; set; } = 1;
    }
}
