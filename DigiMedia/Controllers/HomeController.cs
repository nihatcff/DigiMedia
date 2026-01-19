using System.Diagnostics;
using System.Threading.Tasks;
using DigiMedia.Contexts;
using DigiMedia.ViewModels.ProjectViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DigiMedia.Controllers
{
    public class HomeController(AppDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var projects = await _context.Projects.Select(x=>new ProjectGetVM()
            {
                Id = x.Id,
                Name = x.Name,
                CategoryName = x.Category.Name,
                ImagePath = x.ImagePath
            }).ToListAsync();

            return View(projects);
        }
    }
}
