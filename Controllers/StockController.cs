using FurnitureStore.Data;
using FurnitureStore.Data.Interfaces;
using FurnitureStore.Data.Models;
using FurnitureStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace FurnitureStore.Controllers
{
    public class StockController : Controller
    {
        private readonly IRepository<Stock> _allStock;
        private readonly IWebHostEnvironment _environment;

        public async Task<IActionResult> Index()
        {
            StockListViewModel stockView = new StockListViewModel() { allStock = await _allStock.ListAsync() };
            return View(stockView);
        }

        public StockController(IRepository<Stock> allStock, IWebHostEnvironment IWebHostEnvironment)
        {
            _allStock = allStock;
            _environment = IWebHostEnvironment;
        }
        public async Task<IActionResult> StockPage(int StockID)
        {
            return View(await _allStock.GetAsync(StockID));
        }

        public async Task<IActionResult> StockList()
        {
            return View(await _allStock.ListAsync());
        }

        [Authorize(Roles = "Администратор, Модератор")]
        public ActionResult Create(string error)
        {
            ViewData["Error"] = error;
            return View();
        }

        [Authorize(Roles = "Администратор, Модератор")]
        [HttpPost]
        public async Task<ActionResult> Create(Stock stock,IFormFile pic)
        {
            List<Stock> stocks = (List<Stock>)await _allStock.ListAsync();
            if(stocks.Exists(i=>i.StockName.ToLower() == stock.StockName.ToLower()))
            {
                return RedirectToAction("Create",new {error="Акция с таким названием уже существует" });
            }
            await _allStock.CreateAsync(stock);
            await _allStock.SaveAsync();
            stock.Photo= await Photo(stock.StockID.ToString(), pic);
            await _allStock.Update(stock);
            await _allStock.SaveAsync();
            return RedirectToAction("Index");
        }

        public async Task<string> Photo(string stockID, IFormFile pic)
        {
            if (pic != null)
            {
                await using (var memoryStream = new MemoryStream())
                {
                    await pic.CopyToAsync(memoryStream);
                    var img = Image.FromStream(memoryStream);
                    using (Bitmap newPic = new Bitmap(350, 300))
                    {
                        using (Graphics gr = Graphics.FromImage(newPic))
                        {
                            gr.DrawImage(img, 0, 0, 350, 300);
                            newPic.Save(_environment.WebRootPath + "/img/Stock/" + stockID + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                            memoryStream.Close();
                            newPic.Dispose();
                        }
                    }
                }
                return "/img/Stock/" + stockID + ".jpg";
            }
            else return "";
        }


        [Authorize(Roles = "Администратор, Модератор")]
        [HttpGet]
        public async Task<ActionResult> Edit(int id,string error)
        {
            Stock stock = await _allStock.GetAsync(id);
            ViewData["Error"] = error;
            return View(stock);
        }

        [Authorize(Roles = "Администратор, Модератор")]
        [HttpPost]
        public async Task<ActionResult> Edit(Stock stock, IFormFile pic)
        {
            if (pic != null) stock.Photo = await Photo(stock.StockID.ToString(), pic);
            else stock.Photo = "/img/Stock/" + stock.StockID + ".jpg";
            await _allStock.Update(stock);
            await _allStock.SaveAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> List()
        {
            StockListViewModel stockView = new StockListViewModel() { allStock = await _allStock.ListAsync() };
            return View(stockView);
        }
        [Authorize(Roles = "Администратор, Модератор")]
        public async Task<IActionResult> Delete(int id)
        {
            await _allStock.DeleteAsync(id);
            await _allStock.SaveAsync();
            return RedirectToAction("Index");
        }
    }
}
