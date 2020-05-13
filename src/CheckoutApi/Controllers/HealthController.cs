using System.Net;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Checkout.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, null)]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
