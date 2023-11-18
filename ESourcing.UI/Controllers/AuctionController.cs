using ESourcing.Core.Repositories;
using ESourcing.UI.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace ESourcing.UI.Controllers
{
    public class AuctionController : Controller
    {
        private readonly IUserRepository _userRepository;
        public AuctionController(IUserRepository userRepository)
        {
            userRepository = _userRepository;
        }

        public IActionResult Index()
        {
            List<AuctionViewModel> model = new List<AuctionViewModel>();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var userList = await _userRepository.GetAllAsync();
            ViewBag.UserList = userList;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AuctionViewModel model)
        {
            return View(model);
        }

        public async Task<IActionResult> Detail(string id)
        {
            return View();
        }
    }
}
