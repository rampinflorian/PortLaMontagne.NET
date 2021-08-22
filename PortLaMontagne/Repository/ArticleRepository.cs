using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PortLaMontagne.Data;
using PortLaMontagne.Models;

namespace PortLaMontagne.Repository
{
    public class ArticleRepository : GenericRepository<Article>
    {
        public ArticleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<Article>> FindActiveAsync(bool editor = false)
        {
            List<Article> articles = null;
            
            if (editor)
            {
                articles = await Context.Articles.Include(a => a.Editor).Where(a => a.IsPublished).ToListAsync();
            }
            else
            {
                articles = await Context.Articles.Where(a => a.IsPublished).ToListAsync();
            }

            return articles;
        }
        
        public async Task<int> FindActiveByQuantityAsync(int count = 1)
        {
            return await Context.Articles.Where(a => a.IsPublished).AsNoTracking().Select(a => a.ArticleId).CountAsync();
        }

        public async Task<Article> FindBySlugWithEditorAsync(string slug)
        {
            return await Context.Articles
                .Include(a => a.Editor)
                .FirstOrDefaultAsync(a => a.Slug == slug);        }
    }
}