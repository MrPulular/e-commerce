using Microsoft.AspNetCore.Mvc;
using PulularProje.Models;

namespace PulularProje.View_Components
{
    public class Email:ViewComponent
    {
        PulularProjeContext context = new PulularProjeContext();
        public string Invoke()
        {
            string email = context.Settings.FirstOrDefault(s => s.settingID == 1).email;
            return $"{email}";
        }
    }
}
