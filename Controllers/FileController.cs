using Microsoft.AspNetCore.Mvc;

namespace SimpleUploadServer.Controllers;

[ApiController]
[Route("[controller]")]
public class FileController : ControllerBase
{
   private readonly ILogger<FileController> _logger;

    public FileController(ILogger<FileController> logger)
    {
        _logger = logger;
    }
    
    
    [HttpPost]
    [RequestSizeLimit(int.MaxValue)] 
    [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        var filename = file.FileName;
        var directory = Directory.GetCurrentDirectory();

        await using (FileStream fs = System.IO.File.Create($"{directory}/{filename}"))
        {
            await file.CopyToAsync(fs);
            await fs.FlushAsync();
            fs.Close();
        }

        return Ok();
    }
    
    [HttpGet("{filename}")]
    public FileStreamResult Get(string filename)
    {
        var directory = Directory.GetCurrentDirectory();

        var f = new FileStream($"{directory}/{filename}", FileMode.Open, FileAccess.Read);

        return new FileStreamResult(f, "application/octet-stream");
    }
}