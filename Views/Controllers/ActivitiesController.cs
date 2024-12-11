using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sportify_back.Models;
using Sportify_Back.Services;
using System.Threading.Tasks;

namespace Sportify_Back.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly IActivityService _activityService;

        public ActivitiesController(IActivityService activityService)
        {
            _activityService = activityService;
        }

        public async Task<IActionResult> Index()
        {
            var activities = await _activityService.GetAllAsync();
            return View(activities);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var activity = await _activityService.GetByIdAsync(id.Value);
            if (activity == null) return NotFound();

            return View(activity);
        }

        [Authorize(Policy = "AdministradorOnly")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Policy = "AdministradorOnly")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NameActivity,Description,Active")] Activities activity)
        {
            if (ModelState.IsValid)
            {
                await _activityService.CreateAsync(activity);
                return RedirectToAction(nameof(Index));
            }

            return View(activity);
        }

        [Authorize(Policy = "AdministradorOnly")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var activity = await _activityService.GetByIdAsync(id.Value);
            if (activity == null) return NotFound();

            return View(activity);
        }

        [Authorize(Policy = "AdministradorOnly")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameActivity,Description,Active")] Activities activity)
        {
            if (id != activity.Id) return NotFound();

            if (ModelState.IsValid)
            {
                await _activityService.UpdateAsync(activity);
                return RedirectToAction(nameof(Index));
            }

            return View(activity);
        }

        [Authorize(Policy = "AdministradorOnly")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var activity = await _activityService.GetByIdAsync(id.Value);
            if (activity == null) return NotFound();

            return View(activity);
        }

        [Authorize(Policy = "AdministradorOnly")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _activityService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
