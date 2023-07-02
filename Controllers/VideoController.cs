using MediaManageAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace MediaManageAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class VideoController : ControllerBase
{
    [HttpPost]
    public ActionResult Post([FromForm] VideoModel file)
    {
        try
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", file.FileName);

            using (Stream stream = new FileStream(path, FileMode.Create))
            {
                file.FormFile.CopyTo(stream);
            }

            return StatusCode(StatusCodes.Status201Created);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
