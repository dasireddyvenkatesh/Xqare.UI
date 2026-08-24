using Xqare.Models.Carrer_Insights;

namespace Xqare.Models.Carrer_Insights_Detail
{
    public class CareerInsightDetailModel
    {
        public CareerInsightArticle Article { get; init; } = new();
        public string BackText { get; init; } = "Back to Blog";
        public string HeroImageUrl { get; init; } = "";
        public IReadOnlyList<string> KeyTakeaways { get; init; } = [];
        public IReadOnlyList<CareerInsightSection> Sections { get; init; } = [];
        public CareerInsightCallToAction CallToAction { get; init; } = new();
        public IReadOnlyList<CareerInsightArticle> ContinueReading { get; init; } = [];
    }
}
