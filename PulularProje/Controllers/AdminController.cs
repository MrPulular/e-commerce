﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PulularProje.Models;
using Microsoft.EntityFrameworkCore;

namespace PulularProje.Controllers
{
    public class AdminController : Controller
    {
        cls_User u = new cls_User();
        cls_Product p = new cls_Product();
        cls_Category c = new cls_Category();
        cls_Supplier s = new cls_Supplier();
        cls_Status st = new cls_Status();
        cls_Setting cs = new cls_Setting();
        public IActionResult Login()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Email,Password,NameSurname")] User user)
        {
            if (ModelState.IsValid)
            {
                User? usr = await u.loginControl(user);
                if (usr != null)
                {
                    // HttpContext.Session.SetString("Email", "deneme");
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ViewBag.error = "Login ve/veya şifre yanlış";
            }
            return View();
        }


        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ProductIndex()
        {
            List<Product> products = await p.ProductSelect();
            return View(products);
        }

        public async Task<IActionResult> CategoryIndex()
        {
            List<Category> categories = await c.CategorySelect();
            return View(categories);
        }

        [HttpGet]
        public IActionResult CategoryCreate()
        {
            CategoryFill();
            return View();
        }

        void CategoryFill()
        {
            List<Category> categories = c.CategorySelectMain();
            ViewData["categoryList"] = categories.Select(c => new SelectListItem { Text = c.CategoryName, Value = c.CategoryID.ToString() });
        }

        async void CategoryFillAll()
        {
            List<Category> categories = await c.CategorySelect();
            ViewData["categoryList"] = categories.Select(c => new SelectListItem { Text = c.CategoryName, Value = c.CategoryID.ToString() });
        }

        async void SupplierFill()
        {
            List<Supplier> suppliers = await s.SupplierSelect();
            ViewData["supplierList"] = suppliers.Select(s => new SelectListItem { Text = s.BrandName, Value = s.SupplierID.ToString() });
        }

        async void StatusFill()
        {
            List<Status> statuses = await st.StatusSelect();
            ViewData["StatusList"] = statuses.Select(s => new SelectListItem { Text = s.StatusName, Value = s.StatusID.ToString() });
        }

        PulularProjeContext context = new PulularProjeContext();

        [HttpPost]
        public IActionResult CategoryCreate(Category category)
        {
            bool answer = cls_Category.CategoryInsert(category);
            if (answer == true)
            {
                TempData["Message"] = "Eklendi";
            }
            else
            {
                TempData["Message"] = "HATA";
            }
            return RedirectToAction(nameof(CategoryCreate));
        }


        public async Task<IActionResult> CategoryEdit(int? id)
        {
            CategoryFill();
            if (id == null || context.Categories == null)
            {
                return NotFound();
            }

            var category = await c.CategoryDetails(id);

            return View(category);
        }

        [HttpPost]
        public IActionResult CategoryEdit(Category category)
        {
            bool answer = cls_Category.CategoryUpdate(category);
            if (answer == true)
            {
               
                TempData["Message"] = "Güncellendi";
                return RedirectToAction("CategoryIndex");
            }
            else
            {
                TempData["Message"] = "HATA";
                return RedirectToAction(nameof(CategoryEdit));
            }
        }



        [HttpGet]
        public async Task<IActionResult> CategoryDelete(int? id)
        {
            if (id == null || context.Categories == null)
            {
                return NotFound();
            }

            var category = await context.Categories.FirstOrDefaultAsync(c => c.CategoryID == id);

            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }


        [HttpPost, ActionName("CategoryDelete")]
        public async Task<IActionResult> CategoryDeleteConfirmed(int id)
        {
          
            bool answer = cls_Category.CategoryDelete(id);
            if (answer == true)
            {
                TempData["Message"] = "Silindi";
                return RedirectToAction("CategoryIndex");
            }
            else
            {
                TempData["Message"] = "HATA";
                return RedirectToAction(nameof(CategoryDelete));
            }
        }



        public async Task<IActionResult> CategoryDetails(int? id)
        {
            var category = await context.Categories.FirstOrDefaultAsync(c => c.CategoryID == id);
            ViewBag.categoryname = category?.CategoryName;

            return View(category);
        }



        public async Task<IActionResult> SupplierIndex()
        {
            List<Supplier> suppliers = await s.SupplierSelect();
            return View(suppliers);
        }

        [HttpGet]
        public IActionResult SupplierCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SupplierCreate(Supplier supplier)
        {
            bool answer = cls_Supplier.SupplierInsert(supplier);
            if (answer == true)
            {
                TempData["Message"] = "Eklendi";
            }
            else
            {
                TempData["Message"] = "HATA";
            }
            return RedirectToAction(nameof(SupplierCreate));
        }




        public async Task<IActionResult> SupplierEdit(int? id)
        {
            if (id == null || context.Suppliers == null)
            {
                return NotFound();
            }

            var supplier = await s.SupplierDetails(id);

            return View(supplier);
        }

        [HttpPost]
        public IActionResult SupplierEdit(Supplier supplier)
        {
            if (supplier.PhotoPath == null)
            {
                string? PhotoPath = context.Suppliers.FirstOrDefault(s => s.SupplierID == supplier.SupplierID).PhotoPath;
                supplier.PhotoPath = PhotoPath;
            }

            bool answer = cls_Supplier.SupplierUpdate(supplier);
            if (answer == true)
            {
                
                TempData["Message"] = "Güncellendi";
                return RedirectToAction("SupplierIndex");
            }
            else
            {
                TempData["Message"] = "HATA";
                return RedirectToAction(nameof(SupplierEdit));
            }
        }




        public async Task<IActionResult> SupplierDetails(int? id)
        {
            var supplier = await context.Suppliers.FirstOrDefaultAsync(c => c.SupplierID == id);
            ViewBag.brandname = supplier?.BrandName;

            return View(supplier);
        }




        [HttpGet]
        public async Task<IActionResult> SupplierDelete(int? id)
        {
            if (id == null || context.Suppliers == null)
            {
                return NotFound();
            }

            var supplier = await context.Suppliers.FirstOrDefaultAsync(c => c.SupplierID == id);

            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }


        [HttpPost, ActionName("SupplierDelete")]
        public async Task<IActionResult> SupplierDeleteConfirmed(int id)
        {
            bool answer = cls_Supplier.SupplierDelete(id);
            if (answer == true)
            {
                TempData["Message"] = "Silindi";
                return RedirectToAction("SupplierIndex");
            }
            else
            {
                TempData["Message"] = "HATA";
                return RedirectToAction(nameof(SupplierDelete));
            }
        }



        public async Task<IActionResult> StatusIndex()
        {
            List<Status> statuses = await st.StatusSelect();
            return View(statuses);
        }


        [HttpGet]
        public IActionResult StatusCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult StatusCreate(Status status)
        {
            bool answer = cls_Status.StatusInsert(status);
            if (answer == true)
            {
                TempData["Message"] = "Eklendi";
            }
            else
            {
                TempData["Message"] = "HATA";
            }
            return RedirectToAction(nameof(StatusCreate));
        }


        public async Task<IActionResult> StatusEdit(int? id)
        {
            if (id == null || context.Statuses == null)
            {
                return NotFound();
            }

            var statuses = await st.StatusDetails(id);

            return View(statuses);
        }

        [HttpPost]
        public IActionResult StatusEdit(Status status)
        {
            bool answer = cls_Status.StatusUpdate(status);
            if (answer == true)
            {
                TempData["Message"] = "Güncellendi";
                return RedirectToAction(nameof(StatusIndex));
            }
            else
            {
                TempData["Message"] = "HATA";
                return RedirectToAction(nameof(StatusEdit));
            }
        }


        [HttpGet]
        public async Task<IActionResult> StatusDelete(int? id)
        {
            if (id == null || context.Statuses == null)
            {
                return NotFound();
            }

            var status = await context.Statuses.FirstOrDefaultAsync(c => c.StatusID == id);

            if (status == null)
            {
                return NotFound();
            }
            return View(status);
        }


        [HttpPost, ActionName("StatusDelete")]
        public async Task<IActionResult> StatusDeleteConfirmed(int id)
        {
            bool answer = cls_Status.StatusDelete(id);
            if (answer == true)
            {
                TempData["Message"] = "Silindi";
                return RedirectToAction(nameof(StatusIndex));
            }
            else
            {
                TempData["Message"] = "HATA";
                return RedirectToAction(nameof(StatusDelete));
            }
        }


        public async Task<IActionResult> StatusDetails(int? id)
        {
            var status = await context.Statuses.FirstOrDefaultAsync(c => c.StatusID == id);
            ViewBag.statusname = status?.StatusName;

            return View(status);
        }


        [HttpGet]
        public async Task<IActionResult> ProductCreate()
        {
            List<Category> categories = await c.CategorySelect();
            ViewData["categoryList"] = categories.Select(c => new SelectListItem { Text = c.CategoryName, Value = c.CategoryID.ToString() });

            List<Supplier> suppliers = await s.SupplierSelect();
            ViewData["supplierList"] = suppliers.Select(s => new SelectListItem { Text = s.BrandName, Value = s.SupplierID.ToString() });

            List<Status> statuses = await st.StatusSelect();
            ViewData["StatusList"] = statuses.Select(s => new SelectListItem { Text = s.StatusName, Value = s.StatusID.ToString() });

            return View();
        }


        [HttpPost]
        public IActionResult ProductCreate(Product product)
        {
            bool answer = cls_Product.ProductInsert(product);
            if (answer == true)
            {
                TempData["Message"] = "Eklendi";
            }
            else
            {
                TempData["Message"] = "HATA";
            }
            return RedirectToAction(nameof(ProductCreate));
        }



        public async Task<IActionResult> ProductEdit(int? id)
        {
            CategoryFill();
            SupplierFill();
            StatusFill();

            if (id == null || context.Products == null)
            {
                return NotFound();
            }

            var product = await p.ProductDetails(id);

            return View(product);
        }

        [HttpPost]
        public IActionResult ProductEdit(Product product)
        {
            
            Product prd = context.Products.FirstOrDefault(s => s.ProductID == product.ProductID);            
            product.AddDate = prd.AddDate;
            product.HighLighted = prd.HighLighted;
            product.TopSeller = prd.TopSeller;

            if (product.PhotoPath == null)
            {
                string? PhotoPath = context.Products.FirstOrDefault(s => s.ProductID == product.ProductID).PhotoPath;
                product.PhotoPath = PhotoPath;
            }

            bool answer = cls_Product.ProductUpdate(product);
            if (answer == true)
            {
                TempData["Message"] = "Güncellendi";
                return RedirectToAction("ProductIndex");
            }
            else
            {
                TempData["Message"] = "HATA";
                return RedirectToAction(nameof(ProductEdit));
            }
        }


        public async Task<IActionResult> ProductDetails(int? id)
        {
            var product = await context.Products.FirstOrDefaultAsync(c => c.ProductID == id);
            ViewBag.productname = product?.ProductName;

            return View(product);
        }


        [HttpGet]
        public async Task<IActionResult> ProductDelete(int? id)
        {
            if (id == null || context.Products == null)
            {
                return NotFound();
            }

            var product = await context.Products.FirstOrDefaultAsync(c => c.ProductID == id);

            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }


        [HttpPost, ActionName("ProductDelete")]
        public async Task<IActionResult> ProductDeleteConfirmed(int id)
        {
            bool answer = cls_Product.ProductDelete(id);
            if (answer == true)
            {
                TempData["Message"] = "Silindi";
                return RedirectToAction("ProductIndex");
            }
            else
            {
                TempData["Message"] = "HATA";
                return RedirectToAction(nameof(ProductDelete));
            }
        }



        public IActionResult SettingEdit()
        {
            var setting = cs.SettingDetails();

            return View(setting);
        }

        [HttpPost]
        public IActionResult SettingEdit(Setting setting)
        {
            bool answer = cls_Setting.SettingUpdate(setting);
            if (answer == true)
            {
                TempData["Message"] = "Güncellendi";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Message"] = "HATA";
                return RedirectToAction(nameof(SettingEdit));
            }
        }

        

    }
}
