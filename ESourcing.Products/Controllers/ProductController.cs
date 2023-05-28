using Microsoft.AspNetCore.Mvc;

namespace ESourcing.Products.Controllers
{
    [Route("api/v1/[controller]")]
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
