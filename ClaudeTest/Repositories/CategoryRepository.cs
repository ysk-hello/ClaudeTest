using ClaudeTest.Data;
using ClaudeTest.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClaudeTest.Repositories
{
    /// <summary>CategoryのRepository実装。</summary>
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        /// <summary>DbContextをDI経由で受け取るコンストラクタ。</summary>
        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>IDでカテゴリを取得する。</summary>
        public async Task<Category?> getByIdAsync(int id)
            => await _context.Categories.Include(c => c.Todos).FirstOrDefaultAsync(c => c.Id == id);

        /// <summary>全カテゴリを取得する。</summary>
        public async Task<IEnumerable<Category>> getAllAsync()
            => await _context.Categories.Include(c => c.Todos).ToListAsync();

        /// <summary>名前でカテゴリを検索する。</summary>
        public async Task<Category?> getByNameAsync(string name)
            => await _context.Categories.FirstOrDefaultAsync(c => c.Name == name);

        /// <summary>カテゴリを追加する。</summary>
        public async Task addAsync(Category entity)
            => await _context.Categories.AddAsync(entity);

        /// <summary>カテゴリを更新する。</summary>
        public void update(Category entity)
            => _context.Categories.Update(entity);

        /// <summary>カテゴリを削除する。</summary>
        public void delete(Category entity)
            => _context.Categories.Remove(entity);

        /// <summary>変更をDBに保存する。</summary>
        public async Task saveAsync()
            => await _context.SaveChangesAsync();
    }
}
