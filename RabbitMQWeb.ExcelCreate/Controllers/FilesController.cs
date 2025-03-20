using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMQWeb.ExcelCreate.Contexts;
using RabbitMQWeb.ExcelCreate.Models;

namespace RabbitMQWeb.ExcelCreate.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FilesController : ControllerBase
{
    private readonly AppDbContext _appDbContext;

    public FilesController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file, int FileId)
    {
        if (file is not { Length: > 0 }) return BadRequest();
        var userFile = await _appDbContext.UserFIles.Where(uf => uf.Id == FileId).FirstOrDefaultAsync();
        if (userFile is null) return BadRequest();
        var filePath = userFile.FileName + Path.GetExtension(file.FileName);
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", filePath);

        using FileStream stream = new(path, FileMode.Create);
        await file.CopyToAsync(stream);

        userFile.FilePath = filePath;
        userFile.CreatedDate = DateTime.UtcNow;
        userFile.FileStatus = FileStatus.Completed;
        await _appDbContext.SaveChangesAsync();
        return Ok();
    }
}

