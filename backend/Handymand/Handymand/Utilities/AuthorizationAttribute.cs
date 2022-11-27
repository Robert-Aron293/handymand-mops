using Handymand.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Utilities
{
    public class AuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        private readonly ICollection<Role> _roles;

        public AuthorizationAttribute(params Role[] roles)
        {
            _roles = roles;
        }

        //In Functie de rolurile definite in aplicatie
        //Verificam daca rolurile primite ca parametru la acest atribut exista
        //Daca este sunt null atunci intoarce unauthorized.


        /*            var user = (User)context.HttpContext.Items["User"];

            if (user == null || !_roles.Contains(user.Role))
            {
                context.Result = unauthorizedStatusCodeObject;
            }*/



        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var unauthorizedStatusCodeObject = new JsonResult(new { Message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };

            if (_roles == null)
            {
                context.Result = unauthorizedStatusCodeObject;
            }



            var user = context.HttpContext.User;

            if(user != null)
            {
                var userRole = user.FindFirst("role");

                if(userRole != null)
                {
                    try
                    {
                        if (!_roles.Contains((Role)Convert.ToInt32(userRole.Value)))
                        {
                            context.Result = unauthorizedStatusCodeObject;
                        }
                    }
                    catch (Exception e)
                    {
                        context.Result = unauthorizedStatusCodeObject;
                    }
                }
            }
        }
    }
}