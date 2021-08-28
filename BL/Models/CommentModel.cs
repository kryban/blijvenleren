using System;

namespace BL.Models
{
    public class CommentModel
    {
        public int Id { get; set; }
        public int LearnResourceId { get; set; }
        public string CommentText { get; set; }
        public DateTime CommentDate { get; set; }
        public string Username { get; set; }
        public CommentStatus Status { get; set; }
    }
}
