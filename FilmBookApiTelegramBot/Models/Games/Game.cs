using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmApi.Models.Games
{
    public class Game
    {
        public List<GameResults> results { get; set; }
    }

    public class GameResults
    {
        public int id { get; set; }
        public string name { get; set; }
        public string released { get; set; }
        public string background_image { get; set; }
        public List<GameGenres> genres { get; set; }
        public float rating { get; set; }

    }
    public class GameGenres
    {
        public int id { get; set; }
    }
}
