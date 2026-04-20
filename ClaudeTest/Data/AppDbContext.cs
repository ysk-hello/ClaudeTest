using ClaudeTest.Models;
using Microsoft.EntityFrameworkCore;

namespace ClaudeTest.Data
{
    /// <summary>アプリケーション用EF Core DbContext。</summary>
    public class AppDbContext : DbContext
    {
        /// <summary>Todoテーブル。</summary>
        public DbSet<Todo> Todos { get; set; }

        /// <summary>Categoryテーブル。</summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>DbContextOptions をDI経由で受け取るコンストラクタ。</summary>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        /// <summary>モデルのリレーションを構成する。</summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Todo>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Todos)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
