using Microsoft.AspNetCore.Mvc;
using PulularProje.Models;
using XAct;

namespace PulularProje.View_Components
{
    public class Menus:ViewComponent
    {
        PulularProjeContext context = new PulularProjeContext();
        public IViewComponentResult Invoke()
        {
            List<Category> categories = context.Categories.ToList();
            return View(categories);
        }
    }
}
