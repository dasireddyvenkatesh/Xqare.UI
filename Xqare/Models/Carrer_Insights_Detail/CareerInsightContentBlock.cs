namespace Xqare.Models.Carrer_Insights_Detail
{
    public class CareerInsightContentBlock
    {
        public CareerInsightBlockType Type { get; init; }
        public string? Heading { get; init; }
        public string? Text { get; init; }
        public IReadOnlyList<string> Items { get; init; } = [];
        public IReadOnlyList<CareerInsightSignal> Signals { get; init; } = [];
    }
}
