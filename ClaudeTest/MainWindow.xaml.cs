using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace ClaudeTest
{
    /// <summary>メイン画面のView。</summary>
    public sealed partial class MainWindow : Window
    {
        private readonly IServiceScope _scope;

        /// <summary>バインド用ViewModel。</summary>
        public MainViewModel ViewModel { get; }

        /// <summary>スコープを受け取り、ViewModelをDIから解決するコンストラクタ。</summary>
        public MainWindow(IServiceScope scope)
        {
            _scope = scope;
            ViewModel = _scope.ServiceProvider.GetRequiredService<MainViewModel>();
            InitializeComponent();
            ViewModel.ShowMessageRequested += onShowMessageRequested;
            Closed += onWindowClosed;
        }

        /// <summary>メッセージボックスを表示する。</summary>
        private async void onShowMessageRequested(object? sender, EventArgs e)
        {
            var dialog = new ContentDialog
            {
                Title = "メッセージ",
                Content = "ボタンが押されました。",
                CloseButtonText = "OK",
                XamlRoot = Content.XamlRoot
            };
            await dialog.ShowAsync();
        }

        /// <summary>ウィンドウクローズ時にScopedサービスを破棄する。</summary>
        private void onWindowClosed(object sender, WindowEventArgs args)
        {
            _scope.Dispose();
        }
    }
}
