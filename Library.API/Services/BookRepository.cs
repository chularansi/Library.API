using Library.API.Data;
using Library.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Services
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext dbContext;

        public BookRepository(LibraryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> Create(Book entity)
        {
            await dbContext.Books.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Book entity)
        {
            dbContext.Books.Remove(entity);
            return await Save();
        }

        public async Task<IList<Book>> FindAll()
        {
            return await dbContext.Books.ToListAsync();
        }

        public async Task<Book> FindById(int id)
        {
            return await dbContext.Books.FindAsync(id);
        }

        public async Task<bool> IsExists(int id)
        {
            return await dbContext.Books.AnyAsync(q => q.Id == id);
        }

        public async Task<bool> Save()
        {
            return await dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> Update(Book entity)
        {
            dbContext.Books.Update(entity);
            return await Save();
        }
    }
}
