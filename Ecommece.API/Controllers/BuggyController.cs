using Ecommece.API.Errors;
using Ecommece.Core.Models;
using Ecommece.EF.Data;
using Microsoft.AspNetCore.Mvc;

namespace Ecommece.API.Controllers
{
    public class BuggyController : ControllerBase
    {
        private Context _context;
        public BuggyController(Context context) 
        { 
            _context = context;
        }

        [HttpGet("NotFound")]
        public  ActionResult getNotFoundRequest()
        {
            return NotFound();
        }

        [HttpGet("BadRequest")]
        public ActionResult getBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }
        [HttpGet("BadRequest/{id}")]
        public ActionResult getBadRequest(int id)
        {
            return BadRequest();
        }

    }
}
