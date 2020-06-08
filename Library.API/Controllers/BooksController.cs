using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Library.API.Data.Entities;
using Library.API.DTOs;
using Library.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    /// <summary>
    /// Interacts with the Book Table
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository bookRepository;
        private readonly IMapper mapper;
        private readonly ILoggerService logger;

        public BooksController(IBookRepository bookRepository, IMapper mapper, ILoggerService logger)
        {
            this.bookRepository = bookRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        /// <summary>
        /// Get All Books
        /// </summary>
        /// <returns>A list of Books</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBooks()
        {
            var errLocation = GetControllerAndActionNames();
            try
            {
                var books = await bookRepository.FindAll();
                var response = mapper.Map<IList<BookDTO>>(books);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return ErrorHandler($"{errLocation}: {ex.Message} - {ex.InnerException}");
            }
        }
        
        /// <summary>
        /// Get a Book by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A Book record</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBook(int id)
        {
            var errLocation = GetControllerAndActionNames();

            try
            {
                var book = await bookRepository.FindById(id);
                if (book == null)
                {
                    return NotFound();
                }

                var response = mapper.Map<BookDTO>(book);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return ErrorHandler($"{errLocation} {ex.Message} - {ex.InnerException}");
            }
        }
        
        /// <summary>
        /// Create a Book
        /// </summary>
        /// <param name="book"></param>
        /// <returns>Book Object</returns>
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] BookCreateDTO bookDTO)
        {
            var errLocation = GetControllerAndActionNames();

            try
            {
                if (bookDTO == null)
                {
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var book = mapper.Map<Book>(bookDTO);
                var isSuccess = await bookRepository.Create(book);
                if (!isSuccess)
                {
                    return ErrorHandler($"{errLocation} creation failed");
                }
                return Created("create", new { book });
            }
            catch (Exception ex)
            {
                return ErrorHandler($"{errLocation} {ex.Message} - {ex.InnerException}");
            }
        }
        
        /// <summary>
        /// Update a book
        /// </summary>
        /// <param name="id"></param>
        /// <param name="book"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] BookUpdateDTO bookDTO)
        {
            var errLocation = GetControllerAndActionNames();

            try
            {
                if (id < 1 || bookDTO == null || bookDTO.Id != id)
                {
                    return BadRequest(ModelState);
                }

                var isExists = await bookRepository.IsExists(id);
                if (!isExists)
                {
                    return NotFound();
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var book = mapper.Map<Book>(bookDTO);
                var isSuccess = await bookRepository.Update(book);
                if (!isSuccess)
                {
                    return ErrorHandler($"{errLocation} updation failed");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return ErrorHandler($"{errLocation} {ex.Message} - {ex.InnerException}");
            }
        }

        /// <summary>
        /// Remove a Book
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            var errLocation = GetControllerAndActionNames();

            try
            {
                if (id < 1)
                {
                    return BadRequest();
                }

                var isExists = await bookRepository.IsExists(id);
                if (!isExists)
                {
                    return NotFound();
                }

                var book = await bookRepository.FindById(id);
                var isSuccess = await bookRepository.Delete(book);
                if (!isSuccess)
                {
                    return ErrorHandler("Book delete failed");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return ErrorHandler($"{errLocation} {ex.Message} - {ex.InnerException}");
            }
        }

        private ObjectResult ErrorHandler(string message)
        {
            logger.LogError(message);
            return StatusCode(500, "Something went wrong, Please contact the Administrator");
        }

        private string GetControllerAndActionNames()
        {
            var controller = ControllerContext.ActionDescriptor.ControllerName;
            var action = ControllerContext.ActionDescriptor.ActionName;

            return $"{controller} - {action}";
        }
    }
}
