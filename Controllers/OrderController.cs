using FurnitureStore.Data;
using FurnitureStore.Data.Interfaces;
using FurnitureStore.Data.Models;
using FurnitureStore.ViewModels;
using System.Xml.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ClosedXML.Excel;
using OfficeOpenXml.Style;

namespace FurnitureStore.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationContext _applicationContext;
        private readonly IWebHostEnvironment _environment;
        private readonly IRepository<Order> _allOrders;
        private readonly ShopCart shopCart;
        public IEnumerable<Order> orders;

        public OrderController(IWebHostEnvironment environment, ApplicationContext applicationContext, IRepository<Order> allOrders,ShopCart shopCart)
        {
            this._applicationContext = applicationContext;
            this._allOrders = allOrders;
            this.shopCart = shopCart;
            _environment = environment;
        }

        [Authorize(Roles = "Администратор, Модератор")]
        public async Task<IActionResult> OrderList()
        {
            OrderListViewModel ord = new OrderListViewModel
            {
                allOrders = await _allOrders.ListAsync()
            };
            return View(ord);
        }

        [Authorize(Roles = "Администратор, Модератор")]
        [HttpPost]
        public async Task<IActionResult> OrderList(int numberFilter, DateTime dateFilter, DateTime dateFilter2, string fullNameFilter, string statusFilter)
        {
            OrderListViewModel ord = new OrderListViewModel();
            IEnumerable<Order> filter = await _allOrders.ListAsync();
            if (numberFilter!=0)
            {
                ord.allOrders= filter.Where(i=>i.OrderID==numberFilter);
            }
            else
            {
                filter = filter.Where(i => i.OrderTime >= dateFilter);
                if (dateFilter2 != new DateTime(0001, 01, 01, 0, 00, 00))
                {
                    filter = filter.Where(i => i.OrderTime <= dateFilter2);
                }
                if (fullNameFilter!="" && fullNameFilter != null)
                {
                    filter = filter.Where(i => (i.Surname.ToUpper()+" "+i.Name.ToUpper()+" "+i.MiddleName.ToUpper()).Contains(fullNameFilter.ToUpper()));
                }
                if (statusFilter == "Все" || statusFilter == null) { }
                else
                {
                    bool test = statusFilter == "Завершен" ? true : false;
                    filter = filter.Where(i => i.Status == test);
                }
                ord.allOrders= filter;
            }
            ViewBag.Orders = ord.allOrders;
            return View(ord);
        }

        public IActionResult ClientOrderList()
        {
            OrderListViewModel ord = new OrderListViewModel
            {
                allOrders =  _allOrders.ListAsync().Result.Where(c=>c.User==User.Identity.Name)
            };
            return View(ord);
        }

        public IActionResult OrderDetail(int id)
        {
            Order ord = _allOrders.GetAsync(id).Result;
            return View(ord);
        }
        public  IActionResult Create()
        {
            if (!shopCart.GetShopItems().Any())
            {
                return RedirectToAction("CartIsEmpty");
            }
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Authenticated");
            }
            ViewBag.User = _applicationContext.Users.Where(i => i.Email == User.Identity.Name).FirstOrDefault();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            if (ModelState.IsValid)
            {
                order.User = User.Identity.Name;
                order.Status = false;
                await _allOrders.CreateAsync(order);
                return RedirectToAction("Complete") ;
            }
            
           return View(order);
        }

        public async Task<IActionResult> Delete (int OrderID)
        {
            await _allOrders.DeleteAsync(OrderID);
            return RedirectToAction("OrderList");
        }
        public IActionResult Confirmation(int id)
        {
            _allOrders.GetAsync(id).Result.Status = true;
            _allOrders.SaveAsync();
            return RedirectToAction("ClientOrderList");
        }
        public IActionResult Complete()
        {
            ViewBag.Message = "Заказ успешно обработан ";
            return View();
        }
        public IActionResult CartIsEmpty()
        {
            ViewBag.Message = "Корзина пуста";
            return View();
        }

        public IActionResult Authenticated()
        {
            ViewBag.Message = "Необходимо авторизоваться";
            return View();
        }

        public async Task<IActionResult> Export(string test)
        {
            IEnumerable<Order> allorders= await _allOrders.ListAsync();
            List<Order> ordersList=new List<Order>();
            string[] orders= test.Split('_', StringSplitOptions.RemoveEmptyEntries);
            foreach (var el in orders)
            {
                foreach (var order in allorders)
                {
                    if (order.OrderID == Convert.ToInt32(el))
                    {
                        ordersList.Add(order);
                    }
                }
            }
            int i = 1;
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var worksheet = workbook.Worksheets.Add("Отчет");
                worksheet.Cell("A1").Value = "Номер Заказа";
                worksheet.Cell("B1").Value = "Фамилия";
                worksheet.Cell("C1").Value = "Имя";
                worksheet.Cell("D1").Value = "Отчество";
                worksheet.Cell("E1").Value = "Дата и время";
                worksheet.Cell("F1").Value = "Статус";
                worksheet.Cell("G1").Value = "Сумма";
                worksheet.Cell("I1").Value = "Общая сумма заказов";
                worksheet.Cell("J1").Value = "Всего заказов";
                worksheet.Column(1).Width = 18;
                worksheet.Column(2).Width = 18;
                worksheet.Column(3).Width = 18;
                worksheet.Column(4).Width = 18;
                worksheet.Column(5).Width = 18;
                worksheet.Column(6).Width = 18;
                worksheet.Column(7).Width = 18;
                worksheet.Column(9).Width = 23;
                worksheet.Column(10).Width = 23;
                worksheet.Row(1).Style.Font.Bold = true;
                int sum = 0;
                foreach (var order in ordersList)
                {
                    worksheet.Cell(i + 1, 1).Value = order.OrderID;
                    worksheet.Cell(i + 1, 2).Value = order.Surname;
                    worksheet.Cell(i + 1, 3).Value = order.Name;
                    worksheet.Cell(i + 1, 4).Value = order.MiddleName;
                    worksheet.Cell(i + 1, 5).Value = order.OrderTime;
                    string status = "";
                    if (order.Status == false) status = "Доставка";
                    else status = "Завершен";
                    worksheet.Cell(i + 1, 6).Value = status;
                    int price = 0;
                    foreach (var detail in order.OrderDetail)
                    {
                        price += detail.Price;
                    }
                    worksheet.Cell(i + 1, 7).Value = price;
                    i++;
                    sum += price;
                }
                worksheet.Cell(2, 9).Value = sum;
                worksheet.Cell(2, 10).Value = ordersList.Count();
                worksheet.Cells().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"Отчет_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }
    }
}
