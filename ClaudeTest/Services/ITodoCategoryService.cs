using System;
using System.Threading.Tasks;

namespace ClaudeTest.Services
{
    /// <summary>TodoとCategoryをまとめて処理するサービスのインターフェース。</summary>
    public interface ITodoCategoryService
    {
        /// <summary>
        /// 渡されたaddCategoryAction・addTodoActionをトランザクション内で順に実行する。
        /// </summary>
        Task createTodoWithNewCategoryAsync(Func<Task> addCategoryAction, Func<Task> addTodoAction);
    }
}
