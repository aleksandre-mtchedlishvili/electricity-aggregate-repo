namespace Aggregation.Api.Dtos
{
    public class ElectricityAggregateDto
    {
        public string Tinklas { get; set; }
        public int Count { get; set; }
        public decimal PSum { get; set; }
    }
}
