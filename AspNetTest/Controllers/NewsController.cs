using AspNetTest.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace AspNetTest.Controllers
{
	public class NewsController : Controller
	{
		private readonly SiteContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public NewsController(SiteContext context, IWebHostEnvironment hostEnvironment)
		{
			_context= context;
            _webHostEnvironment= hostEnvironment;
		}
		public IActionResult Index()
		{
			return View(_context.News.ToList());
		}
        [HttpGet]
        public IActionResult Create()
        {
            return View(new NewsItem());
        }
        [HttpPost]
        public IActionResult Create([FromForm] NewsItem newsItem, IFormFile? image)
        {
            if (!ModelState.IsValid)
            {
                return View(newsItem);
            }
            if (image != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                if (newsItem.Image != null)
                {
                    System.IO.File.Delete(Path.Combine(_webHostEnvironment.WebRootPath, "news", newsItem.Image));
                }
                using (var file = System.IO.File.OpenWrite(Path.Combine(_webHostEnvironment.WebRootPath, "news", fileName)))
                {
                    image.CopyTo(file);
                }
                newsItem.Image = fileName;
            }
            _context.Add(newsItem);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
		[HttpGet("/News/Show/{id}")]
		public IActionResult Show(int id)
		{
			var news = _context.News.First(x => x.Id == id);
			return View(news);
		}
        [HttpGet("/News/Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var news = _context.News.First(x => x.Id == id);
            return View(news);
        }

        [HttpPost("/News/Edit/{id}")]
        public IActionResult Edit(int id, [FromForm] NewsItem newsItem, IFormFile? image)
        {


            if (!ModelState.IsValid)
            {
                return View(newsItem);
            }

            var news = _context.News.First(x => x.Id == id);
            if (image != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                if (news.Image != null)
                {
                    System.IO.File.Delete(Path.Combine(_webHostEnvironment.WebRootPath, "news", news.Image));
                }
                using (var file = System.IO.File.OpenWrite(Path.Combine(_webHostEnvironment.WebRootPath, "news", fileName)))
                {
                    image.CopyTo(file);
                }
                news.Image = fileName;
            }

            news.Header = newsItem.Header;
            news.Text = newsItem.Text;
            news.ShortText = newsItem.ShortText;
            news.Date = newsItem.Date;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var news = _context.News.First(x => x.Id == id);
            _context.News.Remove(news);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
