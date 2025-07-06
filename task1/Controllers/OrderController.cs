using BLL.Dtos.OrderDto;
using BLL.Dtos.OrderListDto;
using BLL.Services.OrderService;
using BLL.Services.ProductServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using DAL.Models;
using BLL.Services.OrderListServices;
using Microsoft.EntityFrameworkCore;


namespace Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IProductServices _productService;
        private readonly IOrderListService _orderListService;
        private readonly UserManager<AppUser> _userManager;

        public OrderController(IOrderService orderService, 
            IProductServices productService ,
            IOrderListService orderListService,
            UserManager<AppUser> userManager)
        {
            _orderService = orderService;
            _productService = productService;
            _orderListService = orderListService;
            _userManager = userManager;

        }

        public async Task<IActionResult> Index(string customer, DateTime? date)
        {
            var orders = await _orderService.GetAllAsync();
            if (!string.IsNullOrEmpty(customer))
                orders = orders.Where(o => o.CustomerName.Contains(customer)).ToList();
            if (date.HasValue)
                orders = orders.Where(o => o.OrderDate.Date == date.Value.Date).ToList();
            
            ViewBag.TotalOrders = orders.Count;

            ViewBag.TotalProducts = _productService.GetAllProductsAsync().Result.Count();
            return View(orders);
        }

        public async Task<IActionResult> Create()
        {
            var products = await _productService.GetAllProductsAsync();
            ViewBag.Products = new SelectList(products, "Id", "Name");
            
            var users = await _userManager.Users.ToListAsync();
            ViewBag.Customers = new SelectList(users, "Id", "UserName");

            return View(new CreateOrderDto
            {
                OrderList = new List<CreateOrderlistDto>()

            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderDto dto)
        {
            if (ModelState.IsValid)
            {
                await _orderService.CreateAsync(dto);
                return RedirectToAction(nameof(Index));
            }

            var products = await _productService.GetAllProductsAsync();
            ViewBag.Products = new SelectList(products, "Id", "Name");

            var users = await _userManager.Users.ToListAsync();
            ViewBag.Customers = new SelectList(users, "Id", "UserName");

            return View(dto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null) return NotFound();

            var products = await _productService.GetAllProductsAsync();
            var users = await _userManager.Users.ToListAsync();

            ViewBag.Products = new SelectList(products, "Id", "Name");
            ViewBag.Users = new SelectList(users, "Id", "UserName");

            var dto = new UpdateOrderDto
            {
                Id = order.Id,
                CustomerName = order.CustomerName,
                OrderDate = order.OrderDate,
                UserId = order.UserId,
                OrderList = order.OrderList.Select(item => new CreateOrderlistDto
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                }).ToList()
            };

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdateOrderDto dto)
        {
            if (id != dto.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                var result = await _orderService.UpdateAsync(dto);
                if (result) return RedirectToAction(nameof(Index));
            }

            var products = await _productService.GetAllProductsAsync();
            var users = await _userManager.Users.ToListAsync();

            ViewBag.Products = new SelectList(products, "Id", "Name");
            ViewBag.Users = new SelectList(users, "Id", "UserName");

            return View(dto);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _orderService.DeleteAsync(id);
            if (result) return RedirectToAction(nameof(Index));
            return NotFound();
        }

        public async Task<IActionResult> AddItems(int orderId)
        {
            var products = await _productService.GetAllProductsAsync();
            ViewBag.Products = new SelectList(products, "Id", "Name");
            ViewBag.OrderId = orderId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddItems(int orderId, List<CreateOrderlistDto> items)
        {
            if (ModelState.IsValid)
            {
                await _orderListService.AddItemsAsync(orderId, items);
                return RedirectToAction(nameof(Index));
            }

            var products = await _productService.GetAllProductsAsync();
            ViewBag.Products = new SelectList(products, "Id", "Name");
            ViewBag.OrderId = orderId;
            return View(items);
        }
    }
}
