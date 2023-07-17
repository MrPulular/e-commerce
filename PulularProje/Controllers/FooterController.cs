using Microsoft.AspNetCore.Mvc;

namespace PulularProje.Controllers
{
    public class FooterController : Controller
    {
        public IActionResult InformationalSecurityPolicy()
        {
            return View();
        }

        public IActionResult WorkhealthSecurityandEnvironmentPolicy()
        {
            return View();
        }
    }
}
