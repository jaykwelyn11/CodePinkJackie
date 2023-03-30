#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace CodePinkJackie.Models;

public class User
{
    [Key]
    public int UserId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Nickname { get; set; }

    public Date DateOfBirth { get; set; }

    public string SchoolIdImage { get; set; }

    public string ParentIdImage { get; set; }

    [Required]
    [EmailAddress]
    [UniqueEmail]
    public string Email { get; set; }

    [Required]
    [SpecialPassword]
    [MinLength(8, ErrorMessage = "Woof woof! Needs to be at least 8 characters please!")]
    [DataType(DataType.Password)]
    public string Password { get; set; }


    [NotMapped]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Woof! Passwords don't match, try again!")]
    public string Confirm { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

public class UniqueEmailAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext valContext)
    {
        if (value == null)
        { // if email input is empty
            return new ValidationResult("Email is required");
        }
        MyContext _context = (MyContext)valContext.GetService(typeof(MyContext));
        if (_context.Users.Any(e => e.Email == value.ToString()))
        {
            return new ValidationResult("Email is in use"); // if email is in database
        }
        else
        {
            return ValidationResult.Success; // email not in database good to go
        }
    }
}