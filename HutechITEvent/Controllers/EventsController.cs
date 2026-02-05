using HutechITEvent.Data;
using HutechITEvent.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HutechITEvent.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public EventsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            var events = await _context.Events
                .Include(e => e.Category)
                .Include(e => e.Organizer)
                .Where(e => e.Status == EventStatus.Published || e.Status == EventStatus.Ongoing)
                .OrderByDescending(e => e.StartDate)
                .ToListAsync();

            return View(events);
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventItem = await _context.Events
                .Include(e => e.Category)
                .Include(e => e.Organizer)
                .Include(e => e.Images)
                .Include(e => e.Schedules)
                .Include(e => e.Registrations)
                    .ThenInclude(r => r.Student)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (eventItem == null)
            {
                return NotFound();
            }

            return View(eventItem);
        }

        // GET: Events/Create
        [Authorize(Roles = "Admin,Lecturer")]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories.Where(c => c.Type == CategoryType.Event), "Id", "Name");
            return View();
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Lecturer")]
        public async Task<IActionResult> Create(Event eventItem, IFormFile? thumbnailFile)
        {
            // Remove validation for navigation properties
            ModelState.Remove("Category");
            ModelState.Remove("Organizer");
            ModelState.Remove("Images");
            ModelState.Remove("Registrations");
            ModelState.Remove("Schedules");
            
            // Remove validation for ThumbnailUrl if file is uploaded
            if (thumbnailFile != null)
            {
                ModelState.Remove("ThumbnailUrl");
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                eventItem.OrganizerId = user?.Id;
                eventItem.CreatedAt = DateTime.Now;
                eventItem.Status = EventStatus.Published;

                // Handle file upload
                if (thumbnailFile != null && thumbnailFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "events");
                    Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(thumbnailFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await thumbnailFile.CopyToAsync(fileStream);
                    }

                    eventItem.ThumbnailUrl = "/uploads/events/" + uniqueFileName;
                }

                _context.Add(eventItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Log validation errors for debugging
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            ViewBag.ValidationErrors = errors;

            ViewData["CategoryId"] = new SelectList(_context.Categories.Where(c => c.Type == CategoryType.Event), "Id", "Name", eventItem.CategoryId);
            return View(eventItem);
        }

        // GET: Events/Edit/5
        [Authorize(Roles = "Admin,Lecturer")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }

            // Check if user is owner or admin
            var user = await _userManager.GetUserAsync(User);
            if (!User.IsInRole("Admin") && eventItem.OrganizerId != user?.Id)
            {
                return Forbid();
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories.Where(c => c.Type == CategoryType.Event), "Id", "Name", eventItem.CategoryId);
            return View(eventItem);
        }

        // POST: Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Lecturer")]
        public async Task<IActionResult> Edit(int id, Event eventItem)
        {
            if (id != eventItem.Id)
            {
                return NotFound();
            }

            // Remove validation for navigation properties
            ModelState.Remove("Category");
            ModelState.Remove("Organizer");
            ModelState.Remove("Images");
            ModelState.Remove("Registrations");
            ModelState.Remove("Schedules");

            if (ModelState.IsValid)
            {
                try
                {
                    // Check if user is owner or admin
                    var user = await _userManager.GetUserAsync(User);
                    var existingEvent = await _context.Events.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
                    
                    if (!User.IsInRole("Admin") && existingEvent?.OrganizerId != user?.Id)
                    {
                        return Forbid();
                    }

                    _context.Update(eventItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(eventItem.Id))
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

            ViewData["CategoryId"] = new SelectList(_context.Categories.Where(c => c.Type == CategoryType.Event), "Id", "Name", eventItem.CategoryId);
            return View(eventItem);
        }

        // GET: Events/Delete/5
        [Authorize(Roles = "Admin,Lecturer")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventItem = await _context.Events
                .Include(e => e.Category)
                .Include(e => e.Organizer)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (eventItem == null)
            {
                return NotFound();
            }

            // Check if user is owner or admin
            var user = await _userManager.GetUserAsync(User);
            if (!User.IsInRole("Admin") && eventItem.OrganizerId != user?.Id)
            {
                return Forbid();
            }

            return View(eventItem);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Lecturer")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem != null)
            {
                // Check if user is owner or admin
                var user = await _userManager.GetUserAsync(User);
                if (!User.IsInRole("Admin") && eventItem.OrganizerId != user?.Id)
                {
                    return Forbid();
                }

                _context.Events.Remove(eventItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }

        // POST: Events/Register - Đăng ký sự kiện
        [HttpPost]
        [Authorize(Roles = "Student")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(int eventId)
        {
            var user = await _userManager.GetUserAsync(User);
            var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == user!.Id);

            if (student == null)
            {
                TempData["Error"] = "Không tìm thấy thông tin sinh viên!";
                return RedirectToAction("Details", new { id = eventId });
            }

            var eventItem = await _context.Events
                .Include(e => e.Registrations)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (eventItem == null)
            {
                return NotFound();
            }

            // Check if already registered
            if (eventItem.Registrations.Any(r => r.StudentId == student.Id))
            {
                TempData["Error"] = "Bạn đã đăng ký sự kiện này rồi!";
                return RedirectToAction("Details", new { id = eventId });
            }

            // Check if event is full
            if (eventItem.Registrations.Count >= eventItem.MaxParticipants)
            {
                TempData["Error"] = "Sự kiện đã đủ số lượng đăng ký!";
                return RedirectToAction("Details", new { id = eventId });
            }

            var registration = new EventRegistration
            {
                EventId = eventId,
                StudentId = student.Id,
                RegisteredAt = DateTime.Now,
                Status = RegistrationStatus.Confirmed,
                IsAttended = false
            };

            _context.EventRegistrations.Add(registration);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đăng ký sự kiện thành công!";
            return RedirectToAction("Details", new { id = eventId });
        }

        // POST: Events/Unregister - Hủy đăng ký
        [HttpPost]
        [Authorize(Roles = "Student")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unregister(int eventId)
        {
            var user = await _userManager.GetUserAsync(User);
            var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == user!.Id);

            if (student == null)
            {
                return NotFound();
            }

            var registration = await _context.EventRegistrations
                .FirstOrDefaultAsync(r => r.EventId == eventId && r.StudentId == student.Id);

            if (registration == null)
            {
                TempData["Error"] = "Không tìm thấy đăng ký!";
                return RedirectToAction("Details", new { id = eventId });
            }

            _context.EventRegistrations.Remove(registration);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đã hủy đăng ký thành công!";
            return RedirectToAction("Details", new { id = eventId });
        }

        // GET: Events/MyRegistrations - Danh sách sự kiện đã đăng ký
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> MyRegistrations()
        {
            var user = await _userManager.GetUserAsync(User);
            var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == user!.Id);

            if (student == null)
            {
                return NotFound();
            }

            var registrations = await _context.EventRegistrations
                .Include(r => r.Event)
                    .ThenInclude(e => e.Category)
                .Include(r => r.Event.Organizer)
                .Where(r => r.StudentId == student.Id)
                .OrderByDescending(r => r.RegisteredAt)
                .ToListAsync();

            return View(registrations);
        }

        // GET: Events/Registrations/{id} - Danh sách người đăng ký
        [Authorize(Roles = "Admin,Lecturer")]
        public async Task<IActionResult> Registrations(int id)
        {
            var eventItem = await _context.Events
                .Include(e => e.Category)
                .Include(e => e.Organizer)
                .Include(e => e.Registrations)
                    .ThenInclude(r => r.Student)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (eventItem == null)
            {
                return NotFound();
            }

            // Check if user is owner or admin
            var user = await _userManager.GetUserAsync(User);
            if (!User.IsInRole("Admin") && eventItem.OrganizerId != user?.Id)
            {
                return Forbid();
            }

            return View(eventItem);
        }

        // POST: Events/CheckIn - Điểm danh
        [HttpPost]
        [Authorize(Roles = "Admin,Lecturer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckIn(int registrationId)
        {
            var registration = await _context.EventRegistrations
                .Include(r => r.Event)
                .FirstOrDefaultAsync(r => r.Id == registrationId);

            if (registration == null)
            {
                return NotFound();
            }

            // Check if user is owner or admin
            var user = await _userManager.GetUserAsync(User);
            if (!User.IsInRole("Admin") && registration.Event.OrganizerId != user?.Id)
            {
                return Forbid();
            }

            registration.IsAttended = !registration.IsAttended;
            await _context.SaveChangesAsync();

            TempData["Success"] = registration.IsAttended ? "Đã điểm danh!" : "Đã hủy điểm danh!";
            return RedirectToAction("Registrations", new { id = registration.EventId });
        }
    }
}
