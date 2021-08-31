using BlijvenLeren.Models;
using BlijvenLeren.Repository;
using BlijvenLeren.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlijvenLeren.Controllers
{
    [Authorize]
    public class LearnResourcesController : Controller
    {
        private readonly IBlijvenLerenRepository _repo;
        private readonly IUserService _userService;

        public LearnResourcesController(IBlijvenLerenRepository repo, IUserService userService)
        {
            _repo = repo;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _repo.GetAllLearnResources());
        }

        [HttpGet]
        [Route("api/learnresource/{id}")]
        public LearnResource Get(int id)
        {
            return _repo.GetLearnResourceDetails((int)id).Result.LearnResource;
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            LearnResourcesViewModel vm = _repo.GetLearnResourceDetails((int)id).Result;

            if (vm.LearnResource == null)
            {
                return NotFound();
            }
            
            return View(vm);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LearnResourceId,Title,Description,Link")] LearnResource learnResource)
        {
            if (ModelState.IsValid)
            {
                await _repo.AddLearnResource(learnResource);

                return RedirectToAction(nameof(Index));
            }
            return View(learnResource);
        }

        public IActionResult NewComment(int id)
        {
            return View(new Comment() { LearnResourceId = id, Status = BepaalStatusOpBasisVanUserRol() });
        }

        private CommentStatus BepaalStatusOpBasisVanUserRol()
        {
            var status = User.IsInRole(BlijvenLerenRole.Intern) ? CommentStatus.Approved : CommentStatus.InReview;

            return status;
        }

        public async Task<IActionResult> AddComment([Bind("LearnResourceId, Status, CommentText")] Comment newComment)
        {
            await _repo.AddComment(newComment);

            return RedirectToAction("Details", new { id = newComment.LearnResourceId });
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learnResource = await _repo.GetLearnResource((int)id);
            if (learnResource == null)
            {
                return NotFound();
            }
            return View(learnResource);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LearnResourceId,Title,Description,Link")] LearnResource learnResource)
        {
            if (id != learnResource.LearnResourceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _repo.UpdateLearnResource(learnResource);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_repo.LearnResourceExists(learnResource.LearnResourceId))
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
            return View(learnResource);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learnResource = await _repo.GetLearnResource((int)id);

            if (learnResource == null)
            {
                return NotFound();
            }

            return View(learnResource);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repo.DeleteLearnResource(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
