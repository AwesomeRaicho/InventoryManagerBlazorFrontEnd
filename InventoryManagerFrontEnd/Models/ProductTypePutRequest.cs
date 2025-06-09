using System.ComponentModel.DataAnnotations;

namespace InventoryManagerFrontEnd.Models
{
    public class ProductTypePutRequest
    {
        [Required]
        public string? Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public byte[]? ConcurrencyStamp { get; set; }
    }
}
