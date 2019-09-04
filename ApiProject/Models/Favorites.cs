using System;
using System.Collections.Generic;

namespace ApiProject.Models
{
    public partial class Favorites
    {
        public int Id { get; set; }
        public string MovieId { get; set; }
        public string UserId { get; set; }

        public virtual AspNetUsers User { get; set; }
    }
}
