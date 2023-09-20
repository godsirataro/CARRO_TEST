using CARRO_API.Models;

namespace CARRO_API.Services.Interface
{
    public interface IAuthrorizationsService
    {
        SiginModel Sigin(SiginQuery req);
        SiginModel Sigup(SigupQuery req);
    }
}
