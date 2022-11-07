using CsvHelper.Configuration.Attributes;

namespace Aggregation.Api.Dtos
{
    public class ElectricityDto
    {
        [Name("TINKLAS")]
        public string Tinklas { get; set; }

        [Name("OBT_PAVADINIMAS")]
        public string ObtPavadinimas { get; set; }

        [Name("P+")]
        public decimal? PPlus { get; set; }

        [Name("P-")]
        public decimal? PMinus { get; set; }

        //[Name("OBJ_GV_TIPAS")]
        //public string Tipas { get; set; }

        //[Name("OBJ_NUMERIS")]
        //public int Numeris { get; set; }

        //[Name("PL_T")]
        //public DateTime Date { get; set; }

    }
}
