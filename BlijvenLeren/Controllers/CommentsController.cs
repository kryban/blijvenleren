using BlijvenLeren.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlijvenLeren.Controllers
{
    public class CommentsController : Controller
    {
        IBlijvenLerenRepository _repo;

        public CommentsController(IBlijvenLerenRepository repo)
        {
            _repo = repo;
        }

        public ActionResult Index()
        {
            return View(_repo.GetCommentsForReview());
        }

        // GET: CommentsController/Edit/5
        public ActionResult Edit(int id)
        {
            _repo.ApproveComment(id);
            return View("Index");
        }

        // POST: CommentsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CommentsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CommentsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
