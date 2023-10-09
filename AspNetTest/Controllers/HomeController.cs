using AspNetTest.Models;
using Microsoft.AspNetCore.Mvc;
using AspNetTest.Service;
using Microsoft.EntityFrameworkCore;

namespace AspNetTest.Controllers
{
    public class HomeController : Controller
    {
		//private readonly AboutMeService _aboutMeService;
		private readonly SiteContext _context;
		public HomeController(SiteContext context) 
        {
			_context = context;
		}
        [HttpGet]
        public IActionResult Index()
        {   
            ViewData["AboutMe"] = _context.AboutMe.Include(x=>x.Abilities).First();
            ViewData["News"] = _context.News.Take(6).ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Abilities([FromBody] AboutMeForm form)
        {
            
            Console.WriteLine(form.Query);
            return Json(_context.Abilities
                .Where(x => x.Name.ToLower().Contains(form.Query.ToLower()))); 
        }
    }
}
