using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Handymand.Models.DTOs
{
    public class UserRequestDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Id { get; set; }

        [JsonConverter(typeof(JsonToByteArrayConverter))]
        public byte[] Iv { get; set; }

    }
}
