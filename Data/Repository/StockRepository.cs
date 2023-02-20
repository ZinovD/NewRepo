using FurnitureStore.Data.Interfaces;
using FurnitureStore.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureStore.Data.Repository
{
    public class StockRepository : IRepository<Stock>
    {
        private readonly ApplicationContext applicationContext;
        public StockRepository(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }
        public async Task CreateAsync(Stock item)
        {
            await applicationContext.Stock.AddAsync(item);
        }

        public Task DeleteAsync(int id)
        {
           applicationContext.Stock.Remove(applicationContext.Stock.Where(i => i.StockID == id).FirstOrDefault());
           return Task.CompletedTask;
        }

        public async Task<Stock> GetAsync(int id)
        {
            return await applicationContext.Stock.FindAsync(id);
        }

        public async Task<IEnumerable<Stock>> ListAsync()
        {
            return await applicationContext.Stock.ToListAsync(); ;
        }

        public async Task SaveAsync()
        {
            await applicationContext.SaveChangesAsync();
        }

        public Task Update(Stock item)
        {
            applicationContext.Stock.Update(item);
            return Task.CompletedTask;
        }
    }
}
