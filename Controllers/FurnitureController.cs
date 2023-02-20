using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FurnitureStore.Data.Interfaces;
using FurnitureStore.ViewModels;
using FurnitureStore.Data.Models;
using Microsoft.AspNetCore.Http;
using System.Drawing;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using FurnitureStore.Data;
using Microsoft.AspNetCore.Authorization;

namespace FurnitureStore.Controllers
{
    public class FurnitureController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ApplicationContext _applicationContext;
        private readonly IRepository<Furniture> _allFurnituries;
        private readonly IRepository<Comments> _allComments;
        private readonly IRepository<FurnitureKind> _furnitureKind;
        private readonly IRepository<FurnitureType> _furnitureType;
        private readonly IRepository<FurnitureColor> _furnitureColor;
        private readonly IRepository<Stock> _stock;
        private readonly ShopCart _shopCart;

        public FurnitureController(ApplicationContext applicationContext,IWebHostEnvironment IWebHostEnvironment, ShopCart shopCart, IRepository<Furniture> iallFurnituries, IRepository<FurnitureKind> ifurnitureKind,
            IRepository<FurnitureType> ifurnitureType, IRepository<FurnitureColor> ifurnitureColor, IRepository<Stock> iStock, IRepository<Comments> allComments)
        {
            _applicationContext = applicationContext;
            _environment = IWebHostEnvironment;
            _allComments = allComments;
            _allFurnituries = iallFurnituries;
            _furnitureKind = ifurnitureKind;
            _furnitureType = ifurnitureType;
            _furnitureColor = ifurnitureColor;
            _stock = iStock;
            _shopCart = shopCart;
        }
        public async Task<FurnitureListViewModel> Sort(int furnitureID,string name,string?[] stockFilter,string?[] kindFilter, string?[] typeFilter, string?[] colorFilter, int price1, int price2)
        {
            FurnitureListViewModel fur = new FurnitureListViewModel();
            List<Furniture> filter = new List<Furniture>();
            List<Furniture> filter1 = new List<Furniture>();
            var enumerable = await _allFurnituries.ListAsync();

            if (furnitureID != 0)
            {
                fur.allFurniture = enumerable.Where(i => i.FurnitureID == furnitureID);
                return fur;
            }
            else
            {
                if (kindFilter.Any())
                {
                    foreach (var item in enumerable)
                    {
                        foreach (var el in kindFilter.ToList())
                        {
                            if (item.FurnitureKind.Kind == el) filter.Add(item);
                        }
                    }
                }
                else
                {
                    filter = enumerable.ToList();
                }
                if (typeFilter.Any())
                {
                    foreach (var item in filter)
                    {
                        foreach (var el in typeFilter)
                        {
                            if (item.FurnitureType.Type == el) filter1.Add(item);
                        }
                    }
                }
                else
                {
                    filter1 = filter;
                }
                filter = new List<Furniture>();
                if (colorFilter.Any())
                {
                    foreach (var item in filter1)
                    {
                        foreach (var el in colorFilter)
                        {
                            if (item.FurnitureColor.ColorName == el) filter.Add(item);
                        }
                    }
                }
                else
                {
                    filter = filter1;
                }

                filter1 = new List<Furniture>();
                if (price2 != 0)
                {
                    foreach (var item in filter)
                    {
                        if ((item.Price >= price1) && (item.Price <= price2)) filter1.Add(item);
                    }
                }
                else
                {
                    filter1 = filter;
                }
                filter = new List<Furniture>();
                if (stockFilter.Any())
                {
                    foreach (var item in filter1)
                    {
                        foreach (var el in stockFilter)
                        {
                            if (item.Stock.StockName == el) filter.Add(item);
                        }
                    }
                }
                else
                {
                    filter = filter1;
                }
                if(name!="" && name!=null)
                {
                    filter = filter.Where(i => i.Name.ToLower().Contains(name.ToLower())).ToList();
                }

                fur.allFurniture = filter.Where(i => i.Number > 0);
                return fur;
            }
        }
        public async Task<IActionResult> List()
        {
            ViewData["Path"] = _environment.WebRootPath;
            ViewBag.Items = _shopCart.GetShopItems().Select(i => i.furniture.FurnitureID);
            ViewBag.Kinds = await _furnitureKind.ListAsync();
            ViewBag.Types = await _furnitureType.ListAsync();
            ViewBag.Colors = await _furnitureColor.ListAsync();
            ViewBag.Stock = await _stock.ListAsync();
            var furList=await _allFurnituries.ListAsync();
            FurnitureListViewModel fur = new FurnitureListViewModel
            {
                allFurniture = furList.Where(i => i.Number > 0)
            };
            return View(fur);
        }
        [HttpPost]
        public async Task<IActionResult> List(int furnitureID, string name, string[] stockFilter, string[] kindFilter, string[] typeFilter, string[] colorFilter, int price1, int price2)
        {
            ViewData["Path"] = _environment.WebRootPath;
            ViewBag.Items = _shopCart.GetShopItems().Select(i => i.furniture.FurnitureID);
            ViewBag.Kinds = await _furnitureKind.ListAsync();
            ViewBag.Types = await _furnitureType.ListAsync();
            ViewBag.Colors = await _furnitureColor.ListAsync();
            ViewBag.Stock = await _stock.ListAsync();
            return View(Sort(furnitureID,name,stockFilter,kindFilter, typeFilter, colorFilter, price1, price2).Result);
        }
        public IActionResult Test (string kind)
        {
            Console.WriteLine(kind);
            string[] stockFilter = new string[] { };
            string[] kindFilter = new string[] {kind};
            string[] typeFilter = new string[] { };
            string[] colorFilter = new string[] { };
            int price1=0;
            int price2=0;
            return RedirectToAction("List", new  { stockFilter=stockFilter, kindFilter= kindFilter, typeFilter=typeFilter, colorFilter = colorFilter, price1=price1, price2=price2 });
        }

