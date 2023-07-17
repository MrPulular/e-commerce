using Microsoft.EntityFrameworkCore;

namespace PulularProje.Models
{
    public class cls_Setting
    {

        PulularProjeContext context = new PulularProjeContext();
        public Setting SettingDetails()
        {
            Setting? setting = context.Settings.FirstOrDefault(s => s.settingID == 1);
            return setting;
        }


        public static bool SettingUpdate(Setting setting)
        {
            try
            {                
                using (PulularProjeContext context = new PulularProjeContext())
                {
                    context.Update(setting);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
