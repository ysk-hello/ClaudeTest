using System;
using System.Windows.Input;

namespace ClaudeTest
{
    /// <summary>ICommandの汎用実装。</summary>
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;

        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Action execute) => _execute = execute;

        /// <summary>コマンドが実行可能かどうかを返す。</summary>
        public bool CanExecute(object? parameter) => true;

        /// <summary>コマンドを実行する。</summary>
        public void Execute(object? parameter) => _execute();
    }
}
