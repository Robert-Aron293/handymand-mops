using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Models.DTOs
{
    public class UserResponseDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Token { get; set; }

        public UserResponseDTO(User user, string token)
        {

            FirstName = user.FirstName;
            LastName = user.LastName;
            Token = token;
        }
    }
}
