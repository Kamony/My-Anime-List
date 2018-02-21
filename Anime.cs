using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyAnime_updatedDB.Models
{
    public class Anime
    {
        public int ID { get; set; }

        [Required]
        [DisplayName("Anime Name")]
        public string AnimeName { get; set; }

        [Required]
        [ForeignKey("Genre")]
        public int GenreId { get; set; }
        
        public string Description { get; set; }
        [DisplayName("Aired in")]
        [Range(1940, 2017)]
        public int Year { get; set; }
        [Range(1, 2000)]
        public int Episodes { get; set; }
        
        public bool Finished { get; set; }

        public string Image { get; set; }

        public virtual Genre Genre { get; set; }
    }
}