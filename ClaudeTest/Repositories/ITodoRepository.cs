using ClaudeTest.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClaudeTest.Repositories
{
    /// <summary>Todo専用Repositoryインターフェース。</summary>
    public interface ITodoRepository : IRepository<Todo>
    {
        /// <summary>完了状態でフィルタリングしたTodo一覧を取得する。</summary>
        Task<IEnumerable<Todo>> getByCompletionStatusAsync(bool isCompleted);

        /// <summary>カテゴリIDに紐づくTodo一覧を取得する。</summary>
        Task<IEnumerable<Todo>> getByCategoryIdAsync(int categoryId);
    }
}
