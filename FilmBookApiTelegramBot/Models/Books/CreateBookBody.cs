using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmApi.Models.Book
{
    public class CreateBookBody
    {
        public string isbn_10 { get; set; }
        public string url { get; set; }
        public string title { get; set; }
        public string authors { get; set; }
        public string publish_date { get; set; }
    }
}
