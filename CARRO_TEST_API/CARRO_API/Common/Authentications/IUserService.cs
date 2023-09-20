using CARRO_API.Models;

namespace CARRO_API.Common.Authentications
{
    public interface IUserLocalService
    {
        UserCredential GetUserCredential(string tokne = "");
        string GetAuthToken();
        HttpContext GetHttpContext();
    }
}
