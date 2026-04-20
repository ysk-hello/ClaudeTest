using System;
using System.Threading.Tasks;

namespace ClaudeTest.Repositories
{
    /// <summary>トランザクション実行を抽象化したインターフェース。任意のRepositoryから利用できる。</summary>
    public interface ITransactionRunner
    {
        /// <summary>渡されたデリゲートをトランザクション内で実行する。失敗時はロールバックする。</summary>
        Task executeInTransactionAsync(Func<Task> action);
    }
}
