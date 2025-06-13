using System.ComponentModel.DataAnnotations;

namespace SamexVLS_2025.Models
{
    public class MR23_cotizacion
    {
        public int Id { get; set; }
        [Required] public string Customer { get; set; }
        public string? SalesRep { get; set; }
        public string? Address { get; set; }
        public string? ContactName { get; set; }
        public bool IsEstimate { get; set; }
        public bool IsPrequote { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime LastUpdateDate { get; set; }

        public ICollection<MR23_cotizacion_detalles>? QuoteDetails { get; set; }


   

    }
}
