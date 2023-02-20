using FurnitureStore.Data.Interfaces;
using FurnitureStore.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureStore.Data.Repository
{
    public class FurnitureKindRepository : IRepository<FurnitureKind>
    {
        private readonly ApplicationContext applicationContext;
        public FurnitureKindRepository(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        public async Task CreateAsync(FurnitureKind item)
        {
            await applicationContext.FurnitureKind.AddAsync(item);
            await applicationContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            applicationContext.FurnitureKind.Remove(applicationContext.FurnitureKind.Where(i => i.KindID == id).FirstOrDefault());
            await applicationContext.SaveChangesAsync();
        }

        public async Task<FurnitureKind> GetAsync(int id)
        {
            return await applicationContext.FurnitureKind.FindAsync(id);
        }

        public async Task<IEnumerable<FurnitureKind>> ListAsync()
        {
            return await applicationContext.FurnitureKind.ToListAsync();
        }
        public async Task SaveAsync()
        {
            await applicationContext.SaveChangesAsync();
        }

        public async Task Update(FurnitureKind item)
        {
            applicationContext.FurnitureKind.Update(item);
            await applicationContext.SaveChangesAsync();
        }
    }
}
