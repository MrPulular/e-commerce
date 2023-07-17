namespace PulularProje.Models
{
    public class cls_Order
    {
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public string MyCart { get; set; } //10=1&20=1&30=4&40=2
        public decimal UnitPrice { get; set; }
        public string ProductName { get; set; }
        public int Kdv { get; set; }
        public int MyProperty { get; set; }
        public string PhotoPath { get; set; }
        public string tckimlik_vergi_no { get; set; }

        PulularProjeContext context = new PulularProjeContext();

        public void EfaturaCreate()
        {
            //digital planet xml dosyası
        }
       
        public bool AddToMyCart(string id)
        {
            bool exists = false;
            if (MyCart == "")
            {
                MyCart = id + "=1";
            }
            else
            {
                string[] MyCartArray = MyCart.Split('&');
                for (int i = 0; i < MyCartArray.Length; i++)
                {
                    string[] MyCartArrayLoop = MyCartArray[i].Split('=');
                    if (MyCartArrayLoop[0] == id)
                    {
                        exists = true;
                    }
                }
                if (exists == false)
                {
                    MyCart = MyCart + "&" + id.ToString() + "=1";
                }
            }
            return exists;
        }

        public List<cls_Order> SelectMyCart()
        {
            
            List<cls_Order> list = new List<cls_Order>();
            string[] MyCartArray = MyCart.Split('&');

            if (MyCartArray[0] != "")
            {
                for (int i = 0; i < MyCartArray.Length; i++)
                {
                    string[] MyCartArrayLoop = MyCartArray[i].Split('=');
                    int MyCartID = Convert.ToInt32(MyCartArrayLoop[0]);

                    Product? prd = context.Products.FirstOrDefault(p => p.ProductID == MyCartID);
                    
                    cls_Order ord = new cls_Order();
                    ord.ProductID = prd.ProductID;
                    ord.Quantity = Convert.ToInt32(MyCartArrayLoop[1]);
                    ord.UnitPrice = prd.UnitPrice;
                    ord.ProductName = prd.ProductName;
                    ord.PhotoPath = prd.PhotoPath;
                    ord.Kdv = prd.Kdv;
                    list.Add(ord);
                }
            }
            return list;
        }
        
        public void DeleteFromMyCart(string id)
        {           
            string[] MyCartArray = MyCart.Split('&');
            string NewMyCart = "";
            int count = 1;

            for (int i = 0; i < MyCartArray.Length; i++)
            {
                
                string[] MyCartArrayLoop = MyCartArray[i].Split('=');                
                string MyCartID = MyCartArrayLoop[0];
                if (MyCartID != id)
                {
                    
                    if (count == 1)
                    {
                        NewMyCart = MyCartArrayLoop[0] + "=" + MyCartArrayLoop[1];
                        count++;
                    }
                    else
                    {
                        NewMyCart += "&" + MyCartArrayLoop[0] + "=" + MyCartArrayLoop[1];
                    }
                }
                
            }
            MyCart = NewMyCart;
        }

        public string OrderCreate(string Email)
        {
            List<cls_Order> sipList = SelectMyCart();
            string OrderGroupGUID = DateTime.Now.ToString().Replace(":", "").Replace(" ", "").Replace(".", "");
            DateTime OrderDate = DateTime.Now; ;

            foreach (var item in sipList)
            {
                Order order = new Order();
                order.OrderDate = OrderDate;
                order.OrderGroupGUID = OrderGroupGUID;
                order.UserID = context.Users.FirstOrDefault(u => u.Email == Email).UserID;
                order.ProductID = item.ProductID;
                order.Quantity = item.Quantity;
                context.Orders.Add(order);
                context.SaveChanges();
            }
            return OrderGroupGUID;
        }

        public List<vw_MyOrders> SelectMyOrders(string Email)
        {
            int UserID = context.Users.FirstOrDefault(u => u.Email == Email).UserID;

            List<vw_MyOrders> myOrders = context.vw_MyOrders.Where(o => o.UserID == UserID).ToList();

            return myOrders;
        }


    }
}

