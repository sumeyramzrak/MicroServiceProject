using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ESourcing.Sourcing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionController : ControllerBase //Controllerlar herhangi bir view dönmeyeceğinden ControllerBase i miras alıyor. View dönecek olsaydı Controller dan miras alırdı.
    {
    }
}
