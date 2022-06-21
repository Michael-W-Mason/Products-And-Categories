using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Products_And_Categories.Models;
using Microsoft.EntityFrameworkCore;

namespace Products_And_Categories.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    public Context _context;

    public HomeController(ILogger<HomeController> logger, Context context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("products")]
    public IActionResult Products()
    {
        ViewBag.Products = _context.Products.ToList();
        return View("Products");
    }

    [HttpGet("products/{ProductId}")]
    public IActionResult OneProduct(int ProductId)
    {
        ViewBag.oneProduct = _context.Products
        .Include(prod => prod.Categories)
        .ThenInclude(cat => cat.Category)
        .FirstOrDefault(prod => prod.ProductId == ProductId);

        ViewBag.otherCategories = _context.Categories
        .Include(cat => cat.Products)
        .ThenInclude(prod => prod.Product)
        .Where(cat => !(cat.Products.Any(prod => prod.ProductId == ProductId)));

        return View("Product");
    }

    [HttpGet("categories/{CategoryId}")]
    public IActionResult OneCategory(int CategoryId)
    {
        ViewBag.oneCategory = _context.Categories
        .Include(cat => cat.Products)
        .ThenInclude(prod => prod.Product)
        .FirstOrDefault(cat => cat.CategoryId == CategoryId);

        ViewBag.otherProducts = _context.Products
        .Include(prod => prod.Categories)
        .ThenInclude(cat => cat.Category)
        .Where(prod => !(prod.Categories.Any(cat => cat.CategoryId == CategoryId)));

        return View("Category");
    }

    [HttpPost("products/create")]
    public IActionResult CreateProduct(Product newProduct)
    {
        if (ModelState.IsValid)
        {
            _context.Add(newProduct);
            _context.SaveChanges();
            return RedirectToAction("Products");
        }
        ViewBag.Products = _context.Products.ToList();
        return View("Products");
    }

    [HttpPost("products/update/{ProductId}")]
    public IActionResult UpdateProduct(int ProductId, Association newAssociation)
    {
        newAssociation.ProductId = ProductId;
        _context.ProductsAndCategories.Add(newAssociation);
        _context.SaveChanges();
        return Redirect($"/products/{newAssociation.ProductId}");
    }

    [HttpPost("categories/update/{CategoryId}")]
    public IActionResult UpdateCategory(int CategoryId, Association newAssociation)
    {
        newAssociation.CategoryId = CategoryId;
        _context.ProductsAndCategories.Add(newAssociation);
        _context.SaveChanges();
        return Redirect($"/categories/{newAssociation.CategoryId}");
    }

    [HttpGet("")]
    public IActionResult Category()
    {
        return View("Category");
    }

    [HttpGet("categories")]
    public IActionResult Categories()
    {
        ViewBag.Categories = _context.Categories.ToList();
        return View("Categories");
    }

    [HttpPost("categories/create")]
    public IActionResult CreateCategory(Category newCategory)
    {
        if (ModelState.IsValid)
        {
            _context.Add(newCategory);
            _context.SaveChanges();
            return RedirectToAction("Categories");
        }
        ViewBag.Categories = _context.Categories.ToList();
        return View("Categories");
    }






    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
