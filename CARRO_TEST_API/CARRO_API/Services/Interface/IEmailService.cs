using CARRO_API.Entities;
using CARRO_API.Models;

namespace CARRO_API.Services.Interface
{
    public interface IEmailService
    {
        Task SendEmailSigupSuccess(User user);
    }
}
