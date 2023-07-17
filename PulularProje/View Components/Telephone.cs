using Microsoft.AspNetCore.Mvc;
using PulularProje.Models;

namespace PulularProje.View_Components
{
    public class Telephone:ViewComponent
    {
        PulularProjeContext context = new PulularProjeContext();

        public string Invoke()
        {
            string telephone = context.Settings.FirstOrDefault(s => s.settingID == 1).telephone;
            return $"{telephone}";
        }

    }
}
