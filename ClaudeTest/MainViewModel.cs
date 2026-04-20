using ClaudeTest.Models;
using ClaudeTest.Services;
using ClaudeTest.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClaudeTest
{
    /// <summary>メイン画面のViewModel。</summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ITodoService _todoService;
        private readonly ICategoryService _categoryService;
        private readonly ITodoCategoryService _todoCategoryService;
        private string _newTodoTitle = string.Empty;
        private string _newCategoryName = string.Empty;

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>メッセージボックス表示を要求するイベント。</summary>
        public event EventHandler? ShowMessageRequested;

        /// <summary>画面表示用Todoコレクション。</summary>
        public ObservableCollection<TodoViewModel> Todos { get; } = new();

        /// <summary>入力中のTodoタイトル。</summary>
        public string NewTodoTitle
        {
            get => _newTodoTitle;
            set { _newTodoTitle = value; notifyPropertyChanged(); }
        }

        /// <summary>入力中のカテゴリ名。</summary>
        public string NewCategoryName
        {
            get => _newCategoryName;
            set { _newCategoryName = value; notifyPropertyChanged(); }
        }

        /// <summary>Todo追加コマンド。</summary>
        public ICommand AddTodoCommand { get; }

        /// <summary>メッセージ表示コマンド。</summary>
        public ICommand ShowMessageCommand { get; }

        /// <summary>サービスをDI経由で受け取るコンストラクタ。</summary>
        public MainViewModel(ITodoService todoService, ICategoryService categoryService, ITodoCategoryService todoCategoryService)
        {
            _todoService = todoService;
            _categoryService = categoryService;
            _todoCategoryService = todoCategoryService;
            AddTodoCommand = new RelayCommand(async () => await addTodoAsync());
            ShowMessageCommand = new RelayCommand(requestShowMessage);
            _ = loadTodosAsync();
        }

        /// <summary>
        /// エンティティを生成し、AddデリゲートをServiceへ渡してトランザクション保存する。
        /// 保存後に一覧を再読み込みする。
        /// </summary>
        private async Task addTodoAsync()
        {
            if (string.IsNullOrWhiteSpace(NewTodoTitle) || string.IsNullOrWhiteSpace(NewCategoryName))
                return;

            var todo = new Todo { Title = NewTodoTitle.Trim(), CreatedAt = DateTime.UtcNow };
            var category = new Category { Name = NewCategoryName.Trim() };

            await _todoCategoryService.createTodoWithNewCategoryAsync(
                addCategoryAction: () => _categoryService.addAsync(category),
                addTodoAction: () =>
                {
                    todo.CategoryId = category.Id;  // saveAsync後にcategory.Idが確定しているので安全
                    return _todoService.addAsync(todo);
                }
            );

            NewTodoTitle = string.Empty;
            NewCategoryName = string.Empty;
            await loadTodosAsync();
        }

        /// <summary>DB上の全Todoを読み込んでコレクションに反映する。</summary>
        private async Task loadTodosAsync()
        {
            Todos.Clear();
            var todos = await _todoService.getAllTodosAsync();
            foreach (var todo in todos)
                Todos.Add(new TodoViewModel(todo));
        }

        /// <summary>メッセージ表示イベントを発火する。</summary>
        private void requestShowMessage() =>
            ShowMessageRequested?.Invoke(this, EventArgs.Empty);

        /// <summary>プロパティ変更通知を発火する。</summary>
        private void notifyPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
