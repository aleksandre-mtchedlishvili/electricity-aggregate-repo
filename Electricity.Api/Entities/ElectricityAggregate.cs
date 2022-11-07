namespace Aggregation.Api.Entities
{
    public class ElectricityAggregate
    {
        public int Id { get; set; }
        public string Tinklas { get; set; }
        public int Count { get; set; }
        public decimal PSum { get; set; }
    }
}
