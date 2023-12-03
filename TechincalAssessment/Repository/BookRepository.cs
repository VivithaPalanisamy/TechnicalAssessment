using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using TechincalAssessment.Interface;
using TechincalAssessment.Models;

namespace TechincalAssessment.Repository
{
    public class BookRepository : IBookRepository
    {
        public readonly APIDbContext _context;
        public readonly ILogger<BookRepository> _logger;

        public BookRepository(APIDbContext context,ILogger<BookRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<Book> GetAllBooks(string? searchParam, string? sortBy, string? sortOrder)
        {
            try
            {
                _logger.LogInformation("Entry in GetAllBooks");

                IQueryable<Book> query = _context.Books.Include(book => book.Publisher).Include(book => book.Author);

                if (!string.IsNullOrEmpty(searchParam))
                {
                    query = query.Where(book => book.Title.Contains(searchParam) || book.Publisher.Name.Contains(searchParam) || book.Author.FirstName.Contains(searchParam) || book.Author.LastName.Contains(searchParam));
                }
                if (!string.IsNullOrEmpty(sortBy))
                {
                    query = ApplySort(query, sortBy, sortOrder);
                }

                var books = query.ToList();
                return books;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in GetAllBooks Exceptions : ", ex);
                throw ex;
            }
            finally
            {
                _logger.LogInformation("End in GetAllBooks");
            }
        }

        public async Task<Book> CreateAsync(BookDto bookDto)
        {
            try
            {
                _logger.LogInformation("Entry in GetAllBooks");
                int publisherId;
                int authorId;
                if (string.IsNullOrEmpty(bookDto.Title))
                {
                    _logger.LogError("Book title Cannot be Empty");
                    throw new Exception("Book title Cannot be Empty");
                }

                // Check if the publisher exists
                var publisher = _context.Publishers.FirstOrDefault(p => p.Name == bookDto.Publisher.Name);

                // Publisher doesn't exist in publisher table, insert a new record else use the existing Id
                if (publisher == null)
                {
                    var newPublisher = new Publisher { Name = bookDto.Publisher.Name };
                    _context.Publishers.Add(newPublisher);
                    await _context.SaveChangesAsync();
                    publisherId = newPublisher.PublisherId;
                }
                else
                {
                    publisherId = publisher.PublisherId;
                }
                _logger.LogInformation("GetAllBooks publisherId : "+publisherId);

                // Check if the author exists
                var author = _context.Authors.FirstOrDefault(a => a.LastName == bookDto.Author.LastName && a.FirstName == bookDto.Author.FirstName);

                // Author doesn't exist in author table, insert a new record else use the existing Id
                if (author == null)
                {
                    var newAuthor = new Author
                    {
                        LastName = bookDto.Author.LastName,
                        FirstName = bookDto.Author.FirstName
                    };
                    _context.Authors.Add(newAuthor);
                    await _context.SaveChangesAsync();
                    authorId = newAuthor.AuthorId;
                }
                else
                {
                    authorId = author.AuthorId;
                }
                _logger.LogInformation("GetAllBooks authorId : " + authorId);
                // Create a new Book with obtained PublisherId and AuthorId
                var newBook = new Book
                {
                    Title = bookDto.Title,
                    Price = bookDto.Price,
                    PublisherId = publisherId,
                    AuthorId = authorId
                };

                _context.Books.Add(newBook);
                await _context.SaveChangesAsync();
                return newBook;
               
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in Creating Book Exceptions : ", ex);
                throw ex;
            }
            finally
            {
                _logger.LogInformation("End in GetAllBooks");
            }
        }
        private IQueryable<Book> ApplySort(IQueryable<Book> query, string propertyName, string sortDirection)
        {
            if (sortDirection?.ToLower() == "desc")
            {
                return query.OrderByDescending(b => EF.Property<object>(b, propertyName));
            }
            else
            {
                return query.OrderBy(b => EF.Property<object>(b, propertyName));
            }
        }


        public List<Book> SortByPublisher()
        {
            try
            {
                _logger.LogInformation("Entry in SortdByPublisher method");
                var books = _context.Books.Include(b => b.Publisher).Include(b => b.Author)
                                            .OrderBy(b => b.Publisher.Name)
                                            .ThenBy(b => b.Author.LastName)
                                            .ThenBy(b => b.Author.FirstName)
                                            .ThenBy(b => b.Title)
                                            .ToList();

                return books;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in SortdByPublisher Exceptions : ", ex);
                throw ex;
            }
            finally
            {
                _logger.LogInformation("End in SortdByPublisher");
            }
        }

        public List<Book> SortByAuthor()
        {
            try
            {
                _logger.LogInformation("Entry in SortdByAuthor method");
                var books = _context.Books.Include(b => b.Publisher).Include(b => b.Author)
                                            .OrderBy(b => b.Author.LastName)
                                            .ThenBy(b => b.Author.FirstName)
                                            .ThenBy(b => b.Title)
                                            .ToList();

                return books;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in SortdByAuthor Exceptions : ", ex);
                throw ex;
            }
            finally
            {
                _logger.LogInformation("End in SortdByAuthor");
            }
        }

        public List<Book> SortByPublisherUsingSP()
        {
            try
            {
                _logger.LogInformation("Entry in SortdByPublisherUsingSP method");
                var books = _context.Books.FromSqlRaw("EXEC GetBooksSortedByPublisherAuthorTitle").AsEnumerable() 
                                        .Select(b =>
                                        {
                                            b.Publisher = _context.Publishers.Find(b.PublisherId);
                                            b.Author = _context.Authors.Find(b.AuthorId);
                                            return b;
                                        }).ToList();

                return books;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in SortdByPublisherUsingSP Exceptions : ", ex);
                throw ex;
            }
            finally
            {
                _logger.LogInformation("End in SortdByPublisherUsingSP");
            }
        }

        public List<Book> SortByAuthorUsingSP()
        {
            try
            {
                _logger.LogInformation("Entry in SortdByAuthorUsingSP method");
                var books = _context.Books.FromSqlRaw("EXEC GetBooksSortedByAuthorTitle").AsEnumerable()
                                        .Select(b =>
                                        {
                                            b.Publisher = _context.Publishers.Find(b.PublisherId);
                                            b.Author = _context.Authors.Find(b.AuthorId);
                                            return b;
                                        }).ToList();
                return books;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in SortdByAuthorUsingSP Exceptions : ", ex);
                throw ex;
            }
            finally
            {
                _logger.LogInformation("End in SortdByAuthorUsingSP");
            }
        }

        public async Task CreateRangeAsync(List<BookDto> bookDtos)
        {
            try
            {
                _logger.LogInformation("Entry in CreateRangeAsync");

                var booksToInsert = new List<Book>();
                foreach (var bookDto in bookDtos)
                {
                    int publisherId;
                    int authorId;
                    if (string.IsNullOrEmpty(bookDto.Title))
                    {
                        _logger.LogError("Book title Cannot be Empty");
                        throw new Exception("Book title Cannot be Empty");
                    }

                    // Check if the publisher exists
                    var publisher = _context.Publishers.FirstOrDefault(p => p.Name == bookDto.Publisher.Name);

                    // Publisher doesn't exist in publisher table, insert a new record else use the existing Id
                    if (publisher == null)
                    {
                        var newPublisher = new Publisher { Name = bookDto.Publisher.Name };
                        _context.Publishers.Add(newPublisher);
                        await _context.SaveChangesAsync();
                        publisherId = newPublisher.PublisherId;
                    }
                    else
                    {
                        publisherId = publisher.PublisherId;
                    }

                    // Check if the author exists
                    var author = _context.Authors.FirstOrDefault(a => a.LastName == bookDto.Author.LastName && a.FirstName == bookDto.Author.FirstName);

                    // Author doesn't exist in author table, insert a new record else use the existing Id
                    if (author == null)
                    {
                        var newAuthor = new Author
                        {
                            LastName = bookDto.Author.LastName,
                            FirstName = bookDto.Author.FirstName
                        };
                        _context.Authors.Add(newAuthor);
                        await _context.SaveChangesAsync();
                        authorId = newAuthor.AuthorId;
                    }
                    else
                    {
                        authorId = author.AuthorId;
                    }

                    // Create a new Book with obtained PublisherId and AuthorId
                    var newBook = new Book
                    {
                        Title = bookDto.Title,
                        Price = bookDto.Price,
                        PublisherId = publisherId,
                        AuthorId = authorId
                    };

                    booksToInsert.Add(newBook);
                }

                _context.Books.AddRange(booksToInsert);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in CreateRangeAsync. Exception: ", ex);
                throw;
            }
            finally
            {
                _logger.LogInformation("End in CreateRangeAsync");
            }
        }

        public decimal GetTotalPrice()
        {
            try
            {
                _logger.LogInformation("Entry in GetTotalPrice method");
                var totalPrice = _context.Books.Sum(book => book.Price);
                return totalPrice;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in GetTotalPrice Exceptions : ", ex);
                throw ex;
            }
            finally
            {
                _logger.LogInformation("End in GetTotalPrice");
            }
        }

        public string GetCitation(int id,string Type)
        {
            try
            {
                _logger.LogInformation("Entry in GetMlaCitation method");
                string citation = null ;

                var book = _context.Books.Include(b => b.Author).Include(b => b.Publisher).FirstOrDefault(b => b.Id == id);
                if (book == null)
                {
                    return null;
                }
                switch (Type?.ToLower())
                {
                    case "mla":
                        citation = book.MlaCitation;
                        break;
                    case "chicago":
                        citation = book.ChicagoCitation;
                        break;

                    default:
                        return "Invalid citation style. Supported styles: MLA, Chicago.";
                }

                if (citation != null)
                {
                    return citation;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in GetMlaCitation Exceptions : ", ex);
                throw ex;
            }
            finally
            {
                _logger.LogInformation("End in GetMlaCitation");
            }
        }
    }
}
