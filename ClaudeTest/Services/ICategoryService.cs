using ClaudeTest.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClaudeTest.Services
{
    /// <summary>Categoryサービスのインターフェース。</summary>
    public interface ICategoryService
    {
        /// <summary>全カテゴリを取得する。</summary>
        Task<IEnumerable<Category>> getAllCategoriesAsync();

        /// <summary>Categoryをコンテキストにステージする（保存しない）。</summary>
        Task addAsync(Category category);

        /// <summary>カテゴリを新規作成する。</summary>
        Task<Category> createCategoryAsync(string name);

        /// <summary>カテゴリを削除する。</summary>
        Task deleteCategoryAsync(int categoryId);
    }
}
