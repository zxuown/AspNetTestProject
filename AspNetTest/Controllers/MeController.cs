using Microsoft.AspNetCore.Mvc;
using AspNetTest.Service;
using System.Diagnostics.Metrics;
using AspNetTest.Models;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Authorization;

namespace AspNetTest.Controllers
{
	[Authorize]
	public class MeController : Controller
    {
        private readonly SiteContext _context;
        public IActionResult Test()
        {
            return View();
        }
        private readonly IWebHostEnvironment _webHostEnvironment;
		public MeController(SiteContext context, IWebHostEnvironment hostEnvironment) 
        {
			_context = context;
			_webHostEnvironment = hostEnvironment;

		}
        public IActionResult Index()
        {
            return View(_context.AboutMe.Include(x=>x.Abilities).First());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Abilities());
        }
        [HttpPost]
        public IActionResult Create([FromForm] Abilities abilities, IFormFile? image)
        {
            if (abilities.Name.Length <= 1)
            {
                ModelState.AddModelError(nameof(Abilities.Name), "Too low");
                return View(abilities);
            }

            if (!ModelState.IsValid)
            {
                return View(abilities);
            }
            if (image != null)
            {
				var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
				if (abilities.Image != null)
				{
					System.IO.File.Delete(Path.Combine(_webHostEnvironment.WebRootPath, "uploads", abilities.Image));
				}
				using (var file = System.IO.File.OpenWrite(Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName)))
				{
					image.CopyTo(file);
				}
				abilities.Image = fileName;
			}
			var aboutMe = _context.AboutMe.First();
            aboutMe.Abilities.Add(abilities);
			_context.Add(abilities);
			_context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet("/Me/Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var abiliti = _context.Abilities.First(x => x.Id == id);
            return View(abiliti);
        }

        [HttpPost("/Me/Edit/{id}")]
        public IActionResult Edit(int id, [FromForm] Abilities form, IFormFile? image)
        {
            if (form.Name.Length <= 1)
            {   
                ModelState.AddModelError(nameof(form.Name), "Too low");
                return View(form);
            }

            if (!ModelState.IsValid)
            {
                return View(form);
            }

            var abiliti = _context.Abilities.First(x => x.Id == id);
            if (image != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                if (abiliti.Image != null)
                {
                    System.IO.File.Delete(Path.Combine(_webHostEnvironment.WebRootPath, "uploads", abiliti.Image));
                }
                using (var file = System.IO.File.OpenWrite(Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName)))
                {
                    image.CopyTo(file);
                }
                abiliti.Image = fileName;
			}

            abiliti.Level = form.Level;
            abiliti.Name = form.Name;

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var abiliti = _context.Abilities.First(x => x.Id == id);
			_context.Abilities.Remove(abiliti);
			_context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
