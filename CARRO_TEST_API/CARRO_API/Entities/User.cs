using System;
using System.Collections.Generic;

namespace CARRO_API.Entities
{
    public partial class User
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public DateTime Brithdate { get; set; }
        public bool Active { get; set; }
        public string ModifyBy { get; set; } = null!;
        public DateTime ModifyDate { get; set; }
    }
}
