namespace Xqare.Models.Carrer_Insights_Detail
{
    public class CareerInsightContentBlock
    {
        public CareerInsightBlockType Type { get; init; }

        public string? Heading { get; init; }

        public string? Text { get; init; }

        public IReadOnlyList<CareerInsightRichItem> Items { get; init; } = [];

        public IReadOnlyList<CareerInsightSignal> Signals { get; init; } = [];
    }

    public class CareerInsightRichItem
    {
        public IReadOnlyList<CareerInsightTextSpan> Spans { get; init; } = [];
    }

    public class CareerInsightTextSpan
    {
        public string Text { get; init; } = string.Empty;

        public bool Bold { get; init; }
    }
}