using VideoPlayer.Models;
using VideoPlayer.Models.Domain;

namespace VideoPlayer.Services
{
    public interface IFileRepository
    {
        Task<bool> UploadFiles(FileDetail files);
        IEnumerable<VideoFile> GetVideoFiles();
    }
}
