using AspNetTest.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RV211AspNet.Models.Forms;
using System.Security.Claims;

namespace RV211AspNet.Controllers;

public class AccountController : Controller
{
	private readonly SignInManager<User> _signInManager;
	private readonly UserManager<User> _userManager;

	public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
	{
		_userManager = userManager;
		_signInManager = signInManager;
	}

	public IActionResult Index()
	{
		return View();
	}

	[HttpGet]
	public async Task<IActionResult> Login()
	{
		return View(new LoginForm());
	}

	[HttpPost]
	public async Task<IActionResult> Login([FromForm] LoginForm form)
	{

		if (!ModelState.IsValid)
		{
			return View(form);
		}

		var user = await _userManager.FindByEmailAsync(form.Login);
		if (user == null)
		{
			ModelState.AddModelError(nameof(form.Login), "User not exists");
			return View(form);
		}

		var signInResult = await _signInManager.PasswordSignInAsync(user, form.Password, true, false);
		
		if (!signInResult.Succeeded)
		{
			ModelState.AddModelError(nameof(form.Login), "Sign in fail");
			return View(form);
		}

		return Redirect("/");
	}

	[HttpGet]
	public async Task<IActionResult> Register()
	{
		return View(new RegisterForm());
	}
	[HttpPost]
	[Authorize]
	public async Task<IActionResult> LogOut()
	{
		await _signInManager.SignOutAsync();
		return Redirect("/");
	}
	[HttpPost]
	public async Task<IActionResult> Register([FromForm] RegisterForm form)
	{

		if (!ModelState.IsValid)
		{
			return View(form);
		}
		var user = await _userManager.FindByEmailAsync(form.Login);

		if (user != null)
		{
			ModelState.AddModelError(nameof(form.Login), "User already exists");
			return View(form);
		}

		user = new User
		{
			Email = form.Login,
			UserName = form.Username,
		};

		var registerResult = await _userManager.CreateAsync(user, form.Password);

		if (!registerResult.Succeeded)
		{
			ModelState.AddModelError(nameof(form.Login), "Invalid data");
			return View(form);
		}

		var signInResult = await _signInManager.PasswordSignInAsync(form.Login, form.Password, true, false);

		if (signInResult.Succeeded)
		{
			ModelState.AddModelError(nameof(form.Login), "Sign in fail");
			return View(form);

		}

		return Redirect("/");
	}
}
