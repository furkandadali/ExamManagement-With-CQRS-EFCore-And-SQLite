namespace Entities.Models
{
    public class Articles : Base
    {
        public string Key { get; set; }
        public string Header { get; set; }
        public string Url { get; set; }
        public string ShortInfo { get; set; }
        public string Content { get; set; }
    }
}
