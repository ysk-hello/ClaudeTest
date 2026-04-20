using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClaudeTest.Repositories
{
    /// <summary>汎用Repositoryインターフェース。</summary>
    public interface IRepository<T> where T : class
    {
        /// <summary>IDでエンティティを取得する。</summary>
        Task<T?> getByIdAsync(int id);

        /// <summary>全エンティティを取得する。</summary>
        Task<IEnumerable<T>> getAllAsync();

        /// <summary>エンティティを追加する。</summary>
        Task addAsync(T entity);

        /// <summary>エンティティを更新する。</summary>
        void update(T entity);

        /// <summary>エンティティを削除する。</summary>
        void delete(T entity);

        /// <summary>変更をDBに保存する。</summary>
        Task saveAsync();
    }
}
