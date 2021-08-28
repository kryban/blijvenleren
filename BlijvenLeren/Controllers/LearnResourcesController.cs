using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlijvenLeren.Data;
using BlijvenLeren.Models;

namespace BlijvenLeren.Controllers
{
    public class LearnResourcesController : Controller
    {
        private readonly BlijvenLerenContext _context;

        public LearnResourcesController(BlijvenLerenContext context)
        {
            _context = context;
        }

        // GET: LearnResources
        public async Task<IActionResult> Index()
        {
            return View(await _context.LearnResource.ToListAsync());
        }

        // GET: LearnResources/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var vm = new LearnResourcesViewModel();

            if (id == null)
            {
                return NotFound();
            }

            vm.LearnResource = await _context.LearnResource
                .FirstOrDefaultAsync(m => m.LearnResourceId == id);

            if (vm.LearnResource == null)
            {
                return NotFound();
            }

            vm.Comments = _context.Comment.Where(m => m.LearnResourceId == id).ToList();

            //var learnResource = await _context.LearnResource
            //    .FirstOrDefaultAsync(m => m.LearnResourceId == id);
            
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
                _context.Add(learnResource);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(learnResource);
        }

        public IActionResult AddComment(int id)
        {
            return View(new Comment() { LearnResourceId = id });
        }

        public async Task<IActionResult> SaveComment([Bind("LearnResourceId, CommentText, Username")] Comment commentModel)
        {
            var newComment = commentModel;
            newComment.CommentDate = DateTime.Now;
            newComment.Status = CommentStatus.Approved;

            _context.Comment.Add(newComment); 
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = commentModel.LearnResourceId });
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
                    _context.Update(learnResource);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LearnResourceExists(learnResource.LearnResourceId))
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

            var learnResource = await _context.LearnResource
                .FirstOrDefaultAsync(m => m.LearnResourceId == id);
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
            var learnResource = await _context.LearnResource.FindAsync(id);
            _context.LearnResource.Remove(learnResource);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LearnResourceExists(int id)
        {
            return _context.LearnResource.Any(e => e.LearnResourceId == id);
        }
    }
}
