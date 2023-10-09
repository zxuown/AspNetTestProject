using AspNetTest.Models;
using AspNetTest.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<SiteContext>(options =>
{
	options.UseSqlite("Data Source=context.db");
	SQLitePCL.Batteries.Init();
});
builder.Services.AddMvc();
builder.Services.AddControllersWithViews();
builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
{
	options.SignIn.RequireConfirmedPhoneNumber = false;
	options.SignIn.RequireConfirmedAccount = false;
	options.SignIn.RequireConfirmedEmail = false;

	options.Password.RequiredLength = 3;
	options.Password.RequiredUniqueChars = 0;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireDigit = false;
	options.Password.RequireLowercase = false;
	options.Password.RequireUppercase = false;
})
	.AddRoles<IdentityRole<int>>()
	.AddEntityFrameworkStores<SiteContext>();
builder.Services.AddScoped<AboutMeService>(x =>
{
    return new AboutMeService("AboutME.json");
});
var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}");
//app.MapControllerRoute(name: "abilities", pattern: "{controller=Home}/{action=Abilities}");
//app.MapControllerRoute(name: "abilitiy_list", pattern: "{controller=Me}/{action=Index}");
//app.MapControllerRoute(name: "abilitiy_create", pattern: "{controller=Me}/{action=Create}");
//app.MapControllerRoute(name: "abilitiy_edit", pattern: "{controller=Me}/{action=Edit}/{id}");
//app.MapControllerRoute(name: "abilitiy_delete", pattern: "{controller=Me}/{action=Delete}/{id}");
app.UseStaticFiles();
app.Run();
