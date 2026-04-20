using ClaudeTest.Data;
using ClaudeTest.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaudeTest.Repositories
{
    /// <summary>TodoのRepository実装。</summary>
    public class TodoRepository : ITodoRepository
    {
        private readonly AppDbContext _context;

        /// <summary>DbContextをDI経由で受け取るコンストラクタ。</summary>
        public TodoRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>IDでTodoを取得する。</summary>
        public async Task<Todo?> getByIdAsync(int id)
            => await _context.Todos.Include(t => t.Category).FirstOrDefaultAsync(t => t.Id == id);

        /// <summary>全Todoを取得する。</summary>
        public async Task<IEnumerable<Todo>> getAllAsync()
            => await _context.Todos.Include(t => t.Category).ToListAsync();

        /// <summary>完了状態でフィルタリングしたTodo一覧を取得する。</summary>
        public async Task<IEnumerable<Todo>> getByCompletionStatusAsync(bool isCompleted)
            => await _context.Todos.Where(t => t.IsCompleted == isCompleted).ToListAsync();

        /// <summary>カテゴリIDに紐づくTodo一覧を取得する。</summary>
        public async Task<IEnumerable<Todo>> getByCategoryIdAsync(int categoryId)
            => await _context.Todos.Where(t => t.CategoryId == categoryId).ToListAsync();

        /// <summary>Todoを追加する。</summary>
        public async Task addAsync(Todo entity)
            => await _context.Todos.AddAsync(entity);

        /// <summary>Todoを更新する。</summary>
        public void update(Todo entity)
            => _context.Todos.Update(entity);

        /// <summary>Todoを削除する。</summary>
        public void delete(Todo entity)
            => _context.Todos.Remove(entity);

        /// <summary>変更をDBに保存する。</summary>
        public async Task saveAsync()
            => await _context.SaveChangesAsync();
    }
}
