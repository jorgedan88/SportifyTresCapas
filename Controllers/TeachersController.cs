using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sportify_back.Models;

namespace Sportify_Back.Controllers
{
    public class TeachersController : Controller
    {
        private readonly SportifyDbContext _context;

        public TeachersController(SportifyDbContext context)
        {
            _context = context;
        }

        [Authorize(Policy = "AdministradorOnly")]
        // GET: Teachers
        public async Task<IActionResult> Index()
        {
            var sportifyDbContext = _context.Teachers.Include(t => t.Activities);
            return View(await sportifyDbContext.ToListAsync());
        }

        [Authorize(Policy = "AdministradorOnly")]
        // GET: Teachers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teachers = await _context.Teachers
                .Include(t => t.Activities)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teachers == null)
            {
                return NotFound();
            }

            //ViewData["Activities"] = new SelectList(_context.Activities, "Id", "NameActivity");
            return View(teachers);
        }

        // GET: Teachers/Create
       
        [Authorize(Policy = "AdministradorOnly")]
        public IActionResult Create()
        {
            ViewData["ActivitiesId"] = new SelectList(_context.Activities, "Id", "NameActivity");
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Policy = "AdministradorOnly")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Dni,Mail,Phone,Address,ActivitiesId,Active")] Teachers teachers)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teachers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
           ViewData["ActivitiesId"] = new SelectList(_context.Activities, "Id", "NameActivity",  teachers.ActivitiesId);
            return View(teachers);
        }


        [Authorize(Policy = "AdministradorOnly")]
        // GET: Teachers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teachers = await _context.Teachers.FindAsync(id);
            if (teachers == null)
            {
                return NotFound();
            }
            ViewData["ActivitiesId"] = new SelectList(_context.Activities, "Id", "NameActivity", teachers.ActivitiesId);
            return View(teachers);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Policy = "AdministradorOnly")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Dni,Mail,Phone,Address,ActivitiesId,Active")] Teachers teachers)
        {
            if (id != teachers.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teachers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeachersExists(teachers.Id))
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
            ViewData["ActivitiesId"] = new SelectList(_context.Activities, "Id", "NameActivity", teachers.ActivitiesId);
            return View(teachers);
        }

        // GET: Teachers/Delete/5
        [Authorize(Policy = "AdministradorOnly")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teachers = await _context.Teachers
                .Include(t => t.Activities)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teachers == null)
            {
                return NotFound();
            }

            return View(teachers);
        }
        // POST: Teachers/Delete/5
        [Authorize(Policy = "AdministradorOnly")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teachers = await _context.Teachers.FindAsync(id);
            if (teachers != null)
            {
                _context.Teachers.Remove(teachers);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeachersExists(int id)
        {
            return _context.Teachers.Any(e => e.Id == id);
        }
    }
}
