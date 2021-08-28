using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlijvenLeren.Models
{
    public class LearnResourcesViewModel
    {
        public LearnResource LearnResource{ get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
