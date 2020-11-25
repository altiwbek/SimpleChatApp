using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ChatApp.Data;
using ChatApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ChatApp.Controllers
{
    [Authorize]
    public class ChatsController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<User> _userManager;

        public ChatsController(ApplicationContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Chats
        public async Task<IActionResult> Index()
        {
            return View(await _context.Chats.ToListAsync());
        }

        // GET: Chats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chat = await _context.Chats
                .Include(ch => ch.Messages).ThenInclude(m => m.User)
                .Include(ch => ch.ChatUsers).ThenInclude(chu => chu.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chat == null)
            {
                return NotFound();
            }            

            string userId = _userManager.GetUserId(User);

            ViewBag.CurrentUserName = _context.Users.FirstOrDefault(u => u.Id == userId).UserName;

            if (!chat.ChatUsers.Any(cu => cu.UserId == userId))
            {
                return RedirectToAction(nameof(Index));
            }


            ViewBag.UsersSelect = new SelectList(_context.Users.AsNoTracking(), "Id", "UserName");

            return View(chat);
        }

        // GET: Chats/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Chats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CreatedAt")] Chat chat)
        {
            chat.CreatedAt = DateTime.Now;
            string userId = _userManager.GetUserId(User);
            if (ModelState.IsValid)
            {
               
                _context.Add(chat);
                _context.SaveChanges();

                ChatUser chatUser = new ChatUser()
                {
                    CreatedAt = DateTime.Now,
                    ChatId = chat.Id,
                    UserId = userId
                };
                _context.Add(chatUser);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(chat);
        }

        // GET: Chats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chat = await _context.Chats.FindAsync(id);
            if (chat == null)
            {
                return NotFound();
            }
            return View(chat);
        }

        // POST: Chats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CreatedAt")] Chat chat)
        {
            if (id != chat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChatExists(chat.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(chat);
        }

        // GET: Chats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chat = await _context.Chats
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chat == null)
            {
                return NotFound();
            }

            return View(chat);
        }

        // POST: Chats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chat = await _context.Chats.FindAsync(id);
            _context.Chats.Remove(chat);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Chats/CreateComment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateComment(Message message)
        {
            message.CreatedAt = DateTime.Now;
            //message.UserId = _userManager.GetUserId(User);

            if (ModelState.IsValid)
            {
                _context.Add(message);
                await _context.SaveChangesAsync();
            }
            return Ok("created");
        }

        // POST: Chats/AddUserToChat
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUserToChat(string  userId, int chatId)
        {
            ChatUser chatUser = new ChatUser()
            {
                CreatedAt = DateTime.Now,
                ChatId = chatId,
                UserId = userId
            };
            _context.Add(chatUser);
            _context.SaveChanges();

            return RedirectToAction(nameof(Details), new { id = chatId });
        }

        private bool ChatExists(int id)
        {
            return _context.Chats.Any(e => e.Id == id);
        }
    }
}
