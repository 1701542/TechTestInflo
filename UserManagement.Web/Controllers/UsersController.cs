using System.Linq;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;

    private UserListViewModel BuildModel(IEnumerable<Models.User> users)
    {
        var items = users.Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            IsActive = p.IsActive,
            DateOfBirth = p.DateOfBirth
        });

        return new UserListViewModel
        {
            Items = items.ToList()
        };
    }

    [HttpGet("list")]
    public ViewResult List()
    {
        var users = _userService.GetAll();
        return View("List", BuildModel(users));
    }

    [HttpGet("list/active")]
    public ViewResult ListActive()
    {
        var users = _userService.FilterByActive(true);
        return View("List", BuildModel(users));
    }

    [HttpGet("list/not-active")]
    public ViewResult ListNotActive()
    {
        var users = _userService.FilterByActive(false);
        return View("List", BuildModel(users));
    }
}
