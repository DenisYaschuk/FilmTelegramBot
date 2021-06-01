using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FilmApi.Models.Favourite
{
    public class FavouriteGame
    {
        [Key]
        public int id { get; set; }
        public string ChatId { get; set; }
        public int game_id { get; set; }
    }
}
