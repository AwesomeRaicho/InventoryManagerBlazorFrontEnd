using System.ComponentModel.DataAnnotations;

namespace InventoryManagerFrontEnd.Models
{
    public class TokenResponse
    {
        [Required]
        public string? AccessToken { get; set; }

        [Required]
        public string? RefreshToken { get; set; }

        [Required]
        public DateTime ExpiresAt { get; set; }
    }
}
