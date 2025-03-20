using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMQWeb.ExcelCreate.Contexts;
using RabbitMQWeb.ExcelCreate.Models;
using RabbitMQWeb.ExcelCreate.Services;

namespace RabbitMQWeb.ExcelCreate.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly RabbitMQPublisher _rabbitMQPublisher;

        public ProductController(AppDbContext appDbContext, UserManager<AppUser> userManager,RabbitMQPublisher rabbitMQPublisher)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
            _rabbitMQPublisher = rabbitMQPublisher;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> CreateProductExcel()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            string fileName = $"product-excel-{Guid.NewGuid().ToString().Substring(1, 10)}";
            UserFIle userFIle = new()
            {
                UserId = user.Id,
                FileName = fileName,
                FilePath = "-",
                FileStatus = FileStatus.Creating,
            };
            TempData["StartCreatingExcel"] = true;
            await _appDbContext.AddAsync(userFIle);
            await _appDbContext.SaveChangesAsync();
            await _rabbitMQPublisher.Publish(new CreateExcelMessage() { UserFileId=userFIle.Id });
            return RedirectToAction(nameof(GetFiles));
        }
        public async Task<IActionResult> GetFiles()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            return View(await _appDbContext.UserFIles.Where(uf=>uf.UserId==user.Id).OrderByDescending(uf=>uf.Id).ToListAsync());
        }
    }
}
