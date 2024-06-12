using web.Data;
using web.Models.DTO;

namespace web.Repositories
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataDbContext _DbContext;

        public LocalImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, DataDbContext dbContext)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _DbContext = dbContext;
        }

        public Image Upload(Image image)
        {
            var LocalFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images",
                $"{image.FileName}{image.FileExtention}");
            //upload image to local path
            using var steam = new FileStream(LocalFilePath, FileMode.Create);
            image.File.CopyTo(steam);

            var urlFilePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}:{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/Image/{image.FileName}{image.FileExtention}";
            image.FilePath = urlFilePath;

            //add iamge to the image table
            _DbContext.Images.Add(image);
            _DbContext.SaveChanges();

            return image;
        }

        public List<Image> GetAllInfoImages()
        {
            var allimages = _DbContext.Images.ToList();
            return allimages;
        }

        public (byte[], string, string) DownLoadFile(int Id)
        {
            try
            {
                var FileById = _DbContext.Images.Where(x => x.Id == Id).FirstOrDefault();
                var path = Path.Combine(_webHostEnvironment.ContentRootPath, "Images", $"{FileById.FileName}{FileById.FileExtention}");
                var stream = File.ReadAllBytes(path);
                var filename = FileById.FileName + FileById.FileExtention;
                return (stream, "application/octet-stream", filename);
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }
    }
}
