using RestApiWithDontNet.Data.VO;

namespace RestApiWithDontNet.Business
{
    public interface IFileBusiness
    {
        public byte[] getFile(string filename);

        public Task<FileDetailVO> saveFile(IFormFile formFile);

        public Task<List<FileDetailVO>> saveFiles(IList<IFormFile> formFile);
    }
}
