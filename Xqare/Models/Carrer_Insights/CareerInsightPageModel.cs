namespace Xqare.Models.Carrer_Insights
{
    public class CareerInsightPageModel
    {
        public string BrandName { get; init; } = "XQARE Career Insights";
        public string Subtitle { get; init; } = "Expert guidance, mentorship advice, and career strategies to help professionals grow and succeed in the IT industry.";
        public string SearchPlaceholder { get; init; } = "Search career advice, mentorship insights, or job preparation tips...";
        public IReadOnlyList<CareerInsightCategory> Categories { get; init; } = [];
        public CareerInsightArticle? FeaturedArticle { get; init; }
        public IReadOnlyList<CareerInsightArticle> Articles { get; init; } = [];
    }
}
