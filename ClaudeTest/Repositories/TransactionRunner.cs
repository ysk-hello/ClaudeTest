using ClaudeTest.Data;
using System;
using System.Threading.Tasks;

namespace ClaudeTest.Repositories
{
    /// <summary>DbContextを使ったトランザクション実行の実装。</summary>
    public class TransactionRunner : ITransactionRunner
    {
        private readonly AppDbContext _context;

        /// <summary>DbContextをDI経由で受け取るコンストラクタ。</summary>
        public TransactionRunner(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>渡されたデリゲートをトランザクション内で実行する。失敗時はロールバックする。</summary>
        public async Task executeInTransactionAsync(Func<Task> action)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await action();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
