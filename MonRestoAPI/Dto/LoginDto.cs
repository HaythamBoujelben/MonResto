using System.ComponentModel.DataAnnotations;

namespace MonResto.API.Dto
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "username obligatoire")]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
