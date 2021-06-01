using FilmApi.Models.Book;
using FilmApi.Models.Favourite;
using FilmApi.Models.Films;
using FilmApi.Models.Games;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FilmBookApiTelegramBot
{
    public class ApiClient
    {
        private readonly HttpClient _client = new HttpClient();
        public ApiClient() {
            _client.BaseAddress = new Uri(Constants.ApiAddress);
        }
            
        
        public async Task<FilmResponse> GetRecomendationForFilms(string chatId)
        {

            var favouriteFilms = await this.GetFavouriteFilms(chatId);
            string genres = "";
            foreach(var favouriteFilm in favouriteFilms)
            {
                var film = await this.GetFilmById(favouriteFilm.film_id);
                genres += film.genre_ids+",";
            }
            string[] genres_id = genres.Split(",");
            var sList = new List<String>();

            for (int i = 0; i < genres_id.Length; i++)
            {
                sList.Add(genres_id[i]);
            }

            var favouriteGenre = sList.GroupBy(i => i).OrderByDescending(grp => grp.Count()).Select(grp => grp.Key).First();
            var responce = await _client.GetAsync($"Films/GetFilmByGenre/{favouriteGenre}");
            responce.EnsureSuccessStatusCode();
            var content = responce.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<FilmResponse>(content);

            string[] geners = result.genre_ids.Split(",");

            string geners_to_string = "";
            foreach (var i in geners)
            {
                geners_to_string += await this.GetFilmGenreById(Int32.Parse(i)) + ", ";
            }
            result.genre_ids = geners_to_string.Substring(0, geners_to_string.Length - 2);

            return result;
        }
        public async Task<GameResponse> GetRecomendationForGames(string chatId)
        {

            var favouriteGames= await this.GetFavouriteGames(chatId);
            string genres = "";
            foreach (var favouriteGame in favouriteGames)
            {
                var game = await this.GetGameById(favouriteGame.game_id);
                genres += game.genres + ",";
            }
            string[] genres_id = genres.Split(",");
            var genresList = new List<String>();

            for (int i = 0; i < genres_id.Length; i++)
            {
                genresList.Add(genres_id[i]);
            }

            var favouriteGenre = genresList.GroupBy(i => i).OrderByDescending(grp => grp.Count()).Select(grp => grp.Key).First();
            var responce = await _client.GetAsync($"Games/GetGameByGenre/{favouriteGenre}");
            responce.EnsureSuccessStatusCode();
            var content = responce.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<GameResponse>(content);
            if (result.genres != "")
            {
                string[] geners = result.genres.Split(",");

                string geners_to_string = "";
                foreach (var i in geners)
                {
                    geners_to_string += await this.GetGameGenreById(Int32.Parse(i)) + ", ";
                }
                result.genres = geners_to_string.Substring(0, geners_to_string.Length - 2);
            }

            return result;
        }




        public async Task<IEnumerable<FavouriteFilm>> GetFavouriteFilms(string chatId)
        {
            var responce = await _client.GetAsync($"Favourites/GetAllFavouriteFilms/{chatId}");
            responce.EnsureSuccessStatusCode();
            var content = responce.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<List<FavouriteFilm>>(content);
            return result;
        }
        public async Task<IEnumerable<FavouriteBook>> GetFavouriteBooks(string chatId)
        {

            var responce = await _client.GetAsync($"Favourites/GetAllFavouriteBooks/{chatId}");
            responce.EnsureSuccessStatusCode();
            var content = responce.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<List<FavouriteBook>>(content);
            return result;
        }
        public async Task<IEnumerable<FavouriteGame>> GetFavouriteGames(string chatId)
        {

            var responce = await _client.GetAsync($"Favourites/GetAllFavouriteGames/{chatId}");
            responce.EnsureSuccessStatusCode();
            var content = responce.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<List<FavouriteGame>>(content);
            return result;
        }

        public async Task<String> AddBookToFavourite(string chatId, string isbn)
        {

            var FavouriteBook = new FavouriteBookBody
            {
                ChatId = chatId,
                book_id = isbn
            };
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent(JsonConvert.SerializeObject(FavouriteBook), Encoding.UTF8, "application/json");
            var responce = await _client.PostAsync($"Favourites/createFavouriteBook", content);
            responce.EnsureSuccessStatusCode();

            return "Added to your favourites.";
        }
        public async Task<String> AddGameToFavourite(string chatId, int game_id)
        {

            var FavouriteGame= new FavouriteGameBody
            {
                ChatId = chatId,
                game_id = game_id
            };
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent(JsonConvert.SerializeObject(FavouriteGame), Encoding.UTF8, "application/json");
            var responce = await _client.PostAsync($"Favourites/createFavouriteGame", content);
            responce.EnsureSuccessStatusCode();

            return "Added to your favourites.";
        }
        public async Task<String> AddFilmToFavourite(string chatId, int film_id)
        {

            var FavouriteFilm= new FavouriteFilmBody
            {
                ChatId = chatId,
                film_id = film_id
            };
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent(JsonConvert.SerializeObject(FavouriteFilm), Encoding.UTF8, "application/json");
            var responce = await _client.PostAsync($"Favourites/createFavouriteFilm", content);
            responce.EnsureSuccessStatusCode();

            return "Added to your favourites.";
        }
        public async Task<String> RemoveFilmFromFavourite(string chatId, int film_id)
        {

            var FavouriteFilm = new FavouriteFilmBody
            {
                ChatId = chatId,
                film_id = film_id
            };
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(Constants.ApiAddress+"Favourites/deleteFavouriteFilm"),
                Content = new StringContent(JsonConvert.SerializeObject(FavouriteFilm), Encoding.UTF8, "application/json")
            };
            var responce = await _client.SendAsync(request);
            responce.EnsureSuccessStatusCode();
            return "Removed from your favourite.";
        }
        public async Task<String> RemoveGameFromFavourite(string chatId, int game_id)
        {

            var FavouriteGame = new FavouriteGameBody
            {
                ChatId = chatId,
                game_id = game_id
            };
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(Constants.ApiAddress + "Favourites/deleteFavouriteGame"),
                Content = new StringContent(JsonConvert.SerializeObject(FavouriteGame), Encoding.UTF8, "application/json")
            };
            var responce = await _client.SendAsync(request);
            responce.EnsureSuccessStatusCode();
            return "Removed from your favourite.";
        }
        public async Task<String> RemoveBookFromFavourite(string chatId, string isbn)
        {

            var FavouriteBook = new FavouriteBookBody
            {
                ChatId = chatId,
                book_id = isbn
            };
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(Constants.ApiAddress + "Favourites/deleteFavouriteBook"),
                Content = new StringContent(JsonConvert.SerializeObject(FavouriteBook), Encoding.UTF8, "application/json")
            };
            var responce = await _client.SendAsync(request);
            responce.EnsureSuccessStatusCode();
            return "Removed from your favourite.";
        }



        public async Task<String> GetFilmGenreById(int id)
        {

            var responce = await _client.GetAsync($"Films/GetFilmGenre/{id}");
            responce.EnsureSuccessStatusCode();
            var result = responce.Content.ReadAsStringAsync().Result;
            Console.WriteLine(result);
            return result;
        }
        public async Task<String> GetGameGenreById(int id)
        {

            var responce = await _client.GetAsync($"Games/GetGameGenre/{id}");
            responce.EnsureSuccessStatusCode();
            var result = responce.Content.ReadAsStringAsync().Result;
            return result;
        }
        public async Task<String> GetFilmConnectdGameIdById(int id)
        {

            var responce = await _client.GetAsync($"Films/GetConnectedGamesIds/{id}");
            responce.EnsureSuccessStatusCode();
            var result = responce.Content.ReadAsStringAsync().Result;
            return result;
        }
        public async Task<String> GetFilmConnectdBookIdById(int id)
        {

            var responce = await _client.GetAsync($"Films/GetConnectedBooksIds/{id}");
            responce.EnsureSuccessStatusCode();
            var result = responce.Content.ReadAsStringAsync().Result;
            return result;
        }
        public async Task<String> GetGameConnectdFilmIdById(int id)
        {

            var responce = await _client.GetAsync($"Games/GetConnectedFilmsIds/{id}");
            responce.EnsureSuccessStatusCode();
            var result = responce.Content.ReadAsStringAsync().Result;
            return result;
        }
        public async Task<String> GetGameConnectdBookIdById(int id)
        {

            var responce = await _client.GetAsync($"Games/GetConnectedBooksIds/{id}");
            responce.EnsureSuccessStatusCode();
            var result = responce.Content.ReadAsStringAsync().Result;
            return result;
        }
        public async Task<String> GetBookConnectdFilmIdById(string isbn)
        {

            var responce = await _client.GetAsync($"Books/GetConnectedFilmsIds/{isbn}");
            responce.EnsureSuccessStatusCode();
            var result = responce.Content.ReadAsStringAsync().Result;
            return result;
        }
        public async Task<String> GetBookConnectdGameIdById(string isbn)
        {

            var responce = await _client.GetAsync($"Books/GetConnectedGamesIds/{isbn}");
            responce.EnsureSuccessStatusCode();
            var result = responce.Content.ReadAsStringAsync().Result;
            return result;
        }
        public async Task<FilmResponse> GetFilmById(int id)
        {

            var responce = await _client.GetAsync($"Films/getFilmByIdExternal/{id}");

            responce.EnsureSuccessStatusCode();
            var content = responce.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<FilmResponse>(content);

            return result;
        }

        public async Task<GameResponse> GetGameById(int id)
        {
            var responce = await _client.GetAsync($"Games/getGameByIdExternal/{id}");

            responce.EnsureSuccessStatusCode();
            var content = responce.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<GameResponse>(content);

            return result;
        }
        public async Task<BookResponse> GetBookById(string isbn)
        {

            var responce = await _client.GetAsync($"Books/getBookByISBNExternal/{isbn}");

            responce.EnsureSuccessStatusCode();
            var content = responce.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<BookResponse>(content);

            return result;
        }

        public async Task<FilmResponse> GetFilmByName(string filmName)
        {

            var responce = await _client.GetAsync($"Films/filmByName?filmName={filmName}");
            responce.EnsureSuccessStatusCode();
            var content = responce.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<FilmResponse>(content);

            if (result.genre_ids != "")
            {
                string[] geners = result.genre_ids.Split(",");

                string geners_to_string = "";
                foreach (var i in geners)
                {
                    geners_to_string += await this.GetFilmGenreById(Int32.Parse(i)) + ", ";
                }
                result.genre_ids = geners_to_string.Substring(0, geners_to_string.Length - 2);
            }
            Console.WriteLine(result.genre_ids);
            return result;
        }

        public async Task<GameResponse> GetGameByName(string gameName)
        {
            var responce = await _client.GetAsync($"Games/gameByName?gameName={gameName}");
            responce.EnsureSuccessStatusCode();
            var content = responce.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<GameResponse>(content);
            if (result.genres != "")
            {
                string[] geners = result.genres.Split(",");

                string geners_to_string = "";
                foreach (var i in geners)
                {
                    geners_to_string += await this.GetGameGenreById(Int32.Parse(i)) + ", ";
                }
                result.genres = geners_to_string.Substring(0, geners_to_string.Length - 2);
            }
            

            return result;
        }
    }
}
