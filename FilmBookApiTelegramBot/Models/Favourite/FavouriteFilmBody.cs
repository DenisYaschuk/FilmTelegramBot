using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmApi.Models.Favourite
{
    public class FavouriteFilmBody
    {
        public string ChatId { get; set; }
        public int film_id { get; set; }
    }
}
