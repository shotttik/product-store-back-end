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

            var dbPath = Path.Combine(folderName, fileName);
            var fullPath = Path.Combine(pathToSave, fileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            if(IsImage)
            {
                string [] paths = { Directory.GetCurrentDirectory(), folderName, "Thumbnails", fileName };
                dbPath = Path.Combine(paths.Skip(1).ToArray());
                var thumbPath = Path.Combine(paths);
                Utils.GenerateThumbnail(fullPath, thumbPath);
            }
            return Ok(new { dbPath });
        }
    }
}
