using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Models.DTOs
{
    public class UpdateUserDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string AboutMe { get; set; }
        public string Title { get; set; }
        public DateTime Birthday { get; set; }
        public IFormFile ProfilePicture { get; set; }
    }
}
