using Microsoft.AspNetCore.Mvc;

namespace PulularProje.Controllers
{
    public class WebServiceController : Controller
    {
        public static string tckimlik_vergi_no = "";
        public IActionResult Index()
        {
            return View();
        }
    }
}
