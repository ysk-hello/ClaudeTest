using ClaudeTest.Data;
using ClaudeTest.Repositories;
using ClaudeTest.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using System;
using System.IO;

namespace ClaudeTest
{
    /// <summary>アプリケーションクラス。DIコンテナの初期化とDB起動を担う。</summary>
    public partial class App : Application
    {
        /// <summary>アプリ全体で使用するDIコンテナ。</summary>
        public static IServiceProvider Services { get; private set; } = null!;

        private Window? _window;

        public App()
        {
            InitializeComponent();
            Services = buildServiceProvider();
        }

        /// <summary>DIコンテナを構築して返す。</summary>
        private static IServiceProvider buildServiceProvider()
        {
            var services = new ServiceCollection();

            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var dbFolder = Path.Combine(localAppData, "ClaudeTest");
            Directory.CreateDirectory(dbFolder);
            var dbPath = Path.Combine(dbFolder, "app.db");

            // DbContext - Scoped（同一スコープ内でインスタンスを共有し、トランザクションを機能させる）
            services.AddDbContext<AppDbContext>(
                options => options.UseSqlite($"Data Source={dbPath}"),
                ServiceLifetime.Scoped);

            services.AddScoped<ITransactionRunner, TransactionRunner>();
            services.AddScoped<ITodoRepository, TodoRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ITodoService, TodoService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddTransient<MainViewModel>();

            return services.BuildServiceProvider();
        }

        /// <summary>アプリ起動時にDBを初期化してウィンドウを生成・表示する。</summary>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            using (var initScope = Services.CreateScope())
            {
                var db = initScope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();
            }

            var scope = Services.CreateScope();
            _window = new MainWindow(scope);
            _window.Activate();
        }
    }
}
