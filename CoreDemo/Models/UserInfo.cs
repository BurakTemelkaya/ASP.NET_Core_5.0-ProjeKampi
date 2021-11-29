using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreDemo.Models
{
    public class UserInfo
    {
        public int GetID(ClaimsPrincipal user)
        {
            return int.Parse(((ClaimsIdentity)user.Identity).FindFirst(ClaimTypes.Name).Value);
        }
        public string GetMail(ClaimsPrincipal user)
        {
            return ((ClaimsIdentity)user.Identity).FindFirst(ClaimTypes.Email).Value.ToString();
        }
    }
}
