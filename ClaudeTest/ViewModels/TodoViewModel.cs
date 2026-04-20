using ClaudeTest.Models;

namespace ClaudeTest.ViewModels
{
    /// <summary>Todo一覧表示用のViewModelラッパー。</summary>
    public class TodoViewModel
    {
        /// <summary>TodoのID。</summary>
        public int Id { get; }

        /// <summary>タイトル。</summary>
        public string Title { get; }

        /// <summary>カテゴリ名。カテゴリ未設定の場合は空文字。</summary>
        public string CategoryName { get; }

        /// <summary>Todoエンティティからビュー表示用プロパティを生成するコンストラクタ。</summary>
        public TodoViewModel(Todo todo)
        {
            Id = todo.Id;
            Title = todo.Title;
            CategoryName = todo.Category?.Name ?? string.Empty;
        }
    }
}
