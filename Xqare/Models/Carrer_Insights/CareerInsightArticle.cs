namespace Xqare.Models.Carrer_Insights
{
    public class CareerInsightArticle
    {
        public string Id { get; init; } = "";
        public string CategoryId { get; init; } = "";
        public string CategoryName { get; init; } = "";
        public string Title { get; init; } = "";
        public string Summary { get; init; } = "";
        public string ImageUrl { get; init; } = "";
        public CareerInsightAuthor Author { get; init; } = new();
        public DateTime PublishedOn { get; init; }
        public int ReadMinutes { get; init; }
        public bool IsTrending { get; init; }
        public bool IsNew { get; init; }
        public bool IsFeatured { get; init; }
        public IReadOnlyList<string> Tags { get; init; } = [];
    }
}
