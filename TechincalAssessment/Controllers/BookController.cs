using Microsoft.AspNetCore.Mvc;
using TechincalAssessment.Models;
using Microsoft.EntityFrameworkCore;
using TechincalAssessment.Interface;
using Microsoft.Extensions.Logging;
using Microsoft.Exchange.WebServices.Data;
using Newtonsoft.Json;

namespace TechincalAssessment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        public readonly ILogger<BookController> _logger;

        public BookController( IBookRepository bookRepository,ILogger<BookController> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;

        }

        /// <summary>
        /// GetAll Books method returns all the date in book table
        ///  searchparam - perform the search operation based on the book title
        ///  sortBy(sortfiled) and sortOrder(asc,desc) - perform the sort operation  those are optional paramanters
        /// </summary>
        [HttpGet("GetAllBooks")]
        public IActionResult GetAllBooks(string? searchParam, string? sortBy, string? sortOrder )
        {
            try
            {
                 _logger.LogInformation("Entry in GetAllBooks");
                 var result = _bookRepository.GetAllBooks(searchParam, sortBy, sortOrder);
                 return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in GetAllBooks ", ex);
                return BadRequest($" Exception : {ex.Message}");
            }
            finally
            {
                _logger.LogInformation("End of GetAllBooks");
            }
        }

        /// <summary>
        /// GetAll Books method returns all the date in book table
        /// insert a single data in book collection
        /// </summary>
        [HttpPost("CreateBook")]
        public async Task<IActionResult> CreateBook([FromBody] BookDto bookDto)
        {
            try
            {
                _logger.LogInformation("Entry in GetAllBooks  Param : " + JsonConvert.SerializeObject(bookDto));
                var result = await _bookRepository.CreateAsync(bookDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($" Exception : {ex.Message}");
                return BadRequest(ex.Message);
            }
            finally
            {
                _logger.LogInformation("End of GetAllBooks");
            }
        }


        /// <summary>
        /// SortdByPublisher method returns the data sorted list of these by Publisher, Author (last, first), then title.
        /// </summary>
        [HttpGet("SortByPublisher")]
        public IActionResult SortByPublisher()
        {
            try
            {
                _logger.LogInformation("Entry in SortByPublisher");
                var result = _bookRepository.SortByPublisher();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in SortByPublisher ", ex);
                return BadRequest($"SortByPublisher Exception : {ex.Message}");
            }
            finally
            {
                _logger.LogInformation("End of SortByPublisher");
            }
        }

        /// <summary>
        /// SortdByAuthor method returns the data sorted list of these by Author (last, first) then title.
        /// </summary>
        [HttpGet("SortByAuthor")]
        public IActionResult SortByAuthor()
        {
            try
            {
                _logger.LogInformation("Entry in SortByAuthor");
                var result = _bookRepository.SortByAuthor();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in SortByAuthor ", ex);
                return BadRequest($"SortByAuthor Exception : {ex.Message}");
            }
            finally
            {
                _logger.LogInformation("End of SortByAuthor");
            }
        }

        /// <summary>
        /// SortdByPublisherUsingSP method using the stored procedure it will returns the data sorted list of these by Publisher, Author (last, first), then title.
        /// </summary>
        [HttpGet("SortByPublisherUsingSP")]
        public IActionResult SortByPublisherUsingSP()
        {
            try
            {
                _logger.LogInformation("Entry in SortByPublisherUsingSP with stored procedure ");
                var result = _bookRepository.SortByPublisherUsingSP();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in SortByPublisherUsingSP with stored procedure  ", ex);
                return BadRequest($"SortByPublisherUsingSP with stored procedure  Exception : {ex.Message}");
            }
            finally
            {
                _logger.LogInformation("End of SortByPublisherUsingSP with stored procedure ");
            }
        }

        /// <summary>
        /// SortdByAuthorUsingSP method using the stored procedure returns the data sorted list of these by Author (last, first) then title.
        /// </summary>
        [HttpGet("SortByAuthorUsingSP")]
        public IActionResult SortByAuthorUsingSP()
        {
            try
            {
                _logger.LogInformation("Entry in SortByAuthorUsingSP with stored procedure ");
                var result = _bookRepository.SortByAuthorUsingSP();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in SortByAuthorUsingSP with stored procedure  ", ex);
                return BadRequest($"SortByAuthorUsingSP with stored procedure  Exception : {ex.Message}");
            }
            finally
            {
                _logger.LogInformation("End of SortByAuthorUsingSP with stored procedure ");
            }
        }

        /// <summary>
        /// GetTotalPrice  method  return the total price of all books in the database.
        /// </summary>
        [HttpGet("GetTotalPrice")]
        public IActionResult GetTotalPrice()
        {
            try
            {
                _logger.LogInformation("Entry in GetTotalPrice");
                var result = _bookRepository.GetTotalPrice();
                return Ok(new { TotalPrice = result });
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in GetTotalPrice ", ex);
                return BadRequest($" Exception : {ex.Message}");
            }
            finally
            {
                _logger.LogInformation("End of GetTotalPrice");
            }
        }

        /// <summary>
        /// BookBulkInsert method insert the bulk data in book table
        /// </summary>
        [HttpPost("BookBulkInsert")]
        public async Task<IActionResult> BookBulkInsert([FromBody] List<BookDto> bookDto)
        {
            try
            {
                _logger.LogInformation("Entry in BookBulkInsert");
                await _bookRepository.CreateRangeAsync(bookDto);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in BookBulkInsert ", ex);
                return BadRequest(ex.Message);
            }
            finally
            {
                _logger.LogInformation("End of BookBulkInsert");
            }
        }


        /// <summary>
        /// GetMlaCitation method retrun the data in Mlacitation format
        /// <param name="id">id is primary key in book table</param>
        /// <param name="Type">Type is MLA, Chicago stype type based on id and type it will retrun the citation style</param>
        /// </summary>
        [HttpGet("{id}/GetCitation")]
        public IActionResult GetCitation(int id, string Type)
        {
            try
            {
                _logger.LogInformation("Entry in GetMlaCitation :  Param id:" +id+" Type : "+Type);
                var result = _bookRepository.GetCitation(id, Type);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in GetMlaCitation ", ex);
                return BadRequest(ex.Message);
            }
            finally
            {
                _logger.LogInformation("End of GetMlaCitation");
            }
        }
    }
}
