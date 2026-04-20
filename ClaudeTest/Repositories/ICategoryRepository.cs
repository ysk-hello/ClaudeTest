using ClaudeTest.Models;
using System.Threading.Tasks;

namespace ClaudeTest.Repositories
{
    /// <summary>Category専用Repositoryインターフェース。</summary>
    public interface ICategoryRepository : IRepository<Category>
    {
        /// <summary>名前でカテゴリを検索する。</summary>
        Task<Category?> getByNameAsync(string name);
    }
}
