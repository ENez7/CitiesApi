using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.Api.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class FilesController : ControllerBase
{
    private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;
        
    // Inject FileExtensionContentTypeProvider service for MIME or to set the content type of an HTTP response
    public FilesController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
    {
        _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider 
                                            ?? throw new ArgumentNullException(nameof(fileExtensionContentTypeProvider));
    }
        
    [HttpGet("{fileId}")]
    public ActionResult GetFile(string fileId)
    {
        // Look up for the actual file, depending on the fileId
        // Demonstration code
        const string pathToFile = "ASP.NET Core 6 Web API Fundamentals.postman_collection.json";
        if (!System.IO.File.Exists(pathToFile)) return NotFound();
        if (!_fileExtensionContentTypeProvider.TryGetContentType(pathToFile, out var contentType))
        {
            contentType = "application/octet-stream";  // Default media type for arbitrary binary data
        }
        var bytes = System.IO.File.ReadAllBytes(pathToFile);
        return File(bytes, contentType, Path.GetFileName(pathToFile));
    }
}