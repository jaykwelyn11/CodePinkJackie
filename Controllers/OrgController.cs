using System.Diagnostics;
using CodePinkJackie.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;


public class OrgController : Controller
{
    private MyContext db;
    // Here we can "inject" our context service into the constructor 
    public OrgController(MyContext context)
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

    [HttpGet("codepink/laelynn")]
    public IActionResult Laelynn()
    {
        return View("Laelynn");
    }


    [HttpGet("/codepink/addStudent")]
    public IActionResult AddStudent()
    {
        return View("AddStudent");
    }


    [HttpPost("/codepink/createStudent")]
    public IActionResult CreateStudent(Student s)
    {
        // s.UserId = (int)HttpContext.Session.GetInt32("uid");
        if (ModelState.IsValid)
        {
            db.Students.Add(s);
            db.SaveChanges();
            return RedirectToAction("StudentSuccess");
        }
        return View("Laelynn");
    }

    [HttpGet("/codepink/studentSuccess")]
    public IActionResult StudentSuccess()
    {
        return View("StudentSuccess");
    }

    [HttpGet("/codepink/addDonation")]
    public IActionResult AddDonation()
    {
        return View("AddDonation");
    }


    [HttpPost("/codepink/createDonation")]
    public IActionResult CreateDonation(Student s)
    {
        // s.UserId = (int)HttpContext.Session.GetInt32("uid");
        if (ModelState.IsValid)
        {
            db.Students.Add(s);
            db.SaveChanges();
            return RedirectToAction("DonationSuccess");
        }
        return View("Laelynn");
    }

    [HttpGet("/codepink/donationSuccess")]
    public IActionResult DonationSuccess()
    {
        return View("DonationSuccess");
    }
}