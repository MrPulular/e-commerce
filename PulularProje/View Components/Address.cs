using Microsoft.AspNetCore.Mvc;
using PulularProje.Models;

namespace PulularProje.View_Components
{
    public class Address:ViewComponent
    {
        PulularProjeContext context = new PulularProjeContext();

        public string Invoke()
        {
            string address = context.Settings.FirstOrDefault(s => s.settingID == 1).address;
            return $"{address}";
        }
    }
}
