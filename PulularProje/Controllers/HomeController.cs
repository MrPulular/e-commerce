using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PagedList.Core;
using PulularProje.Models;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net;

namespace PulularProje.Controllers
{
    public class HomeController : Controller
    {
        MainPageModel mpm = new MainPageModel();
        PulularProjeContext context = new PulularProjeContext();
        cls_Product cp = new cls_Product();
        cls_Order o = new cls_Order();
        int mainpageCount = 0;

        public HomeController()
        {
            this.mainpageCount = context.Settings.FirstOrDefault(s => s.settingID == 1).mainpageCount;
        }

        public IActionResult Index()
        {
            mpm.SliderProducts = cp.ProductSelect("Slider", mainpageCount, "", 0);
            mpm.Productofday = cp.ProductDetails("Productofday");//günün ürünü
            mpm.NewProducts = cp.ProductSelect("New", mainpageCount, "", 0); //yeni
            mpm.SpecialProducts = cp.ProductSelect("Special", mainpageCount, "", 0); //özel
            mpm.DiscountedProducts = cp.ProductSelect("Discounted", mainpageCount, "", 0); //indirimli
            mpm.HighlightedProducts = cp.ProductSelect("Highlighted", mainpageCount, "", 0); //öne cıkan
            mpm.TopsellerProducts = cp.ProductSelect("Topseller", mainpageCount, "", 0); //cok satan
            mpm.StarProducts = cp.ProductSelect("Star", mainpageCount, "", 0); //yıldız
            mpm.FeaturedProducts = cp.ProductSelect("Featured", mainpageCount, "", 0); //fırsat
            mpm.NotableProducts = cp.ProductSelect("Notable", mainpageCount, "", 0); //dikkat ceken

            return View(mpm);
        }
        public IActionResult Details(int id)
        {
            
            mpm.ProductDetails = (from p in context.Products where p.ProductID == id select p).FirstOrDefault();

            
            mpm.CategoryName = (from p in context.Products
                                join c in context.Categories
                              on p.CategoryID equals c.CategoryID
                                where p.ProductID == id
                                select c.CategoryName).FirstOrDefault();

            
            mpm.BrandName = (from p in context.Products
                             join s in context.Suppliers
                           on p.SupplierID equals s.SupplierID
                             where p.ProductID == id
                             select s.BrandName).FirstOrDefault();
            
            mpm.RelatedProducts = context.Products.Where(p => p.Related == mpm.ProductDetails!.Related && p.ProductID != id).ToList();

            cls_Product.Highlighted_Increase(id);

            return View(mpm);
        }
        public IActionResult CartProcess(int id)
        {
            cls_Product.Highlighted_Increase(id);
            o.ProductID = id;
            o.Quantity = 1;

            var cookieOptions = new CookieOptions();
            
            var cookie = Request.Cookies["sepetim"];
            if (cookie == null)
            {
                cookieOptions = new CookieOptions();
                cookieOptions.Expires = DateTime.Now.AddDays(1); 
                cookieOptions.Path = "/";
                o.MyCart = "";
                o.AddToMyCart(id.ToString());
                Response.Cookies.Append("sepetim", o.MyCart, cookieOptions); 
                HttpContext.Session.SetString("Message", "Ürün Sepetinize Eklendi.");
                TempData["Message"] = "Ürün Sepetinize Eklendi";
            }
            else
            {
                
                o.MyCart = cookie; 
                if (o.AddToMyCart(id.ToString()) == false)
                {
                    
                    HttpContext.Response.Cookies.Append("sepetim", o.MyCart, cookieOptions);
                    cookieOptions.Expires = DateTime.Now.AddDays(1); 
                    HttpContext.Session.SetString("Message", "Ürün Sepetinize Eklendi.");
                    TempData["Message"] = "Ürün Sepetinize Eklendi";
                }
                else
                {
                    
                    HttpContext.Session.SetString("Message", "Bu Ürün Zaten Sepetinizde Var.");
                    HttpContext.Session.GetString("Message");
                    TempData["Message"] = "Bu Ürün Zaten Sepetinizde Var";
                }
            }
            string url = Request.Headers["Referer"].ToString();
            
            return Redirect(url);
        }
        public IActionResult CategoryPage(int id)
        {
            List<Product> products = cp.ProductSelectWithCategoryID(id);
            return View(products);
        }
        public IActionResult SupplierPage(int id)
        {
            List<Product> products = cp.ProductSelectWithSupplierID(id);
            return View(products);
        }
        public IActionResult NewProducts()
        {
            mpm.NewProducts = cp.ProductSelect("New", mainpageCount, "New", 0); 
            return View(mpm);
        }
        public PartialViewResult _PartialNewProducts(string pageno)
        {
            int pagenumber = Convert.ToInt32(pageno);
            mpm.NewProducts = cp.ProductSelect("New", mainpageCount, "New", pagenumber); 
            return PartialView(mpm);
        }
        public IActionResult SpecialProducts()
        {
            mpm.SpecialProducts = cp.ProductSelect("Special", mainpageCount, "Special", 0); 
            return View(mpm);
        }
        public PartialViewResult _PartialSpecialProducts(string pageno)
        {
            int pagenumber = Convert.ToInt32(pageno);
            mpm.SpecialProducts = cp.ProductSelect("Special", mainpageCount, "Special", pagenumber); 
            return PartialView(mpm);
        }
        public IActionResult DiscountedProducts()
        {
            mpm.DiscountedProducts = cp.ProductSelect("Discounted", mainpageCount, "Discounted", 0); 
            return View(mpm);
        }
        public PartialViewResult _PartialDiscountedProducts(string pageno)
        {
            int pagenumber = Convert.ToInt32(pageno);
            mpm.DiscountedProducts = cp.ProductSelect("Discounted", mainpageCount, "Discounted", pagenumber); 
            return PartialView(mpm);
        }
        public IActionResult HighlightedProducts()
        {
            mpm.HighlightedProducts = cp.ProductSelect("Highlighted", mainpageCount, "Highlighted", 0); 
            return View(mpm);
        }
        public PartialViewResult _PartialHighlightedProducts(string pageno)
        {
            int pagenumber = Convert.ToInt32(pageno);
            mpm.HighlightedProducts = cp.ProductSelect("Highlighted", mainpageCount, "Highlighted", pagenumber); 
            return PartialView(mpm);
        }
        public IActionResult TopsellerProducts(int page = 1, int pageSize = 4)
        {
            PagedList<Product> model = new PagedList<Product>(context.Products.OrderByDescending(p => p.TopSeller), page, pageSize);
                
            return View("TopsellerProducts", model);
        }
        public IActionResult Cart()
        {
            cls_Order o = new cls_Order();
            List<cls_Order> sepet;            

            if (HttpContext.Request.Query["scid"].ToString() != "")
            {
                //sil botunuyla geldim
                string? scid = HttpContext.Request.Query["scid"];
                o.MyCart = Request.Cookies["sepetim"]; 
                o.DeleteFromMyCart(scid); 
                var cookieOptions = new CookieOptions();                
                Response.Cookies.Append("sepetim", o.MyCart, cookieOptions);
                cookieOptions.Expires = DateTime.Now.AddDays(1); 
                TempData["Message"] = "Ürün Sepetten Silindi";                
                sepet = o.SelectMyCart();
                ViewBag.Sepetim = sepet;
                ViewBag.sepet_tablo_detay = sepet;
            }
            else
            {
                //sag üst köseden sepet tıklanınca
                var cookie = Request.Cookies["sepetim"];
                if (cookie == null)
                {
                    o.MyCart = "";
                    sepet = o.SelectMyCart();
                    ViewBag.Sepetim = sepet;
                    ViewBag.sepet_tablo_detay = sepet;
                }
                else
                {
                    var cookieOptions = new CookieOptions();
                    o.MyCart = Request.Cookies["sepetim"];
                    sepet = o.SelectMyCart();
                    ViewBag.Sepetim = sepet;
                    ViewBag.sepet_tablo_detay = sepet;
                }
            }
            if (sepet.Count == 0)
            {
                ViewBag.Sepetim = null;
            }
            return View();
        }
        public IActionResult Order()
        {
            
            if (HttpContext.Session.GetString("Email") != null)
            {
                
                User? usr = cls_User.SelectMemberInfo(HttpContext.Session.GetString("Email"));
                return View(usr);
            }
            else
            {
                
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        public IActionResult Order(IFormCollection frm)
        {
            string txt_individual = Request.Form["txt_individual"];
            string txt_corporate = Request.Form["txt_corporate"];

            if (txt_individual != null)
            {
                //bireysel fatura
                //digital planet
                WebServiceController.tckimlik_vergi_no = txt_individual;
                o.tckimlik_vergi_no = txt_individual;
                o.EfaturaCreate();
            }
            else
            {
                //kurumsal fatura
                WebServiceController.tckimlik_vergi_no = txt_corporate;
                o.tckimlik_vergi_no = txt_corporate;
                o.EfaturaCreate();
            }

            string kredikartno = Request.Form["kredikartno"];
            string kredikartay = frm["kredikartay"];
            string kredikartyil = frm["kredikartyil"];
            string kredikartcvs = frm["kredikartcvs"];

            return RedirectToAction("backref");

            //buradan sonraki kodlar , payu , iyzico

            //payu dan gelen örnek kodlar

            /*  
             
            NameValueCollection data = new NameValueCollection();
            string url = "https://www.kaanpulular.com/backref";

            data.Add("BACK_REF", url);
            data.Add("CC_CVV", kredikartcvs);
            data.Add("CC_NUMBER", kredikartno);
            data.Add("EXP_MONTH", kredikartay);
            data.Add("EXP_YEAR", "20" + kredikartyil);

            var deger = "";

            foreach (var item in data)
            {
                var value = item as string;
                var byteCount = Encoding.UTF8.GetByteCount(data.Get(value));
                deger += byteCount + data.Get(value);
            }

            var signatureKey = "size verilen SECRET_KEY buraya yazılacak";

            var hash = HashWithSignature(deger, signatureKey);

            data.Add("ORDER_HASH", hash);

            var x = POSTFormPAYU("https://secure.payu.com.tr/order/....", data);

            //sanal kart
            if (x.Contains("<STATUS>SUCCESS</STATUS>") && x.Contains("<RETURN_CODE>3DS_ENROLLED</RETURN_CODE>"))
            {
                //sanal kart (debit kart) ile alış veriş yaptı , bankadan onay aldı
            }
            else
            {
                //gerçek kart ile alış veriş yaptı , bankadan onay aldı
            }
            */
        }
        public static string HashWithSignature(string deger, string signatureKey)
        {
            return "";
        }
        public static string POSTFormPAYU(string url, NameValueCollection data)
        {
            return "";
        }
        public IActionResult backref()
        {
            ConfirmOrder();
            return RedirectToAction("ConfirmPage");
        }
        public static string OrderGroupGUID = "";
        public IActionResult ConfirmOrder()
        {
            //sipariş tablosuna kaydet
            //sepetim cookie sinden sepeti temizle
            //e-fatura olustur metodunu cagır
            var cookieOptions = new CookieOptions();
            var cookie = Request.Cookies["sepetim"];
            if (cookie != null)
            {
                o.MyCart = cookie;
                OrderGroupGUID = o.OrderCreate(HttpContext.Session.GetString("Email").ToString());

                cookieOptions.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Delete("sepetim"); //tarayıcıdan sepeti sil
                                                    //    cls_User.Send_Sms(OrderGroupGUID);
                                                    //   cls_User.Send_Email(OrderGroupGUID);
            }
            return RedirectToAction("ConfirmPage");
        }
        public IActionResult ConfirmPage()
        {
            ViewBag.OrderGroupGUID = OrderGroupGUID;
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(User user)
        {
            string answer = cls_User.MemberControl(user);

            if (answer == "error")
            {
                HttpContext.Session.SetString("Mesaj", "Email/Şifre yanlış girildi");
                TempData["Message"] = "Email/Şifre yanlış girildi";
                return View();
            }
            else if (answer == "admin")
            {
                HttpContext.Session.SetString("Email", answer);
                HttpContext.Session.SetString("Admin", answer);
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                HttpContext.Session.SetString("Email", answer);
                return RedirectToAction("Index");
            }
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (cls_User.loginEmailControl(user) == false)
            {
                bool answer = cls_User.AddUser(user);

                if (answer)
                {
                    TempData["Message"] = "Kaydedildi.";
                    return RedirectToAction("Login");
                }
                TempData["Message"] = "Hata.Tekrar deneyiniz.";
            }
            else
            {
                TempData["Message"] = "Bu Email Zaten mevcut.Başka Deneyiniz.";
            }
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Email");
            HttpContext.Session.Remove("Admin");
            return RedirectToAction("Index");
        }
        public IActionResult MyOrders()
        {
            if (HttpContext.Session.GetString("Email") != null)
            {
                List<vw_MyOrders> orders = o.SelectMyOrders(HttpContext.Session.GetString("Email").ToString());
                return View(orders);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        public IActionResult DetailedSearch()
        {
            ViewBag.Categories = context.Categories.ToList();
            ViewBag.Suppliers = context.Suppliers.ToList();
            return View();
        }
        public IActionResult DProducts(int CategoryID, string[] SupplierID, string price, string IsInStock)
        {
            price = price.Replace(" ", "");
            string[] PriceArray = price.Split('-');
            string startprice = PriceArray[0];
            string endprice = PriceArray[1];

            string sign = ">";
            if (IsInStock == "0")
            {
                sign = ">=";
            }

            int count = 0;
            string suppliervalue = ""; //1,2,4
            for (int i = 0; i < SupplierID.Length; i++)
            {
                if (count == 0)
                {
                    suppliervalue = "SupplierID =" + SupplierID[i];
                    count++;
                }
                else
                {
                    suppliervalue += " or SupplierID =" + SupplierID[i];
                }
            }

            string query = "select * from Products where  CategoryID = " + CategoryID + " and (" + suppliervalue + ") and (UnitPrice > " + startprice + " and UnitPrice < " + endprice + ") and Stock " + sign + " 0 order by ProductName";

            ViewBag.Products = cp.SelectProductsByDetails(query);
            return View();
        }
        public IActionResult PharmacyOnDuty()
        {
           

            string json = new WebClient().DownloadString("https://openapi.izmir.bel.tr/api/ibb/nobetcieczaneler");

            var pharmacy = JsonConvert.DeserializeObject<List<Pharmacy>>(json);

            return View(pharmacy);
        }
        public IActionResult ArtAndCulture()
        {
            

            string json = new WebClient().DownloadString("https://openapi.izmir.bel.tr/api/ibb/kultursanat/etkinlikler");

            var activite = JsonConvert.DeserializeObject<List<Activite>>(json);

            return View(activite);
        }
        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult ContactUs()
        {
            return View();
        }
        public PartialViewResult gettingProducts(string id)
        {
            id = id.ToUpper(new System.Globalization.CultureInfo("tr-TR"));
            List<sp_arama> ulist = cls_Product.gettingSearchProducts(id);
            string json = JsonConvert.SerializeObject(ulist);
            var response = JsonConvert.DeserializeObject<List<Search>>(json);
            return PartialView(response);
        }

       
      
        

    }
}