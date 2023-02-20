using System.IO;
using FurnitureStore.Data.Interfaces;
using FurnitureStore.Data.Models;
using FurnitureStore.Models;
using FurnitureStore.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace FurnitureStore.Controllers
{
    public class HomePageController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IRepository<Furniture> _allFurnituries;
        private readonly IRepository<HomePage> _allHomePages;
        private readonly IRepository<FurnitureKind> _furnitureKind;
        private readonly IRepository<FurnitureType> _furnitureType;
        private readonly IRepository<FurnitureColor> _furnitureColor;
        private readonly ShopCart _shopCart;
        private readonly IRepository<Stock> _Stock;

        public HomePageController(IWebHostEnvironment environment, IRepository<HomePage> allHomePages, ShopCart shopCart, IRepository<Furniture> iallFurnituries,
            IRepository<FurnitureKind> ifurnitureKind, IRepository<FurnitureType> ifurnitureType, IRepository<FurnitureColor> ifurnitureColor, IRepository<Stock> _iStock)
        {
            _allFurnituries = iallFurnituries;
            _furnitureKind = ifurnitureKind;
            _furnitureType = ifurnitureType;
            _furnitureColor = ifurnitureColor;
            _shopCart = shopCart;
            _Stock = _iStock;
            _allHomePages = allHomePages;
            _environment = environment;
        }

        [Authorize(Roles = "Администратор, Модератор")]
        public async Task<IActionResult> Create(string error)
        {
            ViewBag.Stock = await _Stock.ListAsync();
            ViewData["Error"] = error;
            ViewBag.Kinds = await _furnitureKind.ListAsync();
            
            return View();
        }

        [Authorize(Roles = "Администратор, Модератор")]
        [HttpPost]
        public async Task<IActionResult> Create(HomePage homePage,IFormFile t1, IFormFile t2, IFormFile t3,IFormFile c1,IFormFile c2,IFormFile c3, int stock1, int stock2, int stock3)
        {

            ViewBag.Stock = await _Stock.ListAsync();
            List<HomePage> pages = (List<HomePage>)await _allHomePages.ListAsync();
            if (pages.Exists(i => i.Name.ToLower() == homePage.Name.ToLower()))
            {
                return RedirectToAction("Create", new { error = "Страница с таким названием уже существует" });
            }
            else
            {
                await _allHomePages.CreateAsync(homePage);
                await _allHomePages.SaveAsync();
                homePage.Stock_1 = await _Stock.GetAsync(stock1);
                homePage.Stock_2 = await _Stock.GetAsync(stock2);
                homePage.Stock_3 = await _Stock.GetAsync(stock3);
                homePage.TitleImage_1 = await PhotoTitle(homePage.HomePageID + "_1", t1);
                homePage.TitleImage_2 = await PhotoTitle(homePage.HomePageID + "_2", t2);
                homePage.TitleImage_3 = await PhotoTitle(homePage.HomePageID + "_3", t3);
                homePage.CircleImage_1 = await PhotoCircle(homePage.HomePageID + "_1", c1);
                homePage.CircleImage_2 = await PhotoCircle(homePage.HomePageID + "_2", c2);
                homePage.CircleImage_3 = await PhotoCircle(homePage.HomePageID + "_3", c3);
                await _allHomePages.Update(homePage);
                await _allHomePages.SaveAsync();
                return RedirectToAction("List");
            }
        }

        [Authorize(Roles = "Администратор, Модератор")]
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.Stock = await _Stock.ListAsync();
            HomePage page = await _allHomePages.GetAsync(id);
            return View(page);
        }

        [Authorize(Roles = "Администратор, Модератор")]
        [HttpPost]
        public async Task<IActionResult> Edit(HomePage page, IFormFile imgT1, IFormFile imgT2, IFormFile imgT3, IFormFile imgC1, IFormFile imgC2, IFormFile imgC3, int stock1, int stock2, int stock3)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine(page.Stock_1);
                ViewBag.Stock = await _Stock.ListAsync();
                page.Stock_1 = await _Stock.GetAsync(stock1);
                page.Stock_2 = await _Stock.GetAsync(stock2);
                page.Stock_3 = await _Stock.GetAsync(stock3);
                page.TitleImage_1 = await Check(imgT1, page.HomePageID, 1, "/img/TitleImage/", 1);
                page.TitleImage_2 = await Check(imgT2, page.HomePageID, 2, "/img/TitleImage/", 1);
                page.TitleImage_3 = await Check(imgT3, page.HomePageID, 3, "/img/TitleImage/", 1);
                page.CircleImage_1 = await Check(imgC1, page.HomePageID, 1, "/img/CircleImage/", 0);
                page.CircleImage_2 = await Check(imgC2, page.HomePageID, 2, "/img/CircleImage/", 0);
                page.CircleImage_3 = await Check(imgC3, page.HomePageID, 3, "/img/CircleImage/", 0);
                await _allHomePages.Update(page);
                return RedirectToAction("List");
            }
            return View(page);
        }

        [Authorize(Roles = "Администратор, Модератор")]
        public async Task<IActionResult> Delete(int id)
        {
            FileInfo fileInf = new FileInfo(_environment.WebRootPath+_allHomePages.GetAsync(id).Result.CircleImage_1);
            if (fileInf.Exists) fileInf.Delete();
            fileInf = new FileInfo(_environment.WebRootPath + _allHomePages.GetAsync(id).Result.CircleImage_2);
            if (fileInf.Exists) fileInf.Delete();
            fileInf = new FileInfo(_environment.WebRootPath + _allHomePages.GetAsync(id).Result.CircleImage_3);
            if (fileInf.Exists) fileInf.Delete();
            fileInf = new FileInfo(_environment.WebRootPath + _allHomePages.GetAsync(id).Result.TitleImage_1);
            if (fileInf.Exists) fileInf.Delete();
            fileInf = new FileInfo(_environment.WebRootPath + _allHomePages.GetAsync(id).Result.TitleImage_2);
            if (fileInf.Exists) fileInf.Delete();
            fileInf = new FileInfo(_environment.WebRootPath + _allHomePages.GetAsync(id).Result.TitleImage_3);
            if (fileInf.Exists) fileInf.Delete();
            await _allHomePages.DeleteAsync(id);
            await _allHomePages.SaveAsync();
            return RedirectToAction("List");
        }


        public async Task<string> Check(IFormFile file, int id, int i,string path,int k)
        {
            if (file != null)
            {
                if(k==1)return await PhotoTitle(id + "_" + i, file);
                else return await PhotoCircle(id + "_" + i, file);
            }
            else if (System.IO.File.Exists(_environment.WebRootPath + path + id + "_" + i + ".jpg"))
            {
                return path + id + "_" + i + ".jpg";
            }
            else return "/img/No Photo/no-photo.jpg";
        }
        public async Task<string> PhotoTitle(string id, IFormFile pic)
        {
            if (pic != null)
            {
                await using (var memoryStream = new MemoryStream())
                {
                    await pic.CopyToAsync(memoryStream);
                    var img = Image.FromStream(memoryStream);
                    using (Bitmap newPic = new Bitmap(1920, 600))
                    {
                        using (Graphics gr = Graphics.FromImage(newPic))
                        {
                            gr.DrawImage(img, 0, 0, 1920, 600);
                            newPic.Save(_environment.WebRootPath + "/img/TitleImage/" + id + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                            memoryStream.Close();
                            newPic.Dispose();
                        }
                    }
                }
                return "/img/TitleImage/" + id + ".jpg";
            }
            else return "/img/No Photo/no-photo.jpg";
        }
        public async Task<string> PhotoCircle(string id, IFormFile pic)
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
                            newPic.Save(_environment.WebRootPath + "/img/CircleImage/" + id + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                            memoryStream.Close();
                            newPic.Dispose();
                        }
                    }
                }
                return "/img/CircleImage/" + id + ".jpg";
            }
            else return "/img/No Photo/no-photo.jpg";
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Items = _shopCart.GetShopItems().Select(i => i.furniture.FurnitureID);
            ViewBag.Kinds = await _furnitureKind.ListAsync();
            ViewBag.Types = await _furnitureType.ListAsync();
            ViewBag.Colors = await _furnitureColor.ListAsync();
            ViewBag.Furniture =  _allFurnituries.ListAsync().Result.Where(i=>i.isFavorite);
            ViewData["Path"] = _environment.WebRootPath;
            HomePage homePage;
            IEnumerable<HomePage> homePages = _allHomePages.ListAsync().Result.Where(i => i.isFavorite);
            if (homePages.Count() == 0)
            {
                 homePage = await _allHomePages.GetAsync(1);
            }
            else
            {
                 homePage = homePages.First();
            }
            return View(homePage);
        }

        [Authorize(Roles = "Администратор, Модератор")]
        public async Task<IActionResult> FavoritePage(int id)
        {
            foreach (var el in _allHomePages.ListAsync().Result.Where(i => i.HomePageID != id))
            {
                el.isFavorite = false;
            }
            await _allHomePages.SaveAsync();
            _allHomePages.GetAsync(id).Result.isFavorite = true;
            await _allHomePages.SaveAsync();
            return RedirectToAction("List");
        }

        public async Task<IActionResult> List()
        {
            IEnumerable<HomePage> pages= await _allHomePages.ListAsync();
            ViewBag.HomePages = new SelectList(pages.OrderByDescending(i=>i.isFavorite), "HomePageID", "Name");
            HomePageListViewModel homePages = new HomePageListViewModel()
            {
                allHomePages = pages
            };
            return View(homePages); 
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
