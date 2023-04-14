#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace CodePinkJackie.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class Donation
{
    [Key]
    public int DonationId { get; set; }

    public string Name { get; set; }

    [Required]
    public bool DonateProduct { get; set; }

    [Required]
    public bool DonateMoney { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;


    public int UserId { get; set; }

}