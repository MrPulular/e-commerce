using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace PulularProje.Models
{
    public class cls_Product
    {
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public string? PhotoPath { get; set; }


        PulularProjeContext context = new PulularProjeContext();
        int subpageCount = 0;

        public async Task<List<Product>> ProductSelect()
        {
            List<Product> products = await context.Products.ToListAsync();
            return products;
        }


        public static bool ProductInsert(Product product)
        {
            try
            {
                
                using (PulularProjeContext context = new PulularProjeContext())
                {
                    product.AddDate = DateTime.Now;
                    context.Add(product);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        public async Task<Product> ProductDetails(int? id)
        {
            Product? product = await context.Products.FindAsync(id);
            return product;
        }


        public static bool ProductUpdate(Product product)
        {
            try
            {
                using (PulularProjeContext context = new PulularProjeContext())
                {
                    context.Update(product);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static bool ProductDelete(int id)
        {
            try
            {
                using (PulularProjeContext context = new PulularProjeContext())
                {
                    Product product = context.Products.FirstOrDefault(c => c.ProductID == id);
                    product.Active = false;
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Product> ProductSelect(string mainPageName, int mainpageCount, string subPageName, int pagenumber)
        {
            subpageCount = context.Settings.FirstOrDefault(s => s.settingID == 1).subpageCount;

            List<Product> products;

            if (mainPageName == "New")
            {
                if (subPageName == "")
                {
                    
                    products = context.Products.OrderByDescending(p => p.AddDate).Take(mainpageCount).ToList();
                }
                else
                {
                    if (pagenumber == 0)
                    {
                        
                        products = context.Products.OrderByDescending(p => p.AddDate).Take(subpageCount).ToList();
                    }
                    else
                    {
                        
                        products = context.Products.OrderByDescending(p => p.AddDate).Skip(pagenumber * subpageCount).Take(subpageCount).ToList();
                    }
                }
            }

            else if (mainPageName == "Special")
            {
                if (subPageName == "")
                {
                     
                    products = context.Products.Where(p => p.StatusID == 2).OrderBy(p => p.ProductName).Take(mainpageCount).ToList();
                }
                else
                {
                    if (pagenumber == 0)
                    {
                        
                        products = context.Products.Where(p => p.StatusID == 2).OrderBy(p => p.ProductName).Take(subpageCount).ToList();
                    }
                    else
                    {
                        
                        products = context.Products.Where(p => p.StatusID == 2).OrderBy(p => p.ProductName).Skip(pagenumber * subpageCount).Take(subpageCount).ToList();
                    }
                }
            }

            else if (mainPageName == "Discounted")
            {
                if (subPageName == "")
                {
                    
                    products = context.Products.OrderByDescending(p => p.Discount).Take(mainpageCount).ToList();
                }
                else
                {
                    if (pagenumber == 0)
                    {
                        
                        products = context.Products.OrderByDescending(p => p.Discount).Take(subpageCount).ToList();
                    }
                    else
                    {
                        
                        products = context.Products.OrderByDescending(p => p.Discount).Skip(pagenumber * subpageCount).Take(subpageCount).ToList();
                    }
                }
            }

            else if (mainPageName == "Highlighted")
            {
                if (subPageName == "")
                {
                    
                    products = context.Products.OrderByDescending(p => p.HighLighted).Take(mainpageCount).ToList();
                }
                else
                {
                    if (pagenumber == 0)
                    {
                        
                        products = context.Products.OrderByDescending(p => p.HighLighted).Take(subpageCount).ToList();
                    }
                    else
                    {
                        
                        products = context.Products.OrderByDescending(p => p.HighLighted).Skip(pagenumber * 4).Take(subpageCount).ToList();
                    }
                }
            }

            else if (mainPageName == "Topseller")
            {
                products = context.Products.OrderByDescending(p => p.TopSeller).Take(mainpageCount).ToList();
            }
            else if (mainPageName == "Slider")
            {
                products = context.Products.Where(p => p.StatusID == 1).Take(mainpageCount).ToList();
            }
            else if (mainPageName == "Star")
            {
                products = context.Products.Where(p => p.StatusID == 3).Take(mainpageCount).ToList();
            }
            else if (mainPageName == "Featured")
            {
                products = context.Products.Where(p => p.StatusID == 4).Take(mainpageCount).ToList();
            }
            else if (mainPageName == "Notable")
            {
                products = context.Products.Where(p => p.StatusID == 5).Take(mainpageCount).ToList();
            }
            else
            {
                products = context.Products.Where(p => p.StatusID == 999).Take(mainpageCount).ToList();
            }
            return products;
        }


        public Product ProductDetails(string mainPageName)
        {
            Product? product = context.Products.FirstOrDefault(p => p.StatusID == 6);
            return product;
        }

        public List<Product> ProductSelectWithCategoryID(int id)
        {
            List<Product> products = context.Products.Where(p => p.CategoryID == id).OrderBy(p => p.ProductName).ToList();
            return products;
        }

        public List<Product> ProductSelectWithSupplierID(int id)
        {
            List<Product> products = context.Products.Where(p => p.SupplierID == id).OrderBy(p => p.ProductName).ToList();
            return products;
        }



        public static void Highlighted_Increase(int id)
        {
            using (PulularProjeContext context = new PulularProjeContext())
            {
                Product? product = context.Products.FirstOrDefault(p => p.ProductID == id);
                product.HighLighted += 1;
                context.Update(product);
                context.SaveChanges();
            }
        }


        public List<cls_Product> SelectProductsByDetails(string query)
        {
            List<cls_Product> products = new List<cls_Product>();
            SqlConnection sqlConnection = connection.ServerConnect;
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                cls_Product product = new cls_Product();
                product.ProductID = Convert.ToInt32(sqlDataReader["ProductID"]);
                product.ProductName = sqlDataReader["ProductName"].ToString();
                product.UnitPrice = Convert.ToDecimal(sqlDataReader["UnitPrice"]);
                product.PhotoPath = sqlDataReader["PhotoPath"].ToString();
                products.Add(product);
            }
            return products;
        }
        public static List<sp_arama> gettingSearchProducts(string id)
        {
            using (PulularProjeContext context = new PulularProjeContext())
            {
                var products = context.sp_Aramas.FromSql($"sp_arama {id}").ToList();
                return products;
            }
        }
    }
}
