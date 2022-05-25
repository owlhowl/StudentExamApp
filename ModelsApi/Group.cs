namespace ModelsApi
{
    public partial class Group : ApiBaseType
    {
        public string Name { get; set; } = null!;
        public int StatusId { get; set; } = 1;
    }
}
