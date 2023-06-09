using System.Diagnostics;
using CodePinkJackie.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;


public class ProductController : Controller
{
    private MyContext db;
    // Here we can "inject" our context service into the constructor 
    public ProductController(MyContext context)
    {
        // When our UserController is instantiated, it will fill in db with context
        // Remember that when context is initialized, it brings in everything we need from DbContext
        // which comes from Entity Framework Core
        db = context;
    }

    private int? uid
    {
        get
        {
            return HttpContext.Session.GetInt32("uid");
        }
    }



    [HttpGet("/codepink")]
    public IActionResult CodePink()
    {
        return View("Index");
    }



    [HttpGet("/codepink/products")]
    public IActionResult AllProducts()
    {
        List<Product> addedProducts = db.Products
        .OrderBy(p => p.Category).ToList();

        return View("AllProd", addedProducts);
        // return View("AllProd");
    }

    [HttpGet("/codepink/{productId}")]
    public IActionResult ViewOne(int productId)
    {
        Product? product = db.Products.FirstOrDefault(product => product.ProductId == productId);
        if (product == null)
        {
            Console.WriteLine("I am here1");
            Console.WriteLine(productId);
            return RedirectToAction("AllProducts");
        }
        else
        {
            Console.WriteLine("I am here2");
            return View("ViewOne", product);
        }
    }


    [SessionCheck]
    [HttpGet("/codepink/addProduct")]
    public IActionResult AddOne()
    {
        return View("AddOne");
    }


    [SessionCheck]
    [HttpPost("/codepink/createProduct")]
    public IActionResult CreateProduct(Product p)
    {
        // p.UserId = (int)HttpContext.Session.GetInt32("uid");
        p.UserId = 1;
        if (ModelState.IsValid)
        {
            db.Products.Add(p);
            db.SaveChanges();
            return RedirectToAction("AllProducts");
        }
        return View("AddOne");
    }

    [SessionCheck]
    [HttpGet("/codepink/edit/{productId}")]
    public IActionResult EditProduct(int productId)
    {
        Product? item = db.Products
        .Include(item => item.Admin)
        .FirstOrDefault(p => p.ProductId == productId);
        if (uid != 1)
        {
            return RedirectToAction("AllProducts");
        }
        else
        {
            return View("Edit", item);
        }
    }


    [SessionCheck]
    [HttpPost("/codepink/updateProduct/{productId}")]
    public IActionResult UpdateProduct(Product p, int productId)
    {
        if (!ModelState.IsValid)
        {
            return EditProduct(productId);
        }
        Product? item = db.Products.FirstOrDefault(item => item.ProductId == productId);
        if (item == null || item.UserId != HttpContext.Session.GetInt32("uid"))
        {
            return RedirectToAction("AllProducts");
        }
        else
        {
            item.Name = p.Name;
            item.Category = p.Category;
            item.Price = p.Price;
            item.Img1 = p.Img1;
            item.Img2 = p.Img2;
            item.Img3 = p.Img3;
            item.Img4 = p.Img4;
            item.Img5 = p.Img5;
            item.Description = p.Description;
            item.UpdatedAt = DateTime.Now;

            db.Products.Update(item);
            db.SaveChanges();

            return RedirectToAction("ViewOne", new { productId = productId });
        }
    }

    [SessionCheck]
    [HttpGet("/codepink/delete/{productId}")]
    public IActionResult DeleteProduct(int productId)
    {
        Product? item = db.Products.FirstOrDefault(item => item.ProductId == productId);
        if (item != null)
        {
            db.Products.Remove(item);
            db.SaveChanges();
        }
        return RedirectToAction("AllProducts");
    }



    [HttpPost("/codepink/{id}/cart/add")]
    public IActionResult AddToCart(int id, Product addedProduct)
    {

        Product? dbProduct = db.Products.FirstOrDefault(t => t.ProductId == id);
        if (dbProduct == null)
        {
            Console.WriteLine("Not added to cart");
            return RedirectToAction("AllProducts");
            // Console.BackgroundColor = ConsoleColor.Black;
            // Console.ForegroundColor = ConsoleColor.Red;
            // Console.WriteLine("FAIL.");
        }

        dbProduct.AddToCart = true;

        db.Products.Update(dbProduct);
        db.SaveChanges();

        Console.WriteLine("Added to cart");
        return RedirectToAction("ViewCart");
    }


    [HttpPost("/codepink/{id}/cart/delete")]
    public IActionResult RemoveFromCart(int id, Product removedProduct)
    {

        Product? dbProduct = db.Products.FirstOrDefault(t => t.ProductId == id);
        if (dbProduct == null)
        {
            return RedirectToAction("AllProducts");
            // Console.BackgroundColor = ConsoleColor.Black;
            // Console.ForegroundColor = ConsoleColor.Red;
            // Console.WriteLine("FAIL.");
        }

        dbProduct.AddToCart = false;

        db.Products.Update(dbProduct);
        db.SaveChanges();

        return RedirectToAction("ViewCart");
    }


    [HttpGet("/codepink/cart")]
    public IActionResult ViewCart()
    {
        List<Product> ProductsInCart = db.Products.Where(t => t.AddToCart == true).ToList();
        return View("Cart", ProductsInCart);
    }



    [HttpGet("/codepink/checkout")]
    public IActionResult Checkout()
    {
        return View("Checkout");
    }
}


public class SessionCheckAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // Find the session, but remember it may be null so we need int?
        int? uid = context.HttpContext.Session.GetInt32("uid");
        // Check to see if we got back null
        if (uid == null)
        {
            // Redirect to the Index page if there was nothing in session
            // "Home" here is referring to "HomeController", you can use any controller that is appropriate here
            context.Result = new RedirectToActionResult("CodePink", "Product", null);
        }
    }
}