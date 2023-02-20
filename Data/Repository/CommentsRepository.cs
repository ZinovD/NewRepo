using FurnitureStore.Data.Interfaces;
using FurnitureStore.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureStore.Data.Repository
{
    public class CommentsRepository : IRepository<Comments>
    {

        private readonly ApplicationContext applicationContext;

        public CommentsRepository(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        public async Task CreateAsync(Comments item)
        {
           await applicationContext.Comments.AddAsync(item);
        }

        public async Task DeleteAsync(int id)
        {
            applicationContext.Comments.Remove(applicationContext.Comments.Where(i => i.CommentID == id).FirstOrDefault());
            await applicationContext.SaveChangesAsync();
        }

        public async Task<Comments> GetAsync(int id)
        {
            return await applicationContext.Comments.FindAsync(id);
        }

        public async Task<IEnumerable<Comments>> ListAsync()
        {
           return await applicationContext.Comments.ToListAsync();
        }

        public async Task SaveAsync()
        {
            await applicationContext.SaveChangesAsync();
        }

        public async Task Update(Comments item)
        {
            applicationContext.Comments.Update(item);
            await applicationContext.SaveChangesAsync();
        }
    }
}
