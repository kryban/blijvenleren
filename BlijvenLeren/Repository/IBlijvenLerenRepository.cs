using BlijvenLeren.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlijvenLeren.Repository
{
    public interface IBlijvenLerenRepository
    {
        Task<List<LearnResource>> GetAllLearnResources();
        Task<LearnResourcesViewModel> GetLearnResourceDetails(int id);
        Task<LearnResource> GetLearnResource(int id);
        Task AddLearnResource(LearnResource learnResource);
        Task AddComment(Comment comment);
        Task UpdateLearnResource(LearnResource learnResource);
        bool LearnResourceExists(int id);
        Task DeleteLearnResource(int id);
        IEnumerable<Comment> GetCommentsForReview();
        void ApproveComment(int id);
        void DeleteComment(int id);
    }
}