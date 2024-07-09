using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using RestApiWithDontNet.Models;
using RestApiWithDontNet.Business;
using RestApiWithDontNet.Data.VO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestApiWithDontNet.Controllers
{
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/api/users")]
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
        public ActionResult<List<UserVO>> Get()
        {
            List<UserVO> users = _userBusiness.FindAll();

            return Ok(users);
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public ActionResult<UserVO> Get(int id)
        {
            try
            {
                return _userBusiness.FindById(id);
            }
            catch (Exception ex) { 
                return NotFound(new {status = false, message = ex.Message});
            }
        }

        // POST api/<UserController>
        [HttpPost]
        public ActionResult<UserVO> Post([FromBody] UserVO user)
        {
            if (user == null) return BadRequest();
            UserVO userCreated = _userBusiness.Create(user);
            return CreatedAtAction(nameof(Get), new { CodUser = userCreated.CodUser }, userCreated);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public ActionResult<UserVO> Put(long id, [FromBody] UserVO user)
        {
            if (user == null) return BadRequest();
            UserVO userUpdated = _userBusiness.Update(id, user);
            return Ok(userUpdated);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
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
    }
}
