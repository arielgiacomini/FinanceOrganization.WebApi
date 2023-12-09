using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class WalletToPayController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}