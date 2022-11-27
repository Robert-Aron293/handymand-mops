using Handymand.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Handymand.Utilities
{
    public class DTOforIds
    {
        public int? Id { get; set; }
        public string cryptId { get; set; }
        public int? UserId { get; set; }

        [JsonConverter(typeof(JsonToByteArrayConverter))]
        public byte[] Iv { get; set; }
    }
}