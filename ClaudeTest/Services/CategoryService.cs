using ClaudeTest.Models;
using ClaudeTest.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClaudeTest.Services
{
    /// <summary>Categoryビジネスロジックのサービス実装。</summary>
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        /// <summary>ICategoryRepositoryをDI経由で受け取るコンストラクタ。</summary>
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        /// <summary>全カテゴリを取得する。</summary>
        public async Task<IEnumerable<Category>> getAllCategoriesAsync()
            => await _categoryRepository.getAllAsync();

        /// <summary>Categoryをコンテキストにステージする（保存しない）。</summary>
        public Task addAsync(Category category) => _categoryRepository.addAsync(category);

        /// <summary>カテゴリを新規作成する。名前が重複する場合は例外をスローする。</summary>
        public async Task<Category> createCategoryAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("カテゴリ名は必須です。", nameof(name));

            var existing = await _categoryRepository.getByNameAsync(name.Trim());
            if (existing is not null)
                throw new InvalidOperationException($"カテゴリ名「{name}」は既に存在します。");

            var category = new Category { Name = name.Trim() };
            await _categoryRepository.addAsync(category);
            await _categoryRepository.saveAsync();
            return category;
        }

        /// <summary>カテゴリを削除する。対象が存在しない場合は例外をスローする。</summary>
        public async Task deleteCategoryAsync(int categoryId)
        {
            var category = await _categoryRepository.getByIdAsync(categoryId)
                ?? throw new InvalidOperationException($"Category(Id={categoryId})が見つかりません。");
            _categoryRepository.delete(category);
            await _categoryRepository.saveAsync();
        }
    }
}
