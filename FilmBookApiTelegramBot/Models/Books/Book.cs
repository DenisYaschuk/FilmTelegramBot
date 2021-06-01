using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmApi.Models.Book
{
    public class Book
    {
        public string url { get; set; }
        public string title { get; set; }
        public string by_statement { get; set; }
        public string publish_date { get; set; }
        public Indentifier identifiers { get; set; }
        public List<Author> authors { get; set; }
    }
    public class Indentifier
    {
        public string[] isbn_10 { get; set; }
    }
    public class Author
    {
        public string name { get; set; }
    }
}
