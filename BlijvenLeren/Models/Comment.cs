using System;
using System.ComponentModel.DataAnnotations;

namespace BlijvenLeren.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        [Required]
        public string CommentText { get; set; }
        public DateTime CommentDate { get; set; }
        public string Username { get; set; }
        public CommentStatus Status { get; set; }
        public int LearnResourceId { get; set; }
        public virtual LearnResource LearnResource { get; set;}
    }
}
