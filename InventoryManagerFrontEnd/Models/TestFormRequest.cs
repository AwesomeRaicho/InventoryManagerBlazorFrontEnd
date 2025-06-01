using System.ComponentModel.DataAnnotations;

namespace InventoryManagerFrontEnd.Models
{
    public class TestFormRequest
    {
        [Required]
        public string? UserId { get; set; }
        [Required] 
        public string? Name { get; set; }
        [Required]
        [Range(18, 100)]
        public int? Age { get; set; }



        //login creds
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required] 
        public string? Password { get; set; }
    }
}
