using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using RestApiWithDontNet.Models;
using RestApiWithDontNet.Business;
using RestApiWithDontNet.Data.VO;

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
        public ActionResult<List<BookVO>> Get()
        {
            List<BookVO> books = _bookBusiness.FindAll();

            return Ok(books);
        }

        // GET api/<BookController>/5
        [HttpGet("{id}")]
        public ActionResult<BookVO> Get(int id)
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
        public ActionResult<BookVO> Post([FromBody] BookVO book)
        {
            if (book == null) return BadRequest();
            BookVO bookCreated = _bookBusiness.Create(book);
            return CreatedAtAction(nameof(Get), new { CodBook = bookCreated.CodBook }, bookCreated);
        }

        // PUT api/<BookController>/5
        [HttpPut("{id}")]
        public ActionResult<BookVO> Put(long id, [FromBody] BookVO book)
        {
            try
            {
                if (book == null) return BadRequest();
                BookVO bookUpdated = _bookBusiness.Update(id, book);
                return Ok(bookUpdated);
            }
            catch (Exception ex) {
                return NotFound(new {status=false, message=ex.Message});
            }
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
