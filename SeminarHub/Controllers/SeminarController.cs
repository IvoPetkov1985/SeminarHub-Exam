using Microsoft.AspNetCore.Mvc;
using SeminarHub.Contracts;
using SeminarHub.Models;
using System.Globalization;
using static SeminarHub.Data.Common.DataConstants;

namespace SeminarHub.Controllers
{
    public class SeminarController : BaseController
    {
        private readonly ISeminarService service;

        public SeminarController(ISeminarService _service)
        {
            service = _service;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            IEnumerable<SeminarAllViewModel> model = await service.GetAllSeminarsAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            IEnumerable<CategoryViewModel> categories = await service.GetAllCategoriesAsync();

            SeminarFormModel model = new()
            {
                Categories = categories
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(SeminarFormModel model)
        {
            if (DateTime.TryParseExact(model.DateAndTime, DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime resultDateAndTime) == false)
            {
                ModelState.AddModelError(nameof(model.DateAndTime), DateTimeNotValidMsg);
            }

            IEnumerable<CategoryViewModel> categories = await service.GetAllCategoriesAsync();

            if (categories.Any(c => c.Id == model.CategoryId) == false)
            {
                ModelState.AddModelError(nameof(model.CategoryId), CategoryNotValidMsg);
            }

            if (ModelState.IsValid == false)
            {
                model.Categories = categories;

                return View(model);
            }

            string userId = GetUserId();

            await service.CreateNewSeminarAsync(model, userId);

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (await service.IsSeminarExistingAsync(id) == false)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            if (await service.IsUserAuthorizedAsync(userId, id) == false)
            {
                return Unauthorized();
            }

            SeminarFormModel model = await service.CreateEditModelAsync(id);

            IEnumerable<CategoryViewModel> categories = await service.GetAllCategoriesAsync();

            model.Categories = categories;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SeminarFormModel model, int id)
        {
            if (await service.IsSeminarExistingAsync(id) == false)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            if (await service.IsUserAuthorizedAsync(userId, id) == false)
            {
                return Unauthorized();
            }

            if (DateTime.TryParseExact(model.DateAndTime, DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime resultDateAndTime) == false)
            {
                ModelState.AddModelError(nameof(model.DateAndTime), DateTimeNotValidMsg);
            }

            IEnumerable<CategoryViewModel> categories = await service.GetAllCategoriesAsync();

            if (categories.Any(c => c.Id == model.CategoryId) == false)
            {
                ModelState.AddModelError(nameof(model.CategoryId), CategoryNotValidMsg);
            }

            if (ModelState.IsValid == false)
            {
                model.Categories = categories;

                return View(model);
            }

            await service.EditSeminarAsync(model, id);

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (await service.IsSeminarExistingAsync(id) == false)
            {
                return BadRequest();
            }

            SeminarDetailsViewModel model = await service.CreateDetailsModelAsync(id);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Joined()
        {
            string userId = GetUserId();

            IEnumerable<SeminarAllViewModel> model = await service.GetAllJoinedSeminarsAsync(userId);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Join(int id)
        {
            if (await service.IsSeminarExistingAsync(id) == false)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            if (await service.IsSeminarAlreadyJoinedAsync(userId, id))
            {
                return RedirectToAction(nameof(All));
            }

            await service.JoinSeminarAsync(userId, id);

            return RedirectToAction(nameof(Joined));
        }

        [HttpPost]
        public async Task<IActionResult> Leave(int id)
        {
            if (await service.IsSeminarExistingAsync(id) == false)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            await service.LeaveSeminarAsync(userId, id);

            return RedirectToAction(nameof(Joined));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (await service.IsSeminarExistingAsync(id) == false)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            if (await service.IsUserAuthorizedAsync(userId, id) == false)
            {
                return Unauthorized();
            }

            SeminarDeleteViewModel model = await service.CreateDeleteModelAsync(id);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(SeminarDeleteViewModel model, int id)
        {
            if (await service.IsSeminarExistingAsync(id) == false)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            if (await service.IsUserAuthorizedAsync(userId, id) == false)
            {
                return Unauthorized();
            }

            await service.RemoveSeminarAsync(model.Id);

            return RedirectToAction(nameof(All));
        }
    }
}
