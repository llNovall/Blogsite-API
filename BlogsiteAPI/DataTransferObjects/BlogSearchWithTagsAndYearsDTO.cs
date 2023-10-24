namespace BlogsiteAPI.DataTransferObjects
{
    public class BlogSearchWithTagsAndYearsDTO
    {
        public List<string> TagIds { get; set; } = new List<string>();

        public List<int> Years { get; set; } = new List<int>();
    }
}