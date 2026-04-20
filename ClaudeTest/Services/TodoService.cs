using ClaudeTest.Models;
using ClaudeTest.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClaudeTest.Services
{
    /// <summary>Todoビジネスロジックのサービス実装。</summary>
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _todoRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITransactionRunner _transactionRunner;

        /// <summary>各依存をコンストラクタ注入で受け取る。</summary>
        public TodoService(ITodoRepository todoRepository, ICategoryRepository categoryRepository, ITransactionRunner transactionRunner)
        {
            _todoRepository = todoRepository;
            _categoryRepository = categoryRepository;
            _transactionRunner = transactionRunner;
        }

        /// <summary>全Todoを取得する。</summary>
        public async Task<IEnumerable<Todo>> getAllTodosAsync()
            => await _todoRepository.getAllAsync();

        /// <summary>Todoをコンテキストにステージする（保存しない）。</summary>
        public Task addAsync(Todo todo) => _todoRepository.addAsync(todo);

        /// <summary>Todoを新規作成する。タイトルが空の場合は例外をスローする。</summary>
        public async Task<Todo> createTodoAsync(string title, int? categoryId = null)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("タイトルは必須です。", nameof(title));

            var todo = new Todo
            {
                Title = title.Trim(),
                CategoryId = categoryId,
                CreatedAt = DateTime.UtcNow
            };
            await _todoRepository.addAsync(todo);
            await _todoRepository.saveAsync();
            return todo;
        }

        /// <summary>Todoの完了状態を反転する。対象が存在しない場合は例外をスローする。</summary>
        public async Task toggleTodoAsync(int todoId)
        {
            var todo = await _todoRepository.getByIdAsync(todoId)
                ?? throw new InvalidOperationException($"Todo(Id={todoId})が見つかりません。");
            todo.IsCompleted = !todo.IsCompleted;
            _todoRepository.update(todo);
            await _todoRepository.saveAsync();
        }

        /// <summary>Todoを削除する。対象が存在しない場合は例外をスローする。</summary>
        public async Task deleteTodoAsync(int todoId)
        {
            var todo = await _todoRepository.getByIdAsync(todoId)
                ?? throw new InvalidOperationException($"Todo(Id={todoId})が見つかりません。");
            _todoRepository.delete(todo);
            await _todoRepository.saveAsync();
        }

        /// <summary>
        /// ViewModelから渡されたaddCategoryAction・addTodoActionをトランザクション内で順に実行する。
        /// Category保存でIDを確定させてからTodoをステージし、最後にまとめて保存する。
        /// </summary>
        public async Task createTodoWithNewCategoryAsync(Func<Task> addCategoryAction, Func<Task> addTodoAction)
        {
            await _transactionRunner.executeInTransactionAsync(async () =>
            {
                await addCategoryAction();
                await _categoryRepository.saveAsync();  // category.Id を確定
                await addTodoAction();                   // todo.CategoryId は呼び出し元デリゲートで設定済み
                await _todoRepository.saveAsync();
            });
        }
    }
}
