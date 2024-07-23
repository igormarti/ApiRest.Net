using RestApiWithDontNet.Data.VO;

namespace RestApiWithDontNet.Business.Impl
{
    public class FileBusiness : IFileBusiness
    {
        private readonly string _basePath;
        private readonly IHttpContextAccessor _context;
        private readonly string[] _filesAllowed = [".pdf",".jpg",".png",".jpeg"];

        public FileBusiness(IHttpContextAccessor context)
        {
            _context = context;
            _basePath = Directory.GetCurrentDirectory() + "\\UploadDir\\";
        }

        public byte[] getFile(string filename)
        {
            var filePath = _basePath+ filename;
            return File.ReadAllBytes(filePath);
        }

        public async Task<FileDetailVO> saveFile(IFormFile formFile)
        {
            if(formFile == null || formFile.Length < 1) throw new Exception("File invalid");

            FileDetailVO file = new FileDetailVO();
            var fileType = Path.GetExtension(formFile.FileName);
            var scheme = _context?.HttpContext?.Request.Scheme;
            var baseUrl = _context?.HttpContext?.Request.Host;

            if (baseUrl == null) throw new Exception("Cannot get base url");

            if (!_filesAllowed.Contains(fileType.ToLowerInvariant()))
            {
                throw new Exception("File is not permited");
            }

            var docName = Path.GetFileName(formFile.FileName);
            var destination = Path.Combine(_basePath, "", docName);
            file.DocumentName = docName;
            file.DocumentType = fileType.Replace(".", "");
            file.DocumentUrl = Path.Combine(scheme+"://"+baseUrl + "/v1/api/files/" + file.DocumentName);

            try
            {    
                using var stream = new FileStream(destination, FileMode.Create);
                await formFile.CopyToAsync(stream);
            }
            catch (Exception)
            {
                throw;
            }

            return file;
        }

        public async Task<List<FileDetailVO>> saveFiles(IList<IFormFile> formFiles)
        {
            List<FileDetailVO> fileDetailVOs = new List<FileDetailVO>();
            foreach (var f in formFiles)
            {
                fileDetailVOs.Add(await this.saveFile(f));
            }
            return fileDetailVOs;
        }
    }
}
