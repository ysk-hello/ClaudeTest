using ClaudeTest.Repositories;
using System;
using System.Threading.Tasks;

namespace ClaudeTest.Services
{
    /// <summary>TodoとCategoryをまとめてトランザクション処理するサービス実装。</summary>
    public class TodoCategoryService : ITodoCategoryService
    {
        private readonly ITodoRepository _todoRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITransactionRunner _transactionRunner;

        /// <summary>各依存をコンストラクタ注入で受け取る。</summary>
        public TodoCategoryService(
            ITodoRepository todoRepository,
            ICategoryRepository categoryRepository,
            ITransactionRunner transactionRunner)
        {
            _todoRepository = todoRepository;
            _categoryRepository = categoryRepository;
            _transactionRunner = transactionRunner;
        }

        /// <summary>
        /// addCategoryAction・addTodoActionをトランザクション内で順に実行する。
        /// Category保存でIDを確定させてからaddTodoActionを呼び出す。
        /// </summary>
        public async Task createTodoWithNewCategoryAsync(Func<Task> addCategoryAction, Func<Task> addTodoAction)
        {
            await _transactionRunner.executeInTransactionAsync(async () =>
            {
                await addCategoryAction();
                await _categoryRepository.saveAsync();  // category.Id を確定
                await addTodoAction();                   // todo.CategoryId は呼び出し元で設定済み
                await _todoRepository.saveAsync();
            });
        }
    }
}
