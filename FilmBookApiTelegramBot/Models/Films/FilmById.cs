using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmApi.Models.Films
{
    public class FilmById
    {
        public int id { get; set; }
        public string backdrop_path { get; set; }
        public List<FilmGenres> genres { get; set; } 
        public string original_title { get; set; }
        public string overview { get; set; }
        public string poster_path { get; set; }
        public string release_date { get; set; }
        public float vote_average { get; set; }
    }
    public class FilmGenres
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}