        [Authorize(Roles = "Администратор, Модератор")]
        public async Task<IActionResult> FurnitureList()
        {
            ViewData["Path"] = _environment.WebRootPath;
            ViewBag.Items = _shopCart.GetShopItems().Select(i => i.furniture.FurnitureID);
            ViewBag.Kinds = await _furnitureKind.ListAsync();
            ViewBag.Types = await _furnitureType.ListAsync();
            ViewBag.Colors = await _furnitureColor.ListAsync();
            ViewBag.Stock = await _stock.ListAsync();
            FurnitureListViewModel fur = new FurnitureListViewModel
            {
                allFurniture = await _allFurnituries.ListAsync()
            };
            return View(fur);
        }

        [Authorize(Roles = "Администратор, Модератор")]
        [HttpPost]
        public async Task<IActionResult> FurnitureList(int furnitureID, string name, string?[] stockFilter, string?[] kindFilter, string?[] typeFilter, string?[] colorFilter, int price1, int price2)
        {
            ViewData["Path"] = _environment.WebRootPath;
            ViewBag.Items = _shopCart.GetShopItems().Select(i => i.furniture.FurnitureID);
            ViewBag.Kinds = await _furnitureKind.ListAsync();
            ViewBag.Types = await _furnitureType.ListAsync();
            ViewBag.Colors = await _furnitureColor.ListAsync();
            ViewBag.Stock = await _stock.ListAsync();
            return View(Sort(furnitureID,name,stockFilter,kindFilter, typeFilter, colorFilter, price1, price2).Result);
            
        }

        [Authorize(Roles = "Администратор, Модератор")]
        public async Task<IActionResult> Create(string error)
        {
            ViewData["Error"] = error;
            ViewBag.Kinds = new SelectList(await _furnitureKind.ListAsync(), "KindID", "Kind");
            ViewBag.Types = new SelectList(await _furnitureType.ListAsync(), "TypeID", "Type");
            ViewBag.Colors = new SelectList(await _furnitureColor.ListAsync(),"ColorID","ColorName");
            ViewBag.Stock = new SelectList(await _stock.ListAsync(), "StockID", "StockName");
            return View();
        }

