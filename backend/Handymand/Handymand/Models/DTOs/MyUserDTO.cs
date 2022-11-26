using System;
using System.IO;

namespace Handymand.Models.DTOs
{
    public class MyUserDTO
    {
        public int? Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Location { get; set; }
        public string WalletAddress { get; set; }
        public double Amount { get; set; }
        public string AboutMe { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Title { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? Birthday { get; set; }
    }
}
