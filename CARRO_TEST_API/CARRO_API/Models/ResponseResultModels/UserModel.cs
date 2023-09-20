namespace CARRO_API.Models
{
    public class UserCredential
    {
        //public string UserId { get; set; }
        //public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        //public int? GroupId { get; set; }
        //public string GroupName { get; set; }
        //public string PhoneNumber { get; set; }
        //public bool IsAdmin { get; set; }
        //public string ImageUrl { get; set; }
    }
}
