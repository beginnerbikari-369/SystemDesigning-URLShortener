namespace UrlShortener.Models
{
    public class UrlMapping
    {
        public int Id { get; set; }
        public string ShortCode { get; set; } = string.Empty;
        public string LongUrl { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