        [Authorize(Roles = "Администратор, Модератор")]
        [HttpPost]
        public async Task<IActionResult> Create(Furniture fur,IFormFile pic1, IFormFile pic2, IFormFile pic3)
        {
            List<Furniture> furnituries = (List<Furniture>)await _allFurnituries.ListAsync();
            if (furnituries.Exists(i => i.Name.ToLower() == fur.Name.ToLower()))
            {
                return base.RedirectToAction("Create", new { error = "Мебель с таким названием уже существует" });
            }
            await _allFurnituries.CreateAsync(fur);
            fur.Photo1 = await Photo(fur.FurnitureID.ToString() + "_1", pic1);
            fur.Photo2 = await Photo(fur.FurnitureID.ToString() + "_2", pic2);
            fur.Photo3 = await Photo(fur.FurnitureID.ToString() + "_3", pic3);
            fur.FurnitureKind = await _furnitureKind.GetAsync(fur.KindID);
            fur.FurnitureType = await _furnitureType.GetAsync(fur.TypeID);
            fur.FurnitureColor = await _furnitureColor.GetAsync(fur.ColorID);
            fur.Stock =await _stock.GetAsync(fur.StockID);
            if (fur.Stock != null) fur.StockPrice = fur.Price-(fur.Price * fur.Stock.StockSize)/100;
            await _allFurnituries.Update(fur);
            return RedirectToAction("FurnitureList");
        }

