using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyAnime_updatedDB.Models
{
    public class AnimeList
    {
        public int ID { get; set; }
        
        [Required]
        [ForeignKey("Anime")]
        public int AnimeId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        
        public virtual Anime Anime { get; set; }

        public virtual ApplicationUser User { get; set; }
        
    }
}