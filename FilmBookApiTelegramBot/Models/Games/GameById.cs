using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmApi.Models.Games
{
    public class GameById
    {
        public int id { get; set; }
        public string name { get; set; }
        public string released { get; set; }
        public string background_image { get; set; }
        public List<GameGenres> genres { get; set; }
        public float rating { get; set; }

    }
}
