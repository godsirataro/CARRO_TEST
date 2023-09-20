using CARRO_API.Models;

namespace CARRO_API.Services.Interface
{
    public interface IUserService
    {
        UserCredential Sigin(SiginQuery req);
        UserCredential Sigup(SigupQuery req);
    }
}
