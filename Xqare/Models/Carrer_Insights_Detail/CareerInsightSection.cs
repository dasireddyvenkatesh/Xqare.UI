namespace Xqare.Models.Carrer_Insights_Detail
{
    public class CareerInsightSection
    {
        public string Id { get; init; } = "";
        public string Title { get; init; } = "";
        public IReadOnlyList<CareerInsightContentBlock> Blocks { get; init; } = [];
    }
}
