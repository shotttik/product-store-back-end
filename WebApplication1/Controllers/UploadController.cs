using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using WebApplication1;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "1")]
    public class UploadController :Controller
    {
        [HttpPost]
        public async Task<IActionResult> Upload()
        {
            var formCollection = await Request.ReadFormAsync();
            var file = formCollection.Files.First();
            if (file.Length <= 0)
            {
                return BadRequest();
            }
            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var folderName = "";
            var IsImage = false;
            if (fileName.EndsWith(".pdf"))
            {
                folderName = Path.Combine("Resources", "Documents");
            }
            else
            {
                folderName = Path.Combine("Resources", "Images");
                IsImage= true;
            }
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            var fullPath = Path.Combine(pathToSave, fileName);
            var dbPath = Path.Combine(folderName, fileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            if(IsImage)
            {
                var thumbPath = Path.Combine(pathToSave, "Thumbnails");
                var thumbFilePath = Path.Combine(thumbPath, fileName);
                Utils.GenerateThumbnail(fullPath, thumbFilePath);
            }
            return Ok(new { dbPath });
        }
    }
}
