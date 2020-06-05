using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Library.API.Data.Entities;
using Library.API.DTOs;
using Library.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    /// <summary>
    /// Endpoint used to interact with the Authors in the Liabrary Database
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorRepository authorRepository;
        private readonly ILoggerService logger;
        private readonly IMapper mapper;

        public AuthorsController(IAuthorRepository authorRepository, ILoggerService logger, IMapper mapper)
        {
            this.authorRepository = authorRepository;
            this.logger = logger;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get All Authors
        /// </summary>
        /// <returns>List of Authors</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAuthors()
        {
            var errLocation = GetControllerAndActionNames();

            try
            {
                var authors = await authorRepository.FindAll();
                var response = mapper.Map<IList<AuthorDTO>>(authors);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return ErrorHandler($"{errLocation} {ex.Message} - {ex.InnerException}");
            }
        }

       /// <summary>
       /// Get an Author by Id
       /// </summary>
       /// <param name="id"></param>
       /// <returns>An Author record</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAuthor(int id)
        {
            var errLocation = GetControllerAndActionNames();

            try
            {
                var author = await authorRepository.FindById(id);
                if(author == null)
                {
                    logger.LogWarn($"Author with id: {id} was not found");
                    return NotFound();
                }
                var response = mapper.Map<AuthorDTO>(author);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return ErrorHandler($"{errLocation} {ex.Message} - {ex.InnerException}");
            }
        }

        /// <summary>
        /// Create an author
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] AuthorCreateDTO authorDTO)
        {
            var errLocation = GetControllerAndActionNames();

            try
            {
                if (authorDTO == null)
                {
                    logger.LogWarn($"Empty request was submitted");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    logger.LogWarn($"Author data was incomplete");
                    return BadRequest(ModelState);
                }
                var author = mapper.Map<Author>(authorDTO);
                var isSuccess = await authorRepository.Create(author);
                if (!isSuccess)
                {
                    return ErrorHandler("Author creation failed");
                }
                return Created("create", new { author });
            }
            catch (Exception ex)
            {
                return ErrorHandler($"{errLocation} {ex.Message} - {ex.InnerException}");
            }
        }

        /// <summary>
        /// Update an Author
        /// </summary>
        /// <param name="id"></param>
        /// <param name="author"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] AuthorUpdateDTO authorDTO)
        {
            var errLocation = GetControllerAndActionNames();

            try
            {
                if (id < 1 || authorDTO == null || id != authorDTO.Id)
                {
                    logger.LogWarn($"Author update failed with bad data");
                    return BadRequest(ModelState);
                }

                var isExists = await authorRepository.IsExists(id);
                if (!isExists)
                {
                    return NotFound();
                }

                if (!ModelState.IsValid)
                {
                    logger.LogWarn($"Author data was incomplete");
                    return BadRequest(ModelState);
                }

                var author = mapper.Map<Author>(authorDTO);
                var isSuccess = await authorRepository.Update(author);
                if (!isSuccess)
                {
                    return ErrorHandler("Author updation failed");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return ErrorHandler($"{errLocation} {ex.Message} - {ex.InnerException}");
            }
        }

        /// <summary>
        /// Remove an Author
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
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

                var isExists = await authorRepository.IsExists(id);
                if (!isExists)
                {
                    return NotFound();
                }

                var author = await authorRepository.FindById(id);
                var isSuccess = await authorRepository.Delete(author);
                if (!isSuccess)
                {
                    return ErrorHandler("Author delete failed");
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
