using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using WANYEN.API.COMMON.Attributes;
using WANYEN.API.COMMON.Models;

namespace WANYEN.API.COMMON.Authentications
{
    [TransientPriorityRegistration]
    public class UserLocalService : IUserLocalService
    {
        private readonly IHttpContextAccessor _httpContext;
        private ITokenManager _tokenManager;

        public UserLocalService(IHttpContextAccessor httpContext
            , AppSettings appSettings
            , ITokenManager tokenManager            )
        {
            _httpContext = httpContext;
            _tokenManager = tokenManager;
        }

        public string GetAuthToken()
        {

            string t = _httpContext.HttpContext.Request.Headers["Authorization"];
            return t;
        }

        public HttpContext GetHttpContext()
        {
            return _httpContext.HttpContext;
        }

        public UserCredential GetUserCredential(string token = "")
        {
            if (string.IsNullOrEmpty(token))
                token = GetAuthToken();

            if (string.IsNullOrEmpty(token))
                return null;

            var user = _tokenManager.GetPrincipal(token);
            if (user == null) return null;
            var result = new UserCredential();
            result.Email = _tokenManager.GetClaim(ClaimStore.email, token);
            result.UserId = _tokenManager.GetClaim(ClaimStore.userId, token);
            var groupId = _tokenManager.GetClaim(ClaimStore.groupId, token);
            if (!string.IsNullOrEmpty(groupId))
                result.GroupId = Convert.ToInt32(groupId);
            result.Name = _tokenManager.GetClaim(ClaimStore.name, token);
            var isAdmin = _tokenManager.GetClaim(ClaimStore.isAdmin, token);
            if (!string.IsNullOrEmpty(isAdmin))
            {
                bool error;
                if (bool.TryParse(isAdmin, out error))
                    result.IsAdmin = Convert.ToBoolean(isAdmin);
            }

            return result;
        }
    }
}
