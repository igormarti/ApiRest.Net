using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using RestApiWithDontNet.Models;
using RestApiWithDontNet.Business;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestApiWithDontNet.Controllers
{
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/api/books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly ILogger<BookController> _logger;
        private readonly IBookBusiness _bookBusiness;

        public BookController(
            ILogger<BookController> logger,
            IBookBusiness bookBusiness
        ) { 
            _logger = logger;
            _bookBusiness = bookBusiness;
        }

        // GET: api/<BookController>
        [HttpGet]
        public ActionResult<List<Book>> Get()
        {
            List<Book> books = _bookBusiness.FindAll();

            return Ok(books);
        }

        // GET api/<BookController>/5
        [HttpGet("{id}")]
        public ActionResult<Book> Get(int id)
        {
            try
            {
                return _bookBusiness.FindById(id);
            }
            catch (Exception ex) { 
                return NotFound(new {status = false, message = ex.Message});
            }
        }

        // POST api/<BookController>
        [HttpPost]
        public ActionResult<Book> Post([FromBody] Book book)
        {
            if (book == null) return BadRequest();
            Book bookCreated = _bookBusiness.Create(book);
            return CreatedAtAction(nameof(Get), new { id = bookCreated.Id }, bookCreated);
        }

        // PUT api/<BookController>/5
        [HttpPut("{id}")]
        public ActionResult<Book> Put(long id, [FromBody] Book book)
        {
            if (book == null) return BadRequest();
            Book bookUpdated = _bookBusiness.Update(id, book);
            return Ok(bookUpdated);
        }

        // DELETE api/<BookController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                _bookBusiness.Delete(id);
                return NoContent();
            }
            catch (Exception ex) { 
                return NotFound(new {status = false, message= ex.Message});
            }
        }
    }
}
