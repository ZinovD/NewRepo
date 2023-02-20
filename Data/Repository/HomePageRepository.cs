using FurnitureStore.Data.Interfaces;
using FurnitureStore.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureStore.Data.Repository
{
    public class HomePageRepository : IRepository<HomePage>
    {
        private readonly ApplicationContext applicationContext;

        public HomePageRepository(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }
        public async Task CreateAsync(HomePage item)
        {
            await applicationContext.HomePage.AddAsync(item);
        }

        public async Task DeleteAsync(int id)
        {
            applicationContext.HomePage.Remove(applicationContext.HomePage.Where(i => i.HomePageID == id).FirstOrDefault());
            await applicationContext.SaveChangesAsync();
        }

        public async Task<HomePage> GetAsync(int id)
        {
            return await applicationContext.HomePage.FindAsync(id);
        }

        public async Task<IEnumerable<HomePage>> ListAsync()
        {
            return await applicationContext.HomePage.Include(i=>i.Stock_1).Include(i=>i.Stock_2).Include(i=>i.Stock_3).ToListAsync();
        }

        public async Task SaveAsync()
        {
            await applicationContext.SaveChangesAsync();
        }

        public async Task Update(HomePage item)
        {
            applicationContext.HomePage.Update(item);
            await applicationContext.SaveChangesAsync();
        }
    }
}
