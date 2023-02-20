using FurnitureStore.Data.Interfaces;
using FurnitureStore.Data.Models;
using FurnitureStore.Data.Repository;
using FurnitureStore.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureStore.Controllers
{
    public class ShopCartController : Controller
    {
        private readonly IRepository<Furniture> _furRep;
        private readonly ShopCart _shopCart;
        private readonly IWebHostEnvironment _environment;

        public ShopCartController(IWebHostEnvironment environment,IRepository<Furniture> furRep,ShopCart shopCart)
        {
            _environment = environment;
            _furRep = furRep;
            _shopCart = shopCart;
        }
        public ViewResult Index()
        {
            ViewData["Path"] = _environment.WebRootPath;
            _shopCart.ListShopItems = _shopCart.GetShopItems();
            var obj = new ShopCartViewModel
            {
                shopCart = _shopCart
            };
            return View(obj);
        }

        public RedirectToActionResult addToCart(int id)
        {
            var item = _furRep.GetAsync(id).Result;
            if (item != null) 
            {
                _shopCart.AddToCart(item);
            }
            return RedirectToAction("Index");
        }

        public RedirectToActionResult Delete(int id)
        {
            _shopCart.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
