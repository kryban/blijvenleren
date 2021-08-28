using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BL.Data;
using BL.Models;

namespace BL.Controllers
{
    public class LearnResourceController : Controller
    {
        private readonly BLContext _context;

        public LearnResourceController(BLContext context)
        {
            _context = context;
        }

        // GET: LearnResource
        public async Task<IActionResult> Index()
        {
            return View(await _context.LearnResourceModel.ToListAsync());
        }

        // GET: LearnResource/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learnResourceModel = await _context.LearnResourceModel
                .FirstOrDefaultAsync(m => m.Id == id);

            learnResourceModel.Comments = _context.CommentModel.Where(c => c.LearnResourceId == id).ToList();

            if (learnResourceModel == null)
            {
                return NotFound();
            }

            return View(learnResourceModel);
        }

        // GET: LearnResource/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LearnResource/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Link")] LearnResourceModel learnResourceModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(learnResourceModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(learnResourceModel);
        }

        // GET: LearnResource/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learnResourceModel = await _context.LearnResourceModel.FindAsync(id);
            if (learnResourceModel == null)
            {
                return NotFound();
            }
            return View(learnResourceModel);
        }

        public IActionResult AddComment(int id)
        {
            return View(new CommentModel() { LearnResourceId = id });
        }

        public async Task<IActionResult> SaveComment([Bind("LearnResourceId, CommentText, Username")] CommentModel commentModel)
        {
            var newComment = commentModel;
            newComment.CommentDate = DateTime.Now;
            newComment.Status = CommentStatus.Approved;

            var learnResourceModel = await _context.LearnResourceModel.FindAsync(commentModel.LearnResourceId);
            learnResourceModel.Comments = _context.CommentModel.Where(c => c.Id == commentModel.LearnResourceId).ToList();
            learnResourceModel.Comments.Add(newComment);

            _context.Update(learnResourceModel);
            await _context.SaveChangesAsync();

            return View("Details");
        }

        // POST: LearnResource/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Link")] LearnResourceModel learnResourceModel)
        {
            if (id != learnResourceModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(learnResourceModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LearnResourceModelExists(learnResourceModel.Id))
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
            return View(learnResourceModel);
        }

        // GET: LearnResource/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learnResourceModel = await _context.LearnResourceModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (learnResourceModel == null)
            {
                return NotFound();
            }

            return View(learnResourceModel);
        }

        // POST: LearnResource/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var learnResourceModel = await _context.LearnResourceModel.FindAsync(id);
            _context.LearnResourceModel.Remove(learnResourceModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LearnResourceModelExists(int id)
        {
            return _context.LearnResourceModel.Any(e => e.Id == id);
        }
    }
}
