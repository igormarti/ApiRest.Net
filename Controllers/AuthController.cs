using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RestApiWithDontNet.Business;
using RestApiWithDontNet.Data.VO;

namespace RestApiWithDontNet.Controllers
{

    [ApiVersion("1")]
    [Route("v{version:apiVersion}/api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly ILoginBusiness _loginBusiness;

        public AuthController(ILogger<AuthController> logger, ILoginBusiness loginBusiness) { 
            _logger = logger;
            _loginBusiness = loginBusiness;
        }

        [HttpPost]
        [Route("signin")]
        public ActionResult SignIn([FromBody] UserAuthVO userAuthVO)
        {

            try
            {
                TokenVO token = _loginBusiness.ValidateCredentials(userAuthVO);
                if (token == null) throw new Exception("Error to try generate token.");

                return CreatedAtAction("v1/books", new { token.Created }, token );
            }
            catch (Exception ex) {
                return BadRequest(new { status = false, message=ex.Message });
            }
        }

        [HttpPost]
        [Route("signup")]
        public ActionResult SignUp([FromBody] UserAuthInputVO userAuthVO)
        {

            try
            {
                UserAuthOutputVO user = _loginBusiness.CreateUserAuth(userAuthVO);
                if (user == null) throw new Exception("Error to try create user.");

                return CreatedAtAction("", new { user.CodUser }, user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = false, message = ex.Message });
            }
        }

        [HttpPost]
        [Route("refresh")]
        public ActionResult Refresh([FromBody] TokenVO tokenVO)
        {

            try
            {
                TokenVO token = _loginBusiness.ValidateCredentials(tokenVO);
                if (token == null) throw new Exception("Error to try generate new token.");

                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = false, message = ex.Message });
            }
        }

        [HttpGet]
        [Authorize("Bearer")]
        [Route("revoke")]
        public ActionResult Revoke()
        {
            var userName = User?.Identity?.Name;
            if (userName==null)
                return BadRequest(new { status = false, message = "User is not authenticated" });

            if (!_loginBusiness.RevokeToken(userName)) 
                return BadRequest(new { status = false, message = "Invalid client request" });
            return NoContent();
        }

    }
}
