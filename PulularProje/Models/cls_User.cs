using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using System.Text;
using XSystem.Security.Cryptography;

namespace PulularProje.Models
{
    public class cls_User
    {
        PulularProjeContext context = new PulularProjeContext();

        public async Task<User> loginControl(User user)
        {
            string md5Sifre = MD5Sifrele(user.Password);

            User? usr = await context.Users.FirstOrDefaultAsync(u => u.Email == user.Email && u.Password == md5Sifre && u.IsAdmin == true && u.Active == true);

            return usr;
        }

        public static User? SelectMemberInfo(string email)
        {
            using (PulularProjeContext context = new PulularProjeContext())
            {
                User? user = context.Users.FirstOrDefault(u => u.Email == email);
                return user;
            }
        }


        public static string MemberControl(User user)
        {
            using (PulularProjeContext context = new PulularProjeContext())
            {
                string answer = "";

                try
                {
                    string md5Sifre = MD5Sifrele(user.Password);
                    User? usr = context.Users.FirstOrDefault(u => u.Email == user.Email && u.Password == md5Sifre);

                    if (usr == null)
                    {
                        
                        answer = "error";
                    }
                    else
                    {
                        
                        if (usr.IsAdmin == true)
                        {
                            
                            answer = "admin";
                        }
                        else
                        {
                            answer = usr.Email;
                        }
                    }
                }
                catch (Exception)
                {
                    return "HATA";
                }
                return answer;
            }
        }


        public static string MD5Sifrele(string value)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] btr = Encoding.UTF8.GetBytes(value);
            btr = md5.ComputeHash(btr);

            StringBuilder sb = new StringBuilder();
            foreach (byte item in btr)
            {
                sb.Append(item.ToString("x2").ToLower());
            }
            return sb.ToString();
        }


        public static bool AddUser(User user)
        {
            using (PulularProjeContext context = new PulularProjeContext())
            {
                try
                {
                    user.Active = true;
                    user.IsAdmin = false;
                    user.Password = MD5Sifrele(user.Password);
                    context.Users.Add(user);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public static bool loginEmailControl(User user)
        {
            using (PulularProjeContext context = new PulularProjeContext())
            {
                User? usr = context.Users.FirstOrDefault(u => u.Email == user.Email);

                if (usr == null)
                {
                    return false;
                }
                return true;
            }
        }

        public static void Send_Sms(string OrderGroupGUID)
        {
            using (PulularProjeContext context = new PulularProjeContext())
            {
                string ss = "";
                ss += "<?xml version='1.0' encoding='UTF-8' >";
                ss += "<mainbody>";
                ss += "<header>";
                ss += "<company dil='TR'>iakademi(üye oldugunuzda size verilen şirket ismi)</company>";
                ss += "<usercode>0850 size verilen user kod burada yazılacak</usercode>";
                ss += "<password>NetGsm123. size verilen şifre burada yazılacak</password>";
                ss += "<startdate></startdate>";
                ss += "<stopdate></stopdate>";
                ss += "<type>n:n</type>";
                ss += "<msgheader>başlık buraya</msgeader>";
                ss += "</header>";
                ss += "<body>";

                Order order = context.Orders.FirstOrDefault(o => o.OrderGroupGUID == OrderGroupGUID);
                User user = context.Users.FirstOrDefault(u => u.UserID == order.UserID);                
                string content = "Sayın " + user.NameSurname + "," + DateTime.Now + " tarihinde " + OrderGroupGUID + " nolu siparişiniz alınmıştır.";

                ss += "<mp><msg><![CDATA[" + content + "]]></msg><no>90" + user.Telephone + "</no></mp>";
                ss += "</body>";
                ss += "</mainbody>";

                string answer = XMLPOST("https://api.netgsm.com/tr/xmlbulkhttppost.asp", ss);
                if (answer != "-1")
                {
                    //sms gitti
                }
                else
                {
                    //sms gitmedi
                }
            }
        }

        public static string XMLPOST(string url, string xmlData)
        {
            try
            {
                WebClient wUpload = new WebClient();
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest; 
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";

                Byte[] bPostArray = Encoding.UTF8.GetBytes(xmlData);
                Byte[] bResonse = wUpload.UploadData(url, "POST", bPostArray);

                Char[] sReturnsChars = Encoding.UTF8.GetChars(bResonse);

                string sWebPage = new string(sReturnsChars);
                return sWebPage;
            }
            catch (Exception)
            {
                return "-1";
            }
        }

        public static void Send_Email(string OrderGroupGUID)
        {
            using (PulularProjeContext context = new PulularProjeContext())
            {
                Order order = context.Orders.FirstOrDefault(o => o.OrderGroupGUID == OrderGroupGUID);
                User user = context.Users.FirstOrDefault(u => u.UserID == order.UserID);

                string mail = "kaanpulular@gmail.com";
                string _mail = user.Email;
                string subject = "";
                string content = "";

                content = "Sayın " + user.NameSurname + "," + DateTime.Now + " tarihinde " + OrderGroupGUID + " nolu siparişiniz alınmıştır.";

                subject = "Sayın " + user.NameSurname + " siparişiniz alınmıştır.";

                string host = "smtp.pulular.com";
                int port = 587;
                string login = "mailserver a baglanılan login buraya";
                string password = "mailserver a baglanılan şifre buraya";

                MailMessage e_posta = new MailMessage();
                e_posta.From = new MailAddress(mail, "Sipariş bilgi"); 
                e_posta.To.Add(_mail); 
                e_posta.Subject = subject;
                e_posta.IsBodyHtml = true;
                e_posta.Body = content;

                SmtpClient smtp = new SmtpClient();
                smtp.Credentials = new NetworkCredential(login, password);
                smtp.Port = port;
                smtp.Host = host;

                try
                {
                    smtp.Send(e_posta);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
