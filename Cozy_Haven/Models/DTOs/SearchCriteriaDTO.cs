namespace Cozy_Haven.Models.DTOs
{
    public class SearchCriteriaDTO
    {
        public string SearchText { get; set; } // Used for searching by hotel name or location
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
