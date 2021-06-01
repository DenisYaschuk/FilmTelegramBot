using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FilmApi.Models.Favourite
{
    public class FavouriteBook
    {
        [Key]
        public int id { get; set; }
        public string ChatId { get; set; }
        public string book_id { get; set; }
    }
}
