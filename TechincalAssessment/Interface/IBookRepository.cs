using Microsoft.AspNetCore.Mvc;
using TechincalAssessment.Models;

namespace TechincalAssessment.Interface
{
    public interface IBookRepository
    {
        Task<Book> CreateAsync(BookDto bookDto);
        List<Book> GetAllBooks(string? searchParam, string? sortBy, string? sortOrder);
        List<Book> SortByPublisher();
        List<Book> SortByAuthor();
        List<Book> SortByPublisherUsingSP();
        List<Book> SortByAuthorUsingSP();
        Task CreateRangeAsync(List<BookDto> bookDtos);
        decimal GetTotalPrice();
        string GetCitation(int id, string Type);
    }
}
