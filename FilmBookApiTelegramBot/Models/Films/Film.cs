using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmApi.Models.Films
{
    public class Film
    {
        public List<FilmResults> results { get; set; }
    }
    public class FilmResults
    {
        public string backdrop_path { get; set; }
        public int[] genre_ids { get; set; }
        public int id { get; set; }
        public string original_title { get; set; }
        public string overview { get; set; }
        public string poster_path { get; set; }
        public string release_date { get; set; }
        public float vote_average { get; set; }
    }
}
