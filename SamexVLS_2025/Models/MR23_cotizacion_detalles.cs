using System.ComponentModel.DataAnnotations;

namespace SamexVLS_2025.Models
{
    public class MR23_cotizacion_detalles
    {
        public int Id { get; set; }
        public string? Class { get; set; }
        public string? Profile { get; set; }
        public string? Description { get; set; }
        public string? BillingContainer { get; set; }
        public string? CollectionContainer { get; set; }
        public string? ContainerDescription { get; set; }

        public string? Treatment { get; set; }
        public string? Destination { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public string? Currency { get; set; }
        public float? TransportationCost { get; set; }
        public float? Overcharges { get; set; }
        public float? Minimum { get; set; }
        public bool Charge {  get; set; }
        public float? Maxweight { get; set; }
        public string? Notes { get; set; }
        public bool ShowInQuote { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdateDate { get; set; }

        [Required] public int ParentId { get; set; }

        public MR23_cotizacion QuoteParent { get; set; }



    }
}
