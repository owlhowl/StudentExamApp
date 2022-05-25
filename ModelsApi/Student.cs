namespace ModelsApi
{
    public partial class Student : ApiBaseType
    {
        public string Name { get; set; } = null!;
        public string Password { get; set; } = "";
        public int GroupId { get; set; }
        public int StatusId { get; set; } = 1;
    }
}
