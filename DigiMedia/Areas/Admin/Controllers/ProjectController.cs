using System.Threading.Tasks;
using DigiMedia.Contexts;
using DigiMedia.Helpers;
using DigiMedia.Models;
using DigiMedia.ViewModels.ProjectViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DigiMedia.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ProjectController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;
    private readonly string _folderPath;

    public ProjectController(AppDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
        _folderPath = Path.Combine(_environment.WebRootPath, "assets", "images");
    }

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

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await _sendCategoriesWithViewBag();
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Create(ProjectCreateVM vm)
    {
        await _sendCategoriesWithViewBag();
        if (!ModelState.IsValid)
            return View(vm);

        var isExistCategory = await _context.Categories.AnyAsync(x=>x.Id == vm.CategoryId);
        if (!isExistCategory)
        {
            ModelState.AddModelError("CategoryId","This category doesn't exist");
            return View(vm);
        }

        if (!vm.Image.CheckSize(2))
        {
            ModelState.AddModelError("Image", "Size must be less than 2 mb");
        }

        if (!vm.Image.CheckType("image"))
        {
            ModelState.AddModelError("Image", "File must be Image");
            return View(vm);
        }

        string uniqueFileName = await vm.Image.UploadFileAsync(_folderPath);

        Project project = new()
        {
            Name = vm.Name,
            CategoryId = vm.CategoryId,
            ImagePath= uniqueFileName
        };

        await _context.AddAsync(project);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));       
    }

    public async Task<IActionResult> Delete(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project is null)
            return NotFound();

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();

        var deletedImagePath = Path.Combine(_folderPath, project.ImagePath);
        ExtensionMethods.DeleteFile(deletedImagePath);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        await _sendCategoriesWithViewBag();
        var project = await _context.Projects.FindAsync(id);
        if (project is null)
            return NotFound();

        ProjectUpdateVM vm = new()
        {
            Id = project.Id,
            Name = project.Name,
            CategoryId = project.CategoryId
        };

        return View(vm);
    }


    [HttpPost]
    public async Task<IActionResult> Update(ProjectUpdateVM vm)
    {
        await _sendCategoriesWithViewBag();
        if (!ModelState.IsValid)
            return View(vm);

        var isExistCategory = await _context.Categories.AnyAsync(x => x.Id == vm.CategoryId);
        if (!isExistCategory)
        {
            ModelState.AddModelError("CategoryId", "This category doesn't exist");
            return View(vm);
        }

        if (!vm.Image?.CheckSize(2) ?? false)
        {
            ModelState.AddModelError("Image", "Size must be less than 2 mb");
        }

        if (!vm.Image?.CheckType("image") ?? false)
        {
            ModelState.AddModelError("Image", "File must be Image");
            return View(vm);
        }

        var existProject = await _context.Projects.FindAsync(vm.Id);
        if (existProject is null)
            return BadRequest();

        existProject.Name = vm.Name;
        existProject.CategoryId = vm.CategoryId;
        
        if(vm.Image is { })
        {
            string newImagePath = await vm.Image.UploadFileAsync(_folderPath);
            string oldImagePath = Path.Combine(_folderPath, existProject.ImagePath);

            ExtensionMethods.DeleteFile(oldImagePath);
            existProject.ImagePath = newImagePath;
        }

        _context.Projects.Update(existProject);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }








    private async Task _sendCategoriesWithViewBag()
    {
        var categories = await _context.Categories.ToListAsync();
        ViewBag.Categories = categories;
    }
}
