using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FilmApi.Models.Games
{
    public class GameResponse
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string released { get; set; }
        public string background_image { get; set; }
        public float rating { get; set; }
        public string genres { get; set; }
    }
}
