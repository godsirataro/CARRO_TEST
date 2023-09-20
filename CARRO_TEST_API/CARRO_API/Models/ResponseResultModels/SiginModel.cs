using System.ComponentModel.DataAnnotations;

namespace CARRO_API.Models
{
    public class SiginQuery
    {
        [Required]
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class SigupQuery
    {
        [Required]
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Brithdate { get; set; }
    }
    public class SiginModel
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
    }
}

