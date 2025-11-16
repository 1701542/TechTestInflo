using System.Linq;
using UserManagement.Models;
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

    [HttpGet("list/view")]
    public ViewResult ViewUserDetails(long id)
    {
        var userData = _userService
            .GetAll()
            .SingleOrDefault(u => u.Id == id);

        if(userData == null)
        {
            return View("Index");
        }

        var userItem = new UserListItemViewModel
        {
            Id = userData.Id,
            Forename = userData.Forename,
            Surname = userData.Surname,
            Email = userData.Email,
            IsActive = userData.IsActive,
            DateOfBirth = userData.DateOfBirth
        };

        var userViewModel= new UserListViewModel
        {
            Items = new List<UserListItemViewModel> { userItem }
        };

        return View("View", userViewModel);
    }

    [HttpGet("list/create")]
    public ViewResult Create()
    {
        return View("Create", new CreateUserViewModel());
    }

    [HttpPost("list/create")]
    public IActionResult Create(CreateUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Create", model);
        }

        //  Could not determine the end user level of privilege so assumed on creation IsActive is true, and Id should be automatically set.
        var newUser = new User
        {
            Id = _userService.GetAll().Select(u => u.Id).DefaultIfEmpty(0).Max() + 1,
            Forename = model.Forename,
            Surname = model.Surname,
            Email = model.Email,
            IsActive = true,
            DateOfBirth = model.DateOfBirth
        };

        _userService.Create(newUser);

        return RedirectToAction("List");
    }

    [HttpGet("edit/{id}")]
    public IActionResult EditUserDetails(long id)
    {
        //implement here
        var user = _userService.GetAll().SingleOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }

        var model = new UserListItemViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            IsActive = user.IsActive,
            DateOfBirth = user.DateOfBirth
        };

        return View("Edit", model);
    }

    [HttpPost("edit/{id}")]
    public IActionResult EditUserDetails(long id, UserListItemViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Edit", model);
        }

        var user = _userService.GetAll().SingleOrDefault(u => u.Id == id);
        if(user == null)
        {
            return NotFound();
        }

        user.Forename = model.Forename!;
        user.Surname = model.Surname!;
        user.Email = model.Email!;
        user.IsActive = model.IsActive;
        user.DateOfBirth = model.DateOfBirth;

        _userService.Update(user);

        return RedirectToAction("List");
    }

    [HttpGet("delete/{id}")]
    public IActionResult DeleteUser(long id)
    {
        var user = _userService.GetAll().SingleOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }

        var model = new UserListItemViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            IsActive = user.IsActive,
            DateOfBirth = user.DateOfBirth
        };

        return View("Delete", model);
    }

    [HttpPost("delete/{id}")]
    public IActionResult DeleteConfirmed(long id)
    {
        var user = _userService.GetAll().SingleOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }

        _userService.Delete(user);

        return RedirectToAction("List");
    }
}
