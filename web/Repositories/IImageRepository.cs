using web.Models.DTO;

namespace web.Repositories
{
    public interface IImageRepository
    {
        Image Upload(Image image);

        List<Image> GetAllInfoImages();

        (byte[], string, string) DownLoadFile(int Id);
    }
}
