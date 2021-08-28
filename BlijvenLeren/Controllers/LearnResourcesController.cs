using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlijvenLeren.Data;
using BlijvenLeren.Models;
using BlijvenLeren.Repository;

namespace BlijvenLeren.Controllers
{
    public class LearnResourcesController : Controller
    {
        private readonly BlijvenLerenContext _context;
        private readonly IBlijvenLerenRepository _repo;

        public LearnResourcesController(BlijvenLerenContext context, IBlijvenLerenRepository repo)
        {
            _context = context;
            _repo = repo;
        }

        // GET: LearnResources
        public async Task<IActionResult> Index()
        {
            return View(await _repo.GetAllLearnResources()); // View(await _context.LearnResource.ToListAsync());
        }

        // GET: LearnResources/Details/5
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

        // GET: LearnResources/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LearnResources/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            return View(new Comment() { LearnResourceId = id });
        }

        public async Task<IActionResult> AddComment([Bind("LearnResourceId, CommentText, Username")] Comment newComment)
        {
            await _repo.AddComment(newComment);

            return RedirectToAction("Details", new { id = newComment.LearnResourceId });
        }

        // GET: LearnResources/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learnResource = await _context.LearnResource.FindAsync(id);
            if (learnResource == null)
            {
                return NotFound();
            }
            return View(learnResource);
        }

        // POST: LearnResources/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: LearnResources/Delete/5
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

        // POST: LearnResources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repo.DeleteLearnResource(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
