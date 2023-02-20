using FurnitureStore.Data.Interfaces;
using FurnitureStore.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureStore.Data.Repository
{
    public class FurnitureColorRepository: IRepository<FurnitureColor>
    {

        private readonly ApplicationContext applicationContext;

        public FurnitureColorRepository(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        public async Task CreateAsync(FurnitureColor item)
        {
            await applicationContext.FurnitureColor.AddAsync(item);
        }

        public Task DeleteAsync(int id)
        {
            applicationContext.FurnitureColor.Remove(applicationContext.FurnitureColor.Where(i => i.ColorID == id).FirstOrDefault());
            applicationContext.SaveChangesAsync();
            return Task.CompletedTask;
        }

        public async Task<FurnitureColor> GetAsync(int id)
        {
            return await applicationContext.FurnitureColor.FindAsync(id);
        }

        public async Task<IEnumerable<FurnitureColor>> ListAsync()
        {
            return await applicationContext.FurnitureColor.ToListAsync();
        }

        public async Task SaveAsync()
        {
            await applicationContext.SaveChangesAsync();
        }

        public Task Update(FurnitureColor item)
        {
            applicationContext.FurnitureColor.Update(item);
            return Task.CompletedTask;
        }
    }
}
