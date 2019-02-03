using InambeBlog.Models.Post;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace InambeBlog.Helpers
{
    public static class ExtensionMethods
    {
        public static string ControllerName(this string str)
        {
            return  str.Replace("Controller", "");
        }
        public static int Id(this ClaimsPrincipal user)
        {
            return Convert.ToInt32(user.Claims.FirstOrDefault(r => r.Type == nameof(PostModel.Id)).Value);
        }
    }
}
