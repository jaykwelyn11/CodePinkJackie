using System.Diagnostics;
using CodePinkJackie.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;


public class CartController : Controller
{
    private MyContext db;
    // Here we can "inject" our context service into the constructor 
    public CartController(MyContext context)
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


    [HttpPost("/codepink/{id}/cart/add")]
    public IActionResult AddToCart(int id, Product addedProduct)
    {

        Product? dbProduct = db.Products.FirstOrDefault(t => t.ProductId == id);
        if (dbProduct == null)
        {
            return RedirectToAction("AllProducts");
            // Console.BackgroundColor = ConsoleColor.Black;
            // Console.ForegroundColor = ConsoleColor.Red;
            // Console.WriteLine("FAIL.");
        }

        dbProduct.AddToCart = true;

        db.Products.Update(dbProduct);
        db.SaveChanges();

        return RedirectToAction("AllProducts");
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