using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using web.Models.Domain;
using web.Models.DTO;
using web.Repositories;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        [HttpPost]
        [Route("Upload")]
        public IActionResult Upload([FromForm] ImageUploadRequestDTO request)
        {
            ValidateFileUpload(request);
            var imageDomainModel = new Image
            {
                File = request.File,
                FileExtention = Path.GetExtension(request.File.FileName),
                FileSizeInBytes = request.File.Length,
                FileName = request.FileName,
                FileDescription = request.FileDescription,
            };
            _imageRepository.Upload(imageDomainModel);
            return Ok(imageDomainModel);
        }

        private void ValidateFileUpload(ImageUploadRequestDTO request)
        {
            var allowExtentions = new string[] { ".jpg", ".jpeg", "png" };
            if (!allowExtentions.Contains(Path.GetExtension(request.File.FileName)))
            {
                ModelState.AddModelError("file", "Unsupported File Extention");
            }
            if (request.File.Length > 1040000)
            {
                ModelState.AddModelError("file", "file size to big, please upload file <10M");
            }
        }

        [HttpGet]
        public IActionResult getAllInfoImages()
        {
            var allImages = _imageRepository.GetAllInfoImages();
            return Ok(allImages);
        }

        [HttpGet]
        [Route("Download")]
        public IActionResult DownloadImages(int id)
        {
            var result = _imageRepository.DownLoadFile(id);
            return File(result.Item1, result.Item2, result.Item3);
        }
    }
}
