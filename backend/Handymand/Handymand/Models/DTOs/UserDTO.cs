using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Handymand.Models.DTOs
{
    public class UserDTO
    {
        public int? Id { get; set; }
        public string Username { get; set; }
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
        public bool Blocked { get; set; }

        [JsonConverter(typeof(JsonToByteArrayConverter))]
        public byte[] Iv { get; set; }

    }
}
