using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClaudeTest.Models
{
    /// <summary>Todoエンティティ。</summary>
    public class Todo
    {
        /// <summary>主キー。</summary>
        [Key]
        public int Id { get; set; }

        /// <summary>タイトル。</summary>
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        /// <summary>完了フラグ。</summary>
        public bool IsCompleted { get; set; }

        /// <summary>作成日時。</summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>紐づくカテゴリのID（外部キー）。</summary>
        public int? CategoryId { get; set; }

        /// <summary>紐づくカテゴリ（ナビゲーションプロパティ）。</summary>
        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; }
    }
}
