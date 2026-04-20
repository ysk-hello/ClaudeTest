using ClaudeTest.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClaudeTest.Services
{
    /// <summary>Todoサービスのインターフェース。</summary>
    public interface ITodoService
    {
        /// <summary>全Todoを取得する。</summary>
        Task<IEnumerable<Todo>> getAllTodosAsync();

        /// <summary>Todoを新規作成する。</summary>
        Task<Todo> createTodoAsync(string title, int? categoryId = null);

        /// <summary>Todoの完了状態を切り替える。</summary>
        Task toggleTodoAsync(int todoId);

        /// <summary>Todoを削除する。</summary>
        Task deleteTodoAsync(int todoId);

        /// <summary>Todoをコンテキストにステージする（保存しない）。</summary>
        Task addAsync(Todo todo);

        /// <summary>
        /// addCategoryAction・addTodoAction をトランザクション内で順に実行し、TodoとCategoryを同時保存する。
        /// </summary>
        Task createTodoWithNewCategoryAsync(Func<Task> addCategoryAction, Func<Task> addTodoAction);
    }
}
