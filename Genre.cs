using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyAnime_updatedDB.Models
{
    public class Genre
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Genre Name")]
        public string GenreName { get; set; }
        [DisplayName("Genre Descriptions")]
        public string GenreDescription { get; set; }
    }
}