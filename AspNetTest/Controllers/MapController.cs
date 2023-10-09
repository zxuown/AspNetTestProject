using AspNetTest.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspNetTest.Controllers
{
	public class MapController : Controller
	{
		private readonly SiteContext _context;
		public MapController(SiteContext context)
		{
			_context = context;
		}

		public IActionResult Show(int id)
		{
			var markers = _context.Markers.First(x => x.Id == id);
			return View(markers);
		}
		public IActionResult Index()
		{
			return View(_context.Markers.ToList());
		}
		[HttpPost]
		public IActionResult Create([FromBody] Marker marker)
		{
			_context.Markers.Add(marker);
			_context.SaveChanges();

			return Ok(marker);
		}
        [HttpPost]
        public IActionResult Edit([FromBody] Marker data,int id)
        {
			var marker = _context.Markers.First(x=>x.Id ==id);
			marker.Lat = data.Lat;
			marker.Lon= data.Lon;
			marker.Name = data.Name;
            _context.SaveChanges();
            return Ok(marker);
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var marker = _context.Markers.First(x => x.Id == id);
			_context.Markers.Remove(marker);
            _context.SaveChanges();
            return Ok(new {Ok=true});
        }
        public IActionResult Markers()
        {
            return Ok(_context.Markers.ToList());
        }
    }
}
