using Etrade.Core.Models.Helper;
using Etrade.DAL.Abstract;
using Etrade.Entity.Models.Entities;
using Etrade.Entity.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Etrade.Core.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductDAL _productDAL;
        private readonly ICategoryDAL _categoryDAL;
        private readonly IOrderDAL _orderDAL;

        public CartController(IProductDAL productDAL, IOrderDAL orderDAL)
        {
            _productDAL = productDAL;
            _orderDAL = orderDAL;
        }

        public IActionResult Index()
        {
            var cart = SessionHelper.GetFromJson<List<CartItem>>(HttpContext.Session, "cart");
            if (cart != null)
            {
                ViewBag.Total = cart.Sum(i => i.Product.Price * i.Quantity).ToString("c");
                SessionHelper.Count = cart.Sum(i => i.Quantity);
                //SessionHelper.Count = cart.Count;
            }
            return View(cart);
        }
        [HttpPost]
        public IActionResult Buy(int id, int quantity)
        {
            if (SessionHelper.GetFromJson<List<CartItem>>(HttpContext.Session, "cart") == null)
            {
                var cart = new List<CartItem>();
                cart.Add(new CartItem()
                {
                    Product = _productDAL.Get(id),
                    Quantity = quantity
                });
                SessionHelper.SetAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                var cart = SessionHelper.GetFromJson<List<CartItem>>(HttpContext.Session, "cart");
                int index = isIndex(cart, id);
                if (index == -1)
                {
                    cart.Add(new CartItem()
                    {
                        Product = _productDAL.Get(id),
                        Quantity = quantity
                    });
                }
                else
                    cart[index].Quantity += quantity;
                SessionHelper.SetAsJson(HttpContext.Session, "cart", cart);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult AddToCart(int Id, int Quantity)
        {
            if (SessionHelper.GetFromJson<List<CartItem>>(HttpContext.Session, "cart") == null)
            {
                var cart = new List<CartItem>();
                cart.Add(new CartItem()
                {
                    Product = _productDAL.Get(Id),
                    Quantity = Quantity
                });
                SessionHelper.SetAsJson(HttpContext.Session, "cart", cart);
                SessionHelper.Count = cart.Sum(i => i.Quantity);
            }
            else
            {
                var cart = SessionHelper.GetFromJson<List<CartItem>>(HttpContext.Session, "cart");
                int index = isIndex(cart, Id);
                if (index == -1)
                {
                    cart.Add(new CartItem()
                    {
                        Product = _productDAL.Get(Id),
                        Quantity = Quantity
                    });
                }
                else
                    cart[index].Quantity += Quantity;
                SessionHelper.SetAsJson(HttpContext.Session, "cart", cart);
                SessionHelper.Count = cart.Sum(i => i.Quantity);
            }
            return Ok();
        }
        public IActionResult Remove(int id)
        {
            var cart = SessionHelper.GetFromJson<List<CartItem>>(HttpContext.Session, "cart");
            int index = isIndex(cart, id);
            cart.RemoveAt(index);
            SessionHelper.SetAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Index");
        }
        private int isIndex(List<CartItem> cart, int id)
        {
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.Id.Equals(id))
                    return i;
            }
            return -1;
        }
        public IActionResult CheckOut()
        {
            var cart = SessionHelper.GetFromJson<List<CartItem>>(HttpContext.Session, "cart");
            if (cart == null)
            {
                ModelState.AddModelError("NullCartError", "Sepetinizde ürün yoktur");
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public IActionResult CheckOut(ShippingDetail model)
        {
            var cart = SessionHelper.GetFromJson<List<CartItem>>(HttpContext.Session, "cart");
            if (ModelState.IsValid)
            {
                SaveOrder(cart, model);
                cart.Clear();
                SessionHelper.Count = 0;
                return View("Completed");
            }
            return View(model);
        }

        private void SaveOrder(List<CartItem>? cart, ShippingDetail model)
        {
            var order = new Order(OrderState.Waiting,DateTime.Now,cart.Sum(c=>c.Product.Price*c.Quantity),model.Username,model.AddressTitle,model.Address,model.City);
            order.OrderLines = new List<OrderLine>();
            foreach (var line in cart)
            {
                var orderline = new OrderLine()
                {
                    Quantity = line.Quantity,
                    Price=line.Product.Price,
                    ProductId=line.Product.Id
                };
               order.OrderLines.Add(orderline);
            }
            _orderDAL.Add(order);
        }
    }
}
