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

        public SeminarController(ISeminarService seminarService)
        {
            service = seminarService;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var model = await service.GetAllSeminarsAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var categories = await service.GetCategoriesAsync();

            var model = new AddOrEditSeminarFormModel()
            {
                Categories = categories
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddOrEditSeminarFormModel model)
        {
            DateTime dateTime;

            if (!DateTime.TryParseExact(model.DateAndTime, DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
            {
                ModelState.AddModelError(nameof(model.DateAndTime), InvalidDateAndTime);
            }

            var categories = await service.GetCategoriesAsync();

            if (!categories.Any(c => c.Id == model.CategoryId))
            {
                ModelState.AddModelError(nameof(model.CategoryId), InvalidCategory);
            }

            if (!ModelState.IsValid)
            {
                model.Categories = categories;
                return View(model);
            }

            string userId = GetUserId();

            await service.CreateSeminarEntityAsync(userId, model);

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            bool isExisting = await service.IsSeminarExistingAsync(id);

            if (isExisting == false)
            {
                return NotFound();
            }

            var model = await service.CreateDetailedSeminar(id);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            bool isExisting = await service.IsSeminarExistingAsync(id);

            if (isExisting == false)
            {
                return NotFound();
            }

            string userId = GetUserId();

            bool isAuthorized = await service.IsUserAuthorizedAsync(id, userId);

            if (isAuthorized == false)
            {
                return Unauthorized();
            }

            var model = await service.CreateEditModelAsync(id);
            model.Categories = await service.GetCategoriesAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AddOrEditSeminarFormModel model, int id)
        {
            bool isExisting = await service.IsSeminarExistingAsync(id);

            if (isExisting == false)
            {
                return NotFound();
            }

            string userId = GetUserId();

            bool isAuthorized = await service.IsUserAuthorizedAsync(id, userId);

            if (isAuthorized == false)
            {
                return Unauthorized();
            }

            DateTime dateTime;

            if (!DateTime.TryParseExact(model.DateAndTime, DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
            {
                ModelState.AddModelError(nameof(model.DateAndTime), InvalidDateAndTime);
            }

            var categories = await service.GetCategoriesAsync();

            if (!categories.Any(c => c.Id == model.CategoryId))
            {
                ModelState.AddModelError(nameof(model.CategoryId), InvalidCategory);
            }

            if (!ModelState.IsValid)
            {
                model.Categories = categories;
                return View(model);
            }

            await service.EditEntityAsync(id, model);

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Join(int id)
        {
            bool isExisting = await service.IsSeminarExistingAsync(id);

            if (isExisting == false)
            {
                return NotFound();
            }

            string userId = GetUserId();

            await service.JoinSeminarAsync(id, userId);

            return RedirectToAction(nameof(Joined));
        }

        [HttpGet]
        public async Task<IActionResult> Joined()
        {
            string userId = GetUserId();

            var model = await service.GetJoinedSeminarsAsync(userId);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Leave(int id)
        {
            bool isExisting = await service.IsSeminarExistingAsync(id);

            if (isExisting == false)
            {
                return NotFound();
            }

            string userId = GetUserId();

            await service.LeaveSeminarAsync(userId, id);

            return RedirectToAction(nameof(Joined));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            bool isExisting = await service.IsSeminarExistingAsync(id);

            if (isExisting == false)
            {
                return NotFound();
            }

            string userId = GetUserId();

            bool isAuthorized = await service.IsUserAuthorizedAsync(id, userId);

            if (isAuthorized == false)
            {
                return Unauthorized();
            }

            var deleteModel = await service.CreateDeleteModelAsync(id);

            return View(deleteModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool isExisting = await service.IsSeminarExistingAsync(id);

            if (isExisting == false)
            {
                return NotFound();
            }

            string userId = GetUserId();

            bool isAuthorized = await service.IsUserAuthorizedAsync(id, userId);

            if (isAuthorized == false)
            {
                return Unauthorized();
            }

            await service.DeleteSeminarAsync(id);

            return RedirectToAction(nameof(All));
        }
    }
}
