using System;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Web.Models.Users;

public class UserListViewModel
{
    public List<UserListItemViewModel> Items { get; set; } = new();
}

public class UserListItemViewModel
{
    public long Id { get; set; }
    [Required]
    public string? Forename { get; set; }
    [Required]
    public string? Surname { get; set; }
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    public bool IsActive { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Date of Birth")]
    public DateTime DateOfBirth { get; set; }
}
