using Etrade.DAL.Abstract;
using Etrade.Entity.Models.Entities;
using Etrade.Entity.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Etrade.Core.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderDAL _orderDAL;
        private readonly IOrderLineDAL _orderLineDAL;
        private readonly IProductDAL _productDAL;

        public OrderController(IOrderDAL orderDAL, IOrderLineDAL orderLineDAL, IProductDAL productDAL)
        {
            _orderDAL = orderDAL;
            _orderLineDAL = orderLineDAL;
            _productDAL = productDAL;
        }

        public IActionResult Index()
        {
            return View(_orderDAL.GetAll());
        }
        public IActionResult Details(int id)
        {
            var order = _orderDAL.Get(id);
            order.OrderLines = _orderLineDAL.GetAll(i => i.OrderId == id);
            foreach (var item in order.OrderLines)
            {
                item.Product = _productDAL.Get(item.ProductId);
            }
            return View(order);
        }
        public IActionResult Delete(int id)
        {
            var order = _orderDAL.Get(id);
            _orderDAL.Delete(order);
            return RedirectToAction("Index");   
        }
        public IActionResult Edit(int id) 
        {
            var order = _orderDAL.Get(id);
            bool orderState = false;
            if (order.OrderState == OrderState.Waiting)
                orderState = false;
            else
                orderState = true;            
            var model = new OrderStateViewModel()
            {
                OrderId = order.Id,
                IsCompleted = orderState
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult Edit(OrderStateViewModel model)
        {
            var order = _orderDAL.Get(model.OrderId);
            if (model.IsCompleted)
                order.OrderState = OrderState.Completed;
            else
                order.OrderState = OrderState.Waiting;
            _orderDAL.Update(order);
            return RedirectToAction("Index");
        }
    }
}
