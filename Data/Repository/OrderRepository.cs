using FurnitureStore.Data.Interfaces;
using FurnitureStore.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureStore.Data.Repository
{
    public class OrderRepository : IRepository<Order>
    {
        private readonly ApplicationContext applicationContext;
        private readonly ShopCart shopCart;

        public OrderRepository(ApplicationContext applicationContext,ShopCart shopCart)
        {
            this.applicationContext = applicationContext;
            this.shopCart = shopCart;
        }
        public async Task CreateAsync(Order item)
        {
            item.OrderTime= DateTime.Now;
            applicationContext.Order.Add(item);
            applicationContext.SaveChanges();
            var items = shopCart.GetShopItems();
            foreach (var el in items)
            {
                var orderDetail = new OrderDetail()
                {
                    OrderID = item.OrderID,
                    FurnitureID = el.furniture.FurnitureID,
                    Price = el.furniture.Price,
                };
                applicationContext.Furniture.Where(i => i.FurnitureID == el.FurnitureID).FirstOrDefault().Number--;
                await applicationContext.OrderDetail.AddAsync(orderDetail);
                await applicationContext.SaveChangesAsync();
                item.OrderDetail.Add(orderDetail);
            }
            applicationContext.Order.Update(item);
            applicationContext.SaveChanges();
        }
        public async Task DeleteAsync(int id)
        {
            applicationContext.Remove(applicationContext.Order.Where(i => i.OrderID == id).FirstOrDefault());
            await applicationContext.SaveChangesAsync();
        }

        public async Task<Order> GetAsync(int id)
        {
            var order = await applicationContext.Order.Where(i=>i.OrderID==id).Include(c => c.OrderDetail)
                .ThenInclude(i => i.Furniture).ThenInclude(c => c.FurnitureType)
                .Include(c => c.OrderDetail).ThenInclude(i => i.Furniture).ThenInclude(c => c.FurnitureKind)
                .Include(c => c.OrderDetail).ThenInclude(i => i.Furniture).ThenInclude(c => c.FurnitureColor)
                .Include(c=>c.OrderDetail).ThenInclude(i => i.Furniture).ThenInclude(c=>c.Stock).ToListAsync();
            return order.FirstOrDefault();
        }
        public async Task<IEnumerable<Order>> ListAsync()
        {
            return await applicationContext.Order.Include(c => c.OrderDetail)
                .ThenInclude(i =>i.Furniture).ThenInclude(c=>c.FurnitureType)
                .Include(c => c.OrderDetail).ThenInclude(i => i.Furniture).ThenInclude(c => c.FurnitureKind)
                .Include(c => c.OrderDetail).ThenInclude(i => i.Furniture).ThenInclude(c => c.FurnitureColor)
                .Include(c => c.OrderDetail).ThenInclude(i => i.Furniture).ThenInclude(c => c.Stock)
                .ToListAsync();
        }
        public async Task SaveAsync()
        {
            await applicationContext.SaveChangesAsync();
        }

        public Task Update(Order item)
        {
            throw new NotImplementedException();
        }
    }
}
