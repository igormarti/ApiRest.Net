using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiWithDontNet.Business;
using RestApiWithDontNet.Data.VO;
using RestApiWithDontNet.Hypermedia.Filters;
using RestApiWithDontNet.Hypermedia.Utils;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestApiWithDontNet.Controllers
{
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/api/users")]
    [Authorize("Bearer")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserBusiness _userBusiness;

        public UserController(
            ILogger<UserController> logger,
            IUserBusiness userBusiness
        ) { 
            _logger = logger;
            _userBusiness = userBusiness;
        }

        // GET: api/<UserController>
        [HttpGet]
        [ProducesResponseType( 200 , Type= typeof(List<UserVO>) )]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        [TypeFilter(typeof(HyperMediaFilter))]

        public ActionResult<PagedSearchVO<UserVO>> Get(
            [FromQuery] string sort = "asc",
            [FromQuery] int pageSize = 10,
            [FromQuery] int page = 1,
            [FromQuery] string name = ""

        )
        {
            PagedSearchVO<UserVO> users = _userBusiness.findWithPagedSearch(name, sort, pageSize, page);

            return Ok(users);
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(UserVO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        [TypeFilter(typeof(HyperMediaFilter))]
        public ActionResult<UserVO> Get(int id)
        {
            try
            {
                return Ok(_userBusiness.FindById(id));
            }
            catch (Exception ex) { 
                return NotFound(new {status = false, message = ex.Message});
            }
        }

        // POST api/<UserController>
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(UserVO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        [TypeFilter(typeof(HyperMediaFilter))]

        public ActionResult<UserVO> Post([FromBody] UserVO user)
        {
            if (user == null) return BadRequest();
            UserVO userCreated = _userBusiness.Create(user);
            return CreatedAtAction(nameof(Get), new { CodUser = userCreated.CodUser }, userCreated);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(UserVO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        [TypeFilter(typeof(HyperMediaFilter))]

        public ActionResult<UserVO> Put(long id, [FromBody] UserVO user)
        {
            if (user == null) return BadRequest();
            UserVO userUpdated = _userBusiness.Update(id, user);
            return Ok(userUpdated);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public ActionResult Delete(int id)
        {
            try
            {
                _userBusiness.Delete(id);
                return NoContent();
            }
            catch (Exception ex) { 
                return NotFound(new {status = false, message= ex.Message});
            }
        }

        // DELETE api/<UserController>/5
        [HttpPatch("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public ActionResult Patch(int id)
        {
            try
            {
                _userBusiness.Disable(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = false, message = ex.Message });
            }
        }
    }
}
