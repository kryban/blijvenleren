using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlijvenLeren.Models
{
    public class LearnResource
    {
        public int LearnResourceId { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }

        public string Link { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
