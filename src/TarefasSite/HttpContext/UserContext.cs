using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TarefasSite.HttpContext
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserEmail()
        {
            System.Security.Claims.ClaimsPrincipal currentUser = _httpContextAccessor.HttpContext.User;
            var email = currentUser.FindFirst(c => c.Type == ClaimTypes.Email).Value;

            return email;
        }

        public bool IsAuthenticated
        {
             get => _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        }
    }
}
