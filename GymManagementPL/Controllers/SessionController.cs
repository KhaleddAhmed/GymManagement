using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        #region Get All Sessions
        public IActionResult Index()
        {
            var sessions = _sessionService.GetAllSessions();
            return View(sessions);
        }
        #endregion

        #region Create Session
        public IActionResult Create()
        {
            LoadCategoriesDropDown();
            LoadTrainersDropDown();
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateSessionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                LoadCategoriesDropDown();
                LoadTrainersDropDown();
                return View(model);
            }

            var result = _sessionService.CreateSession(model);

            if (result)
            {
                TempData["SuccessMessage"] = "Session created successfully!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError(
                    "",
                    "Failed to create session. Please verify trainer and category exist."
                );
                LoadCategoriesDropDown();
                LoadTrainersDropDown();
                return View(model);
            }
        }
        #endregion

        #region Session Details
        public IActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id";
                return RedirectToAction(nameof(Index));
            }
            var session = _sessionService.GetSessionById(id);

            if (session == null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(session);
        }
        #endregion

        #region Edit Session
        public IActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id";
                return RedirectToAction(nameof(Index));
            }
            var session = _sessionService.GetSessionToUpdate(id);

            if (session == null)
            {
                TempData["ErrorMessage"] = "Session cannot be updated";
                return RedirectToAction(nameof(Index));
            }

            LoadTrainersDropDown();
            return View(session);
        }

        [HttpPost]
        public IActionResult Edit(int id, UpdateSessionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                LoadTrainersDropDown();
                return View(model);
            }
            var result = _sessionService.UpdateSession(id, model);

            if (result)
            {
                TempData["SuccessMessage"] = "Session updated successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update session.";
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete Session
        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id";
                return RedirectToAction(nameof(Index));
            }
            var session = _sessionService.GetSessionById(id);

            if (session == null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.SessionId = session.Id;
            return View();
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var result = _sessionService.RemoveSession(id);

            if (result)
            {
                TempData["SuccessMessage"] = "Session deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Session cannot be deleted";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Helper Methods
        public void LoadCategoriesDropDown()
        {
            var Categories = _sessionService.GetCategoriesForDropDown();
            ViewBag.Categories = new SelectList(Categories, "Id", "Name");
        }

        public void LoadTrainersDropDown()
        {
            var trainers = _sessionService.GetTrainersForDropDown();
            ViewBag.Trainers = new SelectList(trainers, "Id", "Name");
        }
        #endregion
    }
}
