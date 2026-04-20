using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClaudeTest.Models
{
    /// <summary>カテゴリエンティティ。</summary>
    public class Category
    {
        /// <summary>主キー。</summary>
        [Key]
        public int Id { get; set; }

        /// <summary>カテゴリ名。</summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>このカテゴリに属するTodoのコレクション。</summary>
        public ICollection<Todo> Todos { get; set; } = new List<Todo>();
    }
}
