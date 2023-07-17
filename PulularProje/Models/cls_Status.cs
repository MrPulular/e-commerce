using Microsoft.EntityFrameworkCore;

namespace PulularProje.Models
{
    public class cls_Status
    {
        PulularProjeContext context = new PulularProjeContext();
        public async Task<List<Status>> StatusSelect()
        {
            List<Status> statuses = await context.Statuses.ToListAsync();
            return statuses;
        }


        public static bool StatusInsert(Status status)
        {
            try
            {
                
                using (PulularProjeContext context = new PulularProjeContext())
                {
                    context.Add(status);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        public async Task<Status> StatusDetails(int? id)
        {
            Status? status = await context.Statuses.FindAsync(id);
            return status;
        }

        public static bool StatusUpdate(Status status)
        {
            try
            {
                
                using (PulularProjeContext context = new PulularProjeContext())
                {
                    context.Update(status);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static bool StatusDelete(int id)
        {
            try
            {
                using (PulularProjeContext context = new PulularProjeContext())
                {
                    Status? status = context.Statuses.FirstOrDefault(c => c.StatusID == id);
                    status.Active = false;
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
