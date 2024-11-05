using System.ComponentModel.DataAnnotations;

namespace MonRestoAPI.Models
{
    public class UserProfile
    {
        [Key]
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public ICollection<Order> OrderHistory { get; set; }
    }

}
