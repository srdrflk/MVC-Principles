using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MVCPrinciples.Data;
using MVCPrinciples.Models;
using System.Diagnostics;

namespace MVCPrinciples.Controllers
{
    public class HomeController : Controller
    {
        private readonly NorthwindContext _context;
        private readonly AppSettings _settings;

        public HomeController(NorthwindContext context, IOptions<AppSettings> settings)
        {
            _context = context;
            _settings = settings.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Create()
        {
            var model = new ProductEditViewModel
            {
                AvailableCategories = _context.Categories.ToList(),
                AvailableSuppliers = _context.Suppliers.ToList()
            };
            return View("Edit", model);
        }
        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            var model = new ProductEditViewModel
            {
                ProductID = product.ProductID,
                ProductName = product.ProductName,
                CategoryID = product.CategoryID,
                SupplierID = product.SupplierID,
                QuantityPerUnit = product.QuantityPerUnit,
                UnitPrice = product.UnitPrice,
                UnitsInStock = product.UnitsInStock,
                UnitsOnOrder = product.UnitsOnOrder,
                ReorderLevel = product.ReorderLevel,
                Discontinued = product.Discontinued,
                AvailableCategories = _context.Categories.ToList(),
                AvailableSuppliers = _context.Suppliers.ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableCategories = _context.Categories.ToList();
                model.AvailableSuppliers = _context.Suppliers.ToList();
                return View(model);
            }

            Product product;
            if (model.ProductID == 0) // New product
            {
                product = new Product();
                _context.Products.Add(product);
            }
            else // Existing product
            {
                product = _context.Products.Find(model.ProductID);
                if (product == null)
                {
                    return NotFound();
                }
            }

            // Map properties
            product.ProductName = model.ProductName;
            product.CategoryID = model.CategoryID;
            product.SupplierID = model.SupplierID;
            product.QuantityPerUnit = model.QuantityPerUnit;
            product.UnitPrice = model.UnitPrice;
            product.UnitsInStock = model.UnitsInStock;
            product.UnitsOnOrder = model.UnitsOnOrder;
            product.ReorderLevel = model.ReorderLevel;
            product.Discontinued = model.Discontinued;

            _context.SaveChanges();
            return RedirectToAction("Products");
        }

        public IActionResult Categories()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        public IActionResult Products()
        {
            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .AsQueryable();

            // Apply product limit if M > 0
            if (_settings.MaxProductsToShow > 0)
            {
                query = query.Take(_settings.MaxProductsToShow);
            }

            var products = query.ToList();

            ViewBag.MaxProductsToShow = _settings.MaxProductsToShow;
            return View(products);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
