using System.ComponentModel.DataAnnotations;

namespace MonResto.API.Dto
{
    public class NewUserDTO
    {
        [Required]
        [EmailAddress]
        [Key]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string UserName { get; set; }
        public string? phoneNumber { get; set; }
    }
}
