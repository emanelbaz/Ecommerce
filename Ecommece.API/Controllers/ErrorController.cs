using Ecommece.API.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Ecommece.API.Controllers
{
    [ApiController]
    [Route("errors/{code}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        public IActionResult Error(int code) 
        { 
            return new ObjectResult (new ApiResponse( code));
        }
        
    }
}
