using FurnitureStore.Data.Interfaces;
using FurnitureStore.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureStore.Data.Repository
{
    public class FurnitureRepository : IRepository<Furniture>
    {
        private readonly ApplicationContext applicationContext;
        public FurnitureRepository(ApplicationContext applicationContext)
        { 
            this.applicationContext = applicationContext;
        }

        public async Task CreateAsync(Furniture item)
        {
            await applicationContext.Furniture.AddAsync(item);
            await applicationContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            applicationContext.Remove(applicationContext.Furniture.Where(i=>i.FurnitureID==id).FirstOrDefault());
            await applicationContext.SaveChangesAsync();
        }

        public async Task<Furniture> GetAsync(int id)
        {
            return await applicationContext.Furniture.FindAsync(id);
        }

        public Furniture getObjectFurniture(int furnitureID) => applicationContext.Furniture.FirstOrDefault(i => i.FurnitureID == furnitureID);

        public async Task<IEnumerable<Furniture>> ListAsync()
        {
            return await applicationContext.Furniture.Include(c => c.Stock).Include(c => c.FurnitureColor)
                .Include(c => c.FurnitureType).Include(c => c.FurnitureKind).Include(c=>c.Comments).ToListAsync();
        }

        public async Task SaveAsync()
        {
            await applicationContext.SaveChangesAsync();
        }

        public async Task Update(Furniture item)
        {   
            applicationContext.Furniture.Update(item);
            await applicationContext.SaveChangesAsync();
        }

    }
}
