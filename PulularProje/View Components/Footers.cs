using Microsoft.AspNetCore.Mvc;
using PulularProje.Models;
using XAct;

namespace PulularProje.View_Components
{
    public class Footers:ViewComponent
    {
        PulularProjeContext context = new PulularProjeContext();
        public IViewComponentResult Invoke()
        {
            List<Supplier> suppliers = context.Suppliers.ToList();
            return View(suppliers);
        }
    }
}
