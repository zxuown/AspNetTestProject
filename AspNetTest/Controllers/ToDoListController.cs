using AspNetTest.Migrations;
using AspNetTest.Models;
using AspNetTest.Models.Forms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetTest.Controllers
{
    [Authorize]
    public class ToDoListController : Controller
    {
        private readonly SiteContext _context;
        private readonly UserManager<User> _userManager;
        public ToDoListController(SiteContext context, UserManager<User> userManager) 
        { 
            _context= context;
            _userManager= userManager;
        }

        private async Task<User> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(HttpContext.User);
        }

        [HttpGet("/ToDoList/List/{date}")]
        public async Task <IActionResult> List(string date)
        {
            DateTime dt = DateTime.Parse(date);
            var user = await GetCurrentUserAsync();
            return Ok(await _context.ToDoLists.Where(x => x.Date.Date == dt.Date 
            && x.User.Id == user.Id).ToListAsync());
        }
        [HttpPost("/ToDoList/Add")]
        public async Task<IActionResult> Add([FromBody] TodoItemForm item)
        {
            await _context.AddAsync(new Models.ToDoList
            {
                Done = item.Done,
                Title = item.Title, 
                Date= item.Date.AddHours((double)3),
                User = await GetCurrentUserAsync(),
            });
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpPost("/ToDoList/Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TodoItemForm item)
        {
            var user = await GetCurrentUserAsync();
            var toDoList = _context.ToDoLists.First(x => x.Id == id && x.User.Id == user.Id);
            toDoList.Title = item.Title;
            toDoList.Date = item.Date;
            
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("/ToDoList/SetCompleted/{id}")]
        public async Task<IActionResult> SetCompleted(int id)
        {
            var user = await GetCurrentUserAsync();
            var toDoList = _context.ToDoLists.First(x => x.Id == id && x.User.Id == user.Id);
            toDoList.Done = true;
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpPost("/ToDoList/SetNotCompleted/{id}")]
        public async Task<IActionResult> SetNotCompleted(int id)
        {
            var user = await GetCurrentUserAsync();
            var toDoList = _context.ToDoLists.First(x => x.Id == id && x.User.Id == user.Id);
            toDoList.Done = false;
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var user = await GetCurrentUserAsync();
            var toDoList = _context.ToDoLists.First(x => x.Id == id && x.User.Id == user.Id);
            _context.ToDoLists.Remove(toDoList);
            _context.SaveChanges();
            return Ok(new { Ok = true });
        }
        [HttpDelete("/ToDoList/DeleteAllDone/{date}")]
        public async Task<IActionResult> DeleteAllDoneAsync(string date)
        {
            var user = await GetCurrentUserAsync();
            DateTime dt = DateTime.Parse(date);
            _context.ToDoLists.RemoveRange(_context.ToDoLists
                .Where(x => x.Date.Date == dt.Date && x.User.Id == user.Id)
                .Where(x => x.Done));
            _context.SaveChanges();
            return Ok(new { Ok = true });
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