        public async Task<string> Photo(string furnitureID, IFormFile pic)
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
                            newPic.Save(_environment.WebRootPath + "/img/FurniturePhoto/" + furnitureID + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                            memoryStream.Close();
                            newPic.Dispose();
                        }
                    }
                }
                return "/img/FurniturePhoto/" + furnitureID + ".jpg";
            }
            else return "";
        }

        [Authorize(Roles = "Администратор, Модератор")]
        public async Task<IActionResult> Delete(int furnitureID)
        {
            FileInfo fileInf = new FileInfo(_environment.WebRootPath + _allFurnituries.GetAsync(furnitureID).Result.Photo1);
            if (fileInf.Exists)fileInf.Delete();
            fileInf = new FileInfo(_environment.WebRootPath + _allFurnituries.GetAsync(furnitureID).Result.Photo2);
            if (fileInf.Exists) fileInf.Delete();
            fileInf = new FileInfo(_environment.WebRootPath + _allFurnituries.GetAsync(furnitureID).Result.Photo3);
            if (fileInf.Exists) fileInf.Delete();
            await _allFurnituries.DeleteAsync(furnitureID);
            await _allFurnituries.SaveAsync();
            return RedirectToAction("FurnitureList");
        }

        public async Task<IActionResult> FurniturePage(int furnitureID)
        {
            ViewBag.Items = _shopCart.GetShopItems().Select(i => i.furniture.FurnitureID);
            var a = await _allFurnituries.ListAsync();
            IEnumerable<Furniture> furniture = a.Where(i => i.FurnitureID == furnitureID);
            var furnitureObj = new FurnitureListViewModel
            {
                allFurniture = furniture
            };
            return View(furnitureObj);
        }
        public async Task<IActionResult> Search(string search)
        {
            ViewData["Path"] = _environment.WebRootPath;
            ViewBag.Items = _shopCart.GetShopItems().Select(i => i.furniture.FurnitureID);
            ViewBag.Kinds = await _furnitureKind.ListAsync();
            ViewBag.Types = await _furnitureType.ListAsync();
            ViewBag.Colors = await _furnitureColor.ListAsync();
            ViewBag.Stock = await _stock.ListAsync();
            IEnumerable<Furniture> furniture = null;
            if (string.IsNullOrEmpty(search))
            {
                furniture = _allFurnituries.ListAsync().Result.OrderBy(i => i.Name);
            }
            else
            {
                furniture = _allFurnituries.ListAsync().Result.Where(i => i.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).OrderBy(i => i.FurnitureID);
            }
            if (furniture == null) furniture = await _allFurnituries.ListAsync();
            FurnitureListViewModel furObj = new FurnitureListViewModel
            {
                allFurniture = furniture.Where(i => i.Number > 0),
            };
            return View(furObj);
        }

        [Authorize(Roles = "Администратор, Модератор")]
        public async Task<IActionResult> Edit(int furnitureID)
        {
            ViewBag.Kinds = new SelectList(await _furnitureKind.ListAsync(), "KindID", "Kind");
            ViewBag.Types = new SelectList(await _furnitureType.ListAsync(), "TypeID", "Type");
            ViewBag.Colors = new SelectList(await _furnitureColor.ListAsync(), "ColorID", "ColorName");
            ViewBag.Stock = new SelectList(await _stock.ListAsync(), "StockID", "StockName");

            Furniture furniture = await _allFurnituries.GetAsync(furnitureID);
            return View(furniture);
        }

        [Authorize(Roles = "Администратор, Модератор")]
        [HttpPost]
        public async Task<IActionResult> Edit(Furniture furniture, IFormFile pict1, IFormFile pic2, IFormFile pic3)
        {
            if (ModelState.IsValid)
            {
                if (pict1 != null) furniture.Photo1 = await Photo(furniture.FurnitureID + "_1", pict1);
                else furniture.Photo1 = "/img/FurniturePhoto/" + furniture.FurnitureID + "_1" + ".jpg";
                if (pic2 != null) furniture.Photo2 = await Photo(furniture.FurnitureID + "_2", pic2);
                else furniture.Photo2 = "/img/FurniturePhoto/" + furniture.FurnitureID + "_2" + ".jpg";
                if (pic3 != null) furniture.Photo3 = await Photo(furniture.FurnitureID + "_3", pic3);
                else furniture.Photo3 = "/img/FurniturePhoto/" + furniture.FurnitureID + "_3" + ".jpg";
                furniture.FurnitureKind = await _furnitureKind.GetAsync(furniture.KindID);
                furniture.FurnitureType = await _furnitureType.GetAsync(furniture.TypeID);
                furniture.FurnitureColor = await _furnitureColor.GetAsync(furniture.ColorID);
                furniture.Stock=await _stock.GetAsync(furniture.StockID);
                if (furniture.Stock != null) furniture.StockPrice = furniture.Price - (furniture.Price * furniture.Stock.StockSize) / 100;
                await _allFurnituries.Update(furniture);
                return RedirectToAction("FurnitureList");
            }
            return View(furniture);
        }

        public IActionResult CreateComment(int FurnitureID)
        {
            ViewBag.Comment = FurnitureID;
            return View();
        }
        public async Task<IActionResult> DeleteComment(int CommentID,int FurnitureID)
        {
            await _allComments.DeleteAsync(CommentID);
            return RedirectToAction("FurniturePage", new { furnitureID = FurnitureID });
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(Comments Com, int FurnitureID, IFormFile pic, IFormFile pic2, IFormFile pic3)
        {
            if (Com.Comment !=null)
            { 
                await _allComments.CreateAsync(Com);
                await _allComments.SaveAsync();
                User user = _applicationContext.Users.FirstOrDefault(i => i.UserName == User.Identity.Name);
                Com.FurnitureID = FurnitureID;
                Com.Date = DateTime.Now;
                Com.Photo = await PhotoComment(Com.CommentID.ToString()+"_1", pic);
                Com.Photo2 = await PhotoComment(Com.CommentID.ToString()+"_2", pic2);
                Com.Photo3 = await PhotoComment(Com.CommentID.ToString()+"_3", pic3);
                Com.IdentityUserName = User.Identity.Name;
                Com.UserName = user.Name;
                Com.Avatar = user.Avatar;
                await _allComments.Update(Com);
            }
            return RedirectToAction("FurniturePage",new { furnitureID=FurnitureID });
        }

        public async Task<string> PhotoComment(string Name, IFormFile pic)
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
                            newPic.Save(_environment.WebRootPath + "/img/Comments/" + Name + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                            memoryStream.Close();
                            newPic.Dispose();
                        }
                    }
                }
                return "/img/Comments/" + Name + ".jpg";
            }
            else return "";
        }
    }
}
