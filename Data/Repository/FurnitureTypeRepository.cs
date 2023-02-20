using FurnitureStore.Data.Interfaces;
using FurnitureStore.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureStore.Data.Repository
{
    public class FurnitureTypeRepository: IRepository<FurnitureType>
    {
        private readonly ApplicationContext applicationContext;
        public FurnitureTypeRepository(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }
        public IEnumerable<FurnitureType> AllType => applicationContext.FurnitureType;

        public async Task CreateAsync(FurnitureType item)
        {
            await applicationContext.FurnitureType.AddAsync(item);
        }
        public Task DeleteAsync(int id)
        {
            applicationContext.FurnitureType.Remove(applicationContext.FurnitureType.Where(i => i.TypeID == id).FirstOrDefault());
            return Task.CompletedTask;
        }

        public async Task<FurnitureType> GetAsync(int id)
        {
            return await applicationContext.FurnitureType.FindAsync(id);
        }

        public async Task<IEnumerable<FurnitureType>> ListAsync()
        {
            return await applicationContext.FurnitureType.ToListAsync();
        }
        public async Task SaveAsync()
        {
            await applicationContext.SaveChangesAsync();
        }

        public Task Update(FurnitureType item)
        {
            applicationContext.FurnitureType.Update(item);
            return Task.CompletedTask;
        }
    }
}
