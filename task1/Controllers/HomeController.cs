using BLL.Services.OrderListServices;
using BLL.Services.OrderService;
using BLL.Services.ProductServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using task1.Models;

namespace task1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IOrderService _orderService;
        private readonly IProductServices _productService;
        private readonly IOrderListService _orderListService;

        public HomeController(ILogger<HomeController> logger,
            IProductServices productService,
            IOrderListService orderListService,
            IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
            _productService = productService;
            _orderListService = orderListService;
        }

        public async Task<IActionResult> Index(string customer, DateTime? dateFrom, DateTime? dateTo)
        {
            var orders = await _orderService.GetAllAsync();

            ViewBag.TotalOrders = _orderService.GetAllAsync().Result.Count();

            ViewBag.TotalProducts = _productService.GetAllProductsAsync().Result.Count();
            return View(orders);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
