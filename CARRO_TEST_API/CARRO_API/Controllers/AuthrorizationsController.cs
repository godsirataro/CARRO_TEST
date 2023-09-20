using CARRO_API.Models;
using CARRO_API.Models;
using CARRO_API.Services.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CARRO_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AuthrorizationsController : Controller
    {
        private readonly ILogger<AuthrorizationsController> _logger;
        private IAuthrorizationsService _authrorizationsService;
        public AuthrorizationsController(
            ILogger<AuthrorizationsController> logger
            , IAuthrorizationsService authrorizationsService
            ) 
        {
            _logger = logger;
            _authrorizationsService = authrorizationsService;
        }

        [HttpPost]
        public ResponseResult<SiginModel> Sigin([FromBody] SiginQuery req)
        {

            var res = _authrorizationsService.Sigin(req);

            return ResponseResult<SiginModel>.Success(res);

        }

        [HttpPost]
        public ResponseResult<SiginModel> Sigup([FromBody] SigupQuery req)
        {

            var res = _authrorizationsService.Sigup(req);

            return ResponseResult<SiginModel>.Success(res);

        }
    }
}
