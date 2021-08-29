using BlijvenLeren.Data;
using BlijvenLeren.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlijvenLeren.Repository
{
    public class BlijvenLerenRepository : IBlijvenLerenRepository
    {
        private readonly BlijvenLerenContext _context;

        public BlijvenLerenRepository(BlijvenLerenContext context)
        {
            _context = context;
        }

        public async Task<List<LearnResource>> GetAllLearnResources()
        {          
            return await _context.LearnResource.ToListAsync();
        }

        public async Task<LearnResourcesViewModel> GetLearnResourceDetails(int id)
        {
            var retval = new LearnResourcesViewModel();

            retval.LearnResource = await _context.LearnResource.FirstOrDefaultAsync(m => m.LearnResourceId == id);

            if(retval.LearnResource == null)
            {
                return retval;
            }

            retval.Comments = _context.Comment.Where(m => m.LearnResourceId == id).ToList();

            return retval;
        }

        public async Task<LearnResource> GetLearnResource(int id)
        {
            var retval =  await _context.LearnResource.FirstOrDefaultAsync(m => m.LearnResourceId == id);
            return retval;
        }

        public async Task AddLearnResource(LearnResource learnResource)
        {
            _context.Add(learnResource);
            await _context.SaveChangesAsync();
        }

        public async Task AddComment(Comment comment)
        {
            comment.CommentDate = DateTime.Now;
            comment.Status = CommentStatus.Approved;

            _context.Comment.Add(comment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateLearnResource(LearnResource learnResource)
        {
            _context.Update(learnResource);
            await _context.SaveChangesAsync();
        }

        public bool LearnResourceExists(int id)
        {
            return _context.LearnResource.Any(e => e.LearnResourceId == id);
        }

        public async Task DeleteLearnResource(int id)
        {
            var learnResource = await GetLearnResource(id);

            _context.LearnResource.Remove(learnResource);
            await _context.SaveChangesAsync();
        }
    }
}
