using Handymand.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Utilities.JWTUtils
{
    public interface IJWTUtils
    {
        public Task<string> GenerateJWTToken(User user);

        public int? ValidateJWTToken(string token);

        public JwtSecurityToken ValidateJWTToken2(string token);
    }
}