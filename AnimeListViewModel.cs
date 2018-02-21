using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyAnime_updatedDB.Models
{
    public class AnimeListViewModel
    {
        public IEnumerable<SelectListItem> Data { get; set; }
        public Anime SelectedAnime { get; set; }
    }
}