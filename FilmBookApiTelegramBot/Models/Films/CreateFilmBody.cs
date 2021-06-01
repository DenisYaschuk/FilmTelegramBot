using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmApi.Models.Films
{
    public class CreateFilmBody
    {
        public string original_title { get; set; }
        public string genre_ids { get; set; }
        public string overview { get; set; }
        public string release_date { get; set; }
        public float vote_average { get; set; }
        public string poster_path { get; set; }

    }
}
