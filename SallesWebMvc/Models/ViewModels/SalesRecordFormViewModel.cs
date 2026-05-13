namespace SallesWebMvc.Models.ViewModels
{
    public class SalesRecordFormViewModel
    {
        public SalesRecord SalesRecord { get; set; }

        public ICollection<Seller> Sellers { get; set; } = new List<Seller>();
    }
}
