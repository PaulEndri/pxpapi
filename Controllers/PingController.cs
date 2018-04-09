#region

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace PixelPubApi.Controllers
{
    [Route("api/[controller]")]
    public class PingController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> check()
        {
            await Task.Delay(1);

            return Ok();
        }
    }
}