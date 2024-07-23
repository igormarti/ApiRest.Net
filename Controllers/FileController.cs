using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiWithDontNet.Business;
using RestApiWithDontNet.Data.VO;

namespace RestApiWithDontNet.Controllers
{
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/api/files")]
    [Authorize("Bearer")]
    [ApiController]
    public class FileController : Controller
    {

        private readonly IFileBusiness _fileBusiness;

        public FileController(IFileBusiness fileBusiness)
        {
            _fileBusiness = fileBusiness;
        }

        [HttpGet("downloadFile/{fileName}")]
        [ProducesResponseType(200, Type = typeof(byte[]))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Produces("application/octet-stream")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            byte[] buffer = _fileBusiness.getFile(fileName);

            if (buffer != null)
            {
                HttpContext.Response.ContentType = $"application/{Path.GetExtension(fileName).Replace(".","")}";
                HttpContext.Response.Headers.Append("content-type", buffer?.Length.ToString());
                await HttpContext.Response.Body.WriteAsync(buffer);
            }

            return new ContentResult();
        }

        [HttpPost("uploadFile")]
        [ProducesResponseType(200, Type = typeof(FileDetailVO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Produces("application/json")]
        public async Task<IActionResult> UploadFile(IFormFile file) {
            try
            {
                FileDetailVO fileDetailVO = await _fileBusiness.saveFile(file);
                return Ok(fileDetailVO);
            }catch (Exception ex) { 
                return BadRequest(new {status=false, message=ex.Message});  
            }
        }

        [HttpPost("uploadFiles")]
        [ProducesResponseType(200, Type = typeof(List<FileDetailVO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Produces("application/json")]
        public async Task<IActionResult> UploadFiles(List<IFormFile> files)
        {
            try
            {
                List<FileDetailVO> fileDetailVO = await _fileBusiness.saveFiles(files);
                return Ok(fileDetailVO);
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = false, message = ex.Message });
            }
        }
    }
}
