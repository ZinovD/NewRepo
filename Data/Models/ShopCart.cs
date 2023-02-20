using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureStore.Data.Models
{
    public class ShopCart
    {
        private readonly ApplicationContext applicationContext;
        public ShopCart(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }
        [Key]
        public string ShopCartId { get; set; }
        public List<ShopCartItem> ListShopItems { get; set; }

        public static ShopCart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            var context = services.GetService<ApplicationContext>();
            string shopCartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();
            session.SetString("CartId", shopCartId);
            return new ShopCart(context) { ShopCartId = shopCartId};
        }

        public void AddToCart(Furniture furniture)
        {
            this.applicationContext.ShopCartItem.Add(new ShopCartItem
            {
                ShopCartId=ShopCartId,
                furniture=furniture,
                price= furniture.Price,
                FurnitureID=furniture.FurnitureID
            });
            applicationContext.SaveChanges();
        }

        public List<ShopCartItem> GetShopItems()
        {
            return applicationContext.ShopCartItem.Where(i => i.ShopCartId == ShopCartId).Include(j => j.furniture).ToList();
        }

        public void Delete(int id)
        {
            applicationContext.ShopCartItem.Remove(applicationContext.ShopCartItem.Where(i => i.itemID == id).FirstOrDefault());
            applicationContext.SaveChanges();
        }
    }
}
