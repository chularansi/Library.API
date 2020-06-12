using Library.API.Data;
using Library.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Services
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly LibraryDbContext dbContext;

        public AuthorRepository(LibraryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> Create(Author entity)
        {
            await dbContext.Authors.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Author entity)
        {
            dbContext.Authors.Remove(entity);
            return await Save();
        }

        public async Task<IList<Author>> FindAll()
        {
            return await dbContext.Authors.Include(q => q.Books).ToListAsync();
        }

        public async Task<Author> FindById(int id)
        {
            return await dbContext.Authors.Include(q => q.Books).FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<bool> IsExists(int id)
        {
            return await dbContext.Authors.AnyAsync(q => q.Id == id);
        }

        public async Task<bool> Save()
        {
            return await dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> Update(Author entity)
        {
            dbContext.Authors.Update(entity);
            return await Save();
        }
    }
}
