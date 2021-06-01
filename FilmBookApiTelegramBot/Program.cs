using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace FilmBookApiTelegramBot
{
    class Program
    {
        private static readonly  ApiClient _client = new ApiClient();
        private static ITelegramBotClient botClient = new TelegramBotClient(Constants.Token) { Timeout = TimeSpan.FromSeconds(20) };
        static void Main(string[] args)
        {
            System.Diagnostics.Debug.WriteLine("sad");

            var me = botClient.GetMeAsync().Result;

            botClient.OnMessage += BotOnMessage;
            botClient.OnCallbackQuery += BotOnCallbackQueryReceived;
            botClient.StartReceiving();


            Console.ReadLine();
        }
        private static InlineKeyboardButton[][] GetInlineKeyboardForFilm(int id)
        {
            var keyboardInline = new InlineKeyboardButton[3][];
            var keyboardRow0 = new InlineKeyboardButton[1];
            var keyboardRow1 = new InlineKeyboardButton[1];
            var keyboardRow2 = new InlineKeyboardButton[1];
            keyboardRow0[0] = new InlineKeyboardButton
            {
                Text = "See Connected Games",
                CallbackData = $"seeGamesForFilm {id}"
            };
            keyboardRow1[0] = new InlineKeyboardButton
            {
                Text = "See Connected Books",
                CallbackData = $"seeBooksForFilm {id}"
            };
            keyboardRow2[0] = new InlineKeyboardButton
            {
                Text = "Add this Film to Your Favourites",
                CallbackData = $"addFilmToFavourites {id}"
            };
            keyboardInline[0] = keyboardRow0;
            keyboardInline[1] = keyboardRow1;
            keyboardInline[2] = keyboardRow2;
            return keyboardInline;
        }

        private static InlineKeyboardButton[][] GetInlineKeyboardForBook(string id)
        {
            var keyboardInline = new InlineKeyboardButton[3][];
            var keyboardRow0 = new InlineKeyboardButton[1];
            var keyboardRow1 = new InlineKeyboardButton[1];
            var keyboardRow2 = new InlineKeyboardButton[1];

            keyboardRow0[0] = new InlineKeyboardButton
            {
                Text = "See Connected Games",
                CallbackData = $"seeGamesForBook {id}"
            };
            keyboardRow1[0] = new InlineKeyboardButton
            {
                Text = "See Connected Films",
                CallbackData = $"seeFilmsForBook {id}"
            };
            keyboardRow2[0] = new InlineKeyboardButton
            {
                Text = "Add this Book to Your Favourites",
                CallbackData = $"addBookToFavourites {id}"
            };
            keyboardInline[0] = keyboardRow0;
            keyboardInline[1] = keyboardRow1;
            keyboardInline[2] = keyboardRow2;
            return keyboardInline;
        }
        private static InlineKeyboardButton[][] GetInlineKeyboardForGame(int id)
        {
            var keyboardInline = new InlineKeyboardButton[3][];
            var keyboardRow0 = new InlineKeyboardButton[1];
            var keyboardRow1 = new InlineKeyboardButton[1];
            var keyboardRow2 = new InlineKeyboardButton[1];

            keyboardRow0[0] = new InlineKeyboardButton
            {
                Text = "See Connected Books",
                CallbackData = $"seeBooksForGame {id}"
            };
            keyboardRow1[0] = new InlineKeyboardButton
            {
                Text = "See Connected Films",
                CallbackData = $"seeFilmsForGame {id}"
            };
            keyboardRow2[0] = new InlineKeyboardButton
            {
                Text = "Add this Game to Your Favourites",
                CallbackData = $"addGameToFavourites {id}"
            };
            keyboardInline[0] = keyboardRow0;
            keyboardInline[1] = keyboardRow1;
            keyboardInline[2] = keyboardRow2;
            return keyboardInline;
        }


        private static InlineKeyboardButton[][] GetInlineKeyboardForFilmFavourite(int id)
        {
            var keyboardInline = new InlineKeyboardButton[3][];
            var keyboardRow0 = new InlineKeyboardButton[1];
            var keyboardRow1 = new InlineKeyboardButton[1];
            var keyboardRow2 = new InlineKeyboardButton[1];

            keyboardRow0[0] = new InlineKeyboardButton
            {
                Text = "See Connected Games",
                CallbackData = $"seeGamesForFilm {id}"
            };
            keyboardRow1[0] = new InlineKeyboardButton
            {
                Text = "See Connected Books",
                CallbackData = $"seeBooksForFilm {id}"
            };
            keyboardRow2[0] = new InlineKeyboardButton
            {
                Text = "Remove From Favourite",
                CallbackData = $"removeFavouriteFilm {id}"
            };
            keyboardInline[0] = keyboardRow0;
            keyboardInline[1] = keyboardRow1;
            keyboardInline[2] = keyboardRow2;
            return keyboardInline;
        }
        private static InlineKeyboardButton[][] GetInlineKeyboardForGameFavourite(int id)
        {
            var keyboardInline = new InlineKeyboardButton[3][];
            var keyboardRow0 = new InlineKeyboardButton[1];
            var keyboardRow1 = new InlineKeyboardButton[1];
            var keyboardRow2 = new InlineKeyboardButton[1];

            keyboardRow0[0] = new InlineKeyboardButton
            {
                Text = "See Connected Books",
                CallbackData = $"seeBooksForGame {id}"
            };
            keyboardRow1[0] = new InlineKeyboardButton
            {
                Text = "See Connected Films",
                CallbackData = $"seeFilmsForGame {id}"
            };
            keyboardRow2[0] = new InlineKeyboardButton
            {
                Text = "Remove From Favourite",
                CallbackData = $"removeFavouriteGame {id}"
            };
            keyboardInline[0] = keyboardRow0;
            keyboardInline[1] = keyboardRow1;
            keyboardInline[2] = keyboardRow2;
            return keyboardInline;
        }
        private static InlineKeyboardButton[][] GetInlineKeyboardForBookFavourite(string isbn)
        {
            var keyboardInline = new InlineKeyboardButton[3][];
            var keyboardRow0 = new InlineKeyboardButton[1];
            var keyboardRow1 = new InlineKeyboardButton[1];
            var keyboardRow2 = new InlineKeyboardButton[1];

            keyboardRow0[0] = new InlineKeyboardButton
            {
                Text = "See Connected Games",
                CallbackData = $"seeGamesForBook {isbn}"
            };
            keyboardRow1[0] = new InlineKeyboardButton
            {
                Text = "See Connected Films",
                CallbackData = $"seeFilmsForBook {isbn}"
            };
            keyboardRow2[0] = new InlineKeyboardButton
            {
                Text = "Remove From Favourite",
                CallbackData = $"removeFavouriteBook {isbn}"
            };
            keyboardInline[0] = keyboardRow0;
            keyboardInline[1] = keyboardRow1;
            keyboardInline[2] = keyboardRow2;
            return keyboardInline;
        }





        private async static void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs e)
        {
            if (e.CallbackQuery.Data.StartsWith("seeGamesForFilm "))
            {
                try
                {
                    int FilmId = Int32.Parse(e.CallbackQuery.Data.Split(" ")[1]);
                    System.Diagnostics.Debug.WriteLine(FilmId);
                    int ConnectedGameId = Int32.Parse(await _client.GetFilmConnectdGameIdById(FilmId));
                    var game = await _client.GetGameById(ConnectedGameId);

                    await botClient.SendTextMessageAsync(
                        chatId: e.CallbackQuery.Message.Chat.Id,
                        text: "That's what we found:\n" +
                        $"Title: {game.name}\n" +
                        $"Genres: {game.genres}\n" +
                        $"Rating: {game.rating} out of 5\n" +
                        $"Release Date: {game.released}\n" +
                        $"Poster: {game.background_image} \n" +
                        $"More info on this link: https://rawg.io/games/{game.id}\n"

                        ).ConfigureAwait(false);
                }
                catch
                {
                    await botClient.SendTextMessageAsync(
                        chatId: e.CallbackQuery.Message.Chat.Id,
                        text: "Sorry, we weren't able to find anything or you sent a few requests too fast\n" +
                        "If you are sure that it is a mistake please contact us at @denyess."
                        ).ConfigureAwait(false);
                }
            }
            else if (e.CallbackQuery.Data.StartsWith("seeBooksForFilm "))
            {
                try
                {
                    int FilmId = Int32.Parse(e.CallbackQuery.Data.Split(" ")[1]);
                    string ConnectedBookId = await _client.GetFilmConnectdBookIdById(FilmId);
                    var book = await _client.GetBookById(ConnectedBookId);

                    await botClient.SendTextMessageAsync(
                        chatId: e.CallbackQuery.Message.Chat.Id,
                        text: "That's what we found:\n" +
                        $"Title: {book.title}\n" +
                        $"Authors: {book.authors}\n" +
                        $"Publish Date: {book.publish_date}\n" +
                        $"ISBN: {book.isbn_10} \n" +
                        $"More info on this link: {book.url}\n"
                        ).ConfigureAwait(false);
                }
                catch
                {
                    await botClient.SendTextMessageAsync(
                        chatId: e.CallbackQuery.Message.Chat.Id,
                        text: "Sorry, we weren't able to find anything or you sent a few requests too fast\n" +
                        "If you are sure that it is a mistake please contact us at @denyess."
                        ).ConfigureAwait(false);
                }
            }
            else if (e.CallbackQuery.Data.StartsWith("seeFilmsForGame "))
            {
                try
                {
                    int gameId = Int32.Parse(e.CallbackQuery.Data.Split(" ")[1]);
                    string connectedFilmId = await _client.GetGameConnectdFilmIdById(gameId);
                    var film = await _client.GetFilmById(Int32.Parse(connectedFilmId));

                    await botClient.SendTextMessageAsync(
                        chatId: e.CallbackQuery.Message.Chat.Id,
                        text: "That's what we found:\n" +
                        $"Title: {film.original_title}\n" +
                        $"Overview: {film.overview}\n" +
                        $"Genres: {film.genre_ids}\n" +
                        $"Average Mark: {film.vote_average} out of 10 \n" +
                        $"Release Date: {film.release_date}\n" +
                        $"Poster: https://image.tmdb.org/t/p/original/{film.poster_path} \n" +
                        $"More info by this link: https://www.themoviedb.org/movie/{film.id}"
                        ).ConfigureAwait(false);
                }
                catch
                {
                    await botClient.SendTextMessageAsync(
                        chatId: e.CallbackQuery.Message.Chat.Id,
                        text: "Sorry, we weren't able to find anything or you sent a few requests too fast\n" +
                        "If you are sure that it is a mistake please contact us at @denyess."
                        ).ConfigureAwait(false);
                }

            }
            else if (e.CallbackQuery.Data.StartsWith("seeBooksForGame "))
            {
                try
                {
                    int gameId = Int32.Parse(e.CallbackQuery.Data.Split(" ")[1]);
                    string connectedFilmId = await _client.GetGameConnectdBookIdById(gameId);
                    var book = await _client.GetBookById(connectedFilmId);
                    await botClient.SendTextMessageAsync(
                        chatId: e.CallbackQuery.Message.Chat.Id,
                        text: "That's what we found:\n" +
                        $"Title: {book.title}\n" +
                        $"Authors: {book.authors}\n" +
                        $"Publish Date: {book.publish_date}\n" +
                        $"ISBN: {book.isbn_10} \n" +
                        $"More info on this link: {book.url}\n"
                        ).ConfigureAwait(false);
                }
                catch
                {
                    await botClient.SendTextMessageAsync(
                        chatId: e.CallbackQuery.Message.Chat.Id,
                        text: "Sorry, we weren't able to find anything or you sent a few requests too fast\n" +
                        "If you are sure that it is a mistake please contact us at @denyess."
                        ).ConfigureAwait(false);
                }
            }
            else if (e.CallbackQuery.Data.StartsWith("seeFilmsForBook "))
            {
                try
                {
                    string BookIsbn = e.CallbackQuery.Data.Split(" ")[1];
                    string connectedFilmId = await _client.GetBookConnectdFilmIdById(BookIsbn);
                    var film = await _client.GetFilmById(Int32.Parse(connectedFilmId));

                    await botClient.SendTextMessageAsync(
                        chatId: e.CallbackQuery.Message.Chat.Id,
                        text: "That's what we found:\n" +
                        $"Title: {film.original_title}\n" +
                        $"Overview: {film.overview}\n" +
                        $"Genres: {film.genre_ids}\n" +
                        $"Average Mark: {film.vote_average} out of 10 \n" +
                        $"Release Date: {film.release_date}\n" +
                        $"Poster: https://image.tmdb.org/t/p/original/{film.poster_path} \n" +
                        $"More info by this link: https://www.themoviedb.org/movie/{film.id}"
                        ).ConfigureAwait(false);
                }
                catch
                {
                    await botClient.SendTextMessageAsync(
                        chatId: e.CallbackQuery.Message.Chat.Id,
                        text: "Sorry, we weren't able to find anything or you sent a few requests too fast\n" +
                        "If you are sure that it is a mistake please contact us at @denyess."
                        ).ConfigureAwait(false);
                }
            }
            else if (e.CallbackQuery.Data.StartsWith("seeGamesForBook "))
            {
                try
                {
                    string BookId = e.CallbackQuery.Data.Split(" ")[1];
                    int ConnectedGameId = Int32.Parse(await _client.GetBookConnectdGameIdById(BookId));
                    var game = await _client.GetGameById(ConnectedGameId);

                    await botClient.SendTextMessageAsync(
                        chatId: e.CallbackQuery.Message.Chat.Id,
                        text: "That's what we found:\n" +
                        $"Title: {game.name}\n" +
                        $"Genres: {game.genres}\n" +
                        $"Rating: {game.rating} out of 5\n" +
                        $"Release Date: {game.released}\n" +
                        $"Poster: {game.background_image} \n" +
                        $"More info on this link: https://rawg.io/games/{game.id}\n"

                        ).ConfigureAwait(false);
                }
                catch
                {
                    await botClient.SendTextMessageAsync(
                        chatId: e.CallbackQuery.Message.Chat.Id,
                        text: "Sorry, we weren't able to find anything or you sent a few requests too fast\n" +
                        "If you are sure that it is a mistake please contact us at @denyess."
                        ).ConfigureAwait(false);
                }
            }
            else if (e.CallbackQuery.Data.StartsWith("addBookToFavourites "))
            {
                try
                {
                    var result = await _client.AddBookToFavourite(e.CallbackQuery.Message.Chat.Id.ToString(), e.CallbackQuery.Data.Split(" ")[1]);
                    await botClient.SendTextMessageAsync(
                        chatId: e.CallbackQuery.Message.Chat.Id,
                        text: result
                        ).ConfigureAwait(false);
                }
                catch
                {
                    await botClient.SendTextMessageAsync(
                        chatId: e.CallbackQuery.Message.Chat.Id,
                        text: "You already have this book in your favourites\n"
                        ).ConfigureAwait(false);
                }
            }
            else if (e.CallbackQuery.Data.StartsWith("addGameToFavourites "))
            {
                try
                {
                    var result = await _client.AddGameToFavourite(e.CallbackQuery.Message.Chat.Id.ToString(), Int32.Parse(e.CallbackQuery.Data.Split(" ")[1]));
                    await botClient.SendTextMessageAsync(
                        chatId: e.CallbackQuery.Message.Chat.Id,
                        text: result
                        ).ConfigureAwait(false);
                }
                catch
                {
                    await botClient.SendTextMessageAsync(
                        chatId: e.CallbackQuery.Message.Chat.Id,
                        text: "You already have this game in your favourites\n"
                        ).ConfigureAwait(false);
                }
            }
            else if (e.CallbackQuery.Data.StartsWith("addFilmToFavourites "))
            {
                try
                {
                    var result = await _client.AddFilmToFavourite(e.CallbackQuery.Message.Chat.Id.ToString(), Int32.Parse(e.CallbackQuery.Data.Split(" ")[1]));
                    await botClient.SendTextMessageAsync(
                        chatId: e.CallbackQuery.Message.Chat.Id,
                        text: result
                        ).ConfigureAwait(false);
                }
                catch
                {
                    await botClient.SendTextMessageAsync(
                        chatId: e.CallbackQuery.Message.Chat.Id,
                        text: "You already have this game in your favourites\n"
                        ).ConfigureAwait(false);
                }
            }
            else if (e.CallbackQuery.Data.StartsWith("removeFavouriteFilm "))
            {
                try
                {
                    var result = await _client.RemoveFilmFromFavourite(e.CallbackQuery.Message.Chat.Id.ToString(), Int32.Parse(e.CallbackQuery.Data.Split(" ")[1]));
                    await botClient.SendTextMessageAsync(
                        chatId: e.CallbackQuery.Message.Chat.Id,
                        text: result
                        ).ConfigureAwait(false);
                }
                catch
                {
                    await botClient.SendTextMessageAsync(
                        chatId: e.CallbackQuery.Message.Chat.Id,
                        text: "This film is already removed from your favourite."
                        ).ConfigureAwait(false);
                }
            }
            else if (e.CallbackQuery.Data.StartsWith("removeFavouriteGame "))
            {
                try
                {
                    var result = await _client.RemoveGameFromFavourite(e.CallbackQuery.Message.Chat.Id.ToString(), Int32.Parse(e.CallbackQuery.Data.Split(" ")[1]));
                    await botClient.SendTextMessageAsync(
                        chatId: e.CallbackQuery.Message.Chat.Id,
                        text: result
                        ).ConfigureAwait(false);
                }
                catch
                {
                    await botClient.SendTextMessageAsync(
                        chatId: e.CallbackQuery.Message.Chat.Id,
                        text: "This game is already removed from your favourite."
                        ).ConfigureAwait(false);
                }
            }
            else if (e.CallbackQuery.Data.StartsWith("removeFavouriteBook "))
            {
                try
                {
                    var result = await _client.RemoveBookFromFavourite(e.CallbackQuery.Message.Chat.Id.ToString(), e.CallbackQuery.Data.Split(" ")[1]);
                    await botClient.SendTextMessageAsync(
                        chatId: e.CallbackQuery.Message.Chat.Id,
                        text: result
                        ).ConfigureAwait(false);
                }
                catch
                {
                    await botClient.SendTextMessageAsync(
                        chatId: e.CallbackQuery.Message.Chat.Id,
                        text: "This book is already removed from your favourite."
                        ).ConfigureAwait(false);
                }
            }
        }
        private async static void BotOnMessage(object sender, MessageEventArgs e)
        {
            try
            {
                var text = e?.Message?.Text;
                if (text == null)
                {
                    return;
                }
                else if (text == "/start")
                {
                    await botClient.SendTextMessageAsync(
                        chatId: e.Message.Chat.Id,
                        text: $"Hello dear user, this is a FilmsGamesBooksBot that can help you seek for new titles.\nIf you want to see the list of commands please type /help",
                        replyToMessageId: e.Message.MessageId
                        ).ConfigureAwait(false);
                }
                else if (text == "/help")
                {
                    await botClient.SendTextMessageAsync(
                        chatId: e.Message.Chat.Id,
                        text: $"/findfilm 'NameOfFilm' will show you the info about the film you are looking for\n" +
                        $"/findgame 'NameOfGame' will show you the info about the game you are looking for\n" +
                        $"/findbook 'ISBNOfBook' will show you the info about the book you are looking for\n" +
                        $"/showfavouritegames will show you a list of your favourite games\n" +
                        $"/showfavouritefilms will show you a list of your favourite films\n" +
                        $"/showfavouritebooks will show you a list of your favourite books\n" +
                        $"/getrecommendationforgames will show you a game that we think you should try based on your favourite games\n" +
                        $"/getrecommendationforfilms will show you a film that we think you should try based on your favourite films\n" +
                        $"That's all for now \n",
                        replyToMessageId: e.Message.MessageId
                        ).ConfigureAwait(false);
                }
                else if (text.StartsWith("/findfilm"))
                {
                    try
                    {
                        var film = await _client.GetFilmByName(text.Substring(10));
                        var keyboardMarkupForOneDay = new InlineKeyboardMarkup(GetInlineKeyboardForFilm(film.id));
                        System.Diagnostics.Debug.WriteLine(film.genre_ids);
                        await botClient.SendTextMessageAsync(
                            chatId: e.Message.Chat.Id,
                            text: "That's what we found:\n" +
                            $"Title: {film.original_title}\n" +
                            $"Overview: {film.overview}\n" +
                            $"Genres: {film.genre_ids}\n" +
                            $"Average Mark: {film.vote_average} out of 10 \n" +
                            $"Release Date: {film.release_date}\n" +
                            $"Poster: https://image.tmdb.org/t/p/original/{film.poster_path} \n" +
                            $"More info by this link: https://www.themoviedb.org/movie/{film.id}",
                            replyMarkup: keyboardMarkupForOneDay,
                            replyToMessageId: e.Message.MessageId
                            ).ConfigureAwait(false);
                    }
                    catch
                    {
                        await botClient.SendTextMessageAsync(
                            chatId: e.Message.Chat.Id,
                            text: "Sorry we weren't able to find anything. Or our servers are lagging",
                            replyToMessageId: e.Message.MessageId
                            ).ConfigureAwait(false);
                    }
                }
                else if (text.StartsWith("/findbook"))
                {
                    try
                    {
                        var book = await _client.GetBookById(text.Substring(10));

                        var keyboardMarkupForOneDay = new InlineKeyboardMarkup(GetInlineKeyboardForBook(book.isbn_10));
                        await botClient.SendTextMessageAsync(
                        chatId: e.Message.Chat.Id,
                        text: "That's what we found:\n" +
                        $"Title: {book.title}\n" +
                        $"Authors: {book.authors}\n" +
                        $"Publish Date: {book.publish_date}\n" +
                        $"ISBN: {book.isbn_10} \n" +
                        $"More info by this link: {book.url}",
                        replyMarkup: keyboardMarkupForOneDay,
                        replyToMessageId: e.Message.MessageId
                        ).ConfigureAwait(false);
                    }
                    catch
                    {
                        await botClient.SendTextMessageAsync(
                        chatId: e.Message.Chat.Id,
                        text: "Sorry we weren't able to find anything",
                        replyToMessageId: e.Message.MessageId
                        ).ConfigureAwait(false);
                    }

                }
                else if (text.StartsWith("/findgame"))
                {
                    try
                    {
                        var game = await _client.GetGameByName(text.Substring(10));

                        var keyboardMarkupForOneDay = new InlineKeyboardMarkup(GetInlineKeyboardForGame(game.id));
                        await botClient.SendTextMessageAsync(
                        chatId: e.Message.Chat.Id,
                        text: "That's what we found:\n" +
                        $"Title: {game.name}\n" +
                        $"Genres: {game.genres}\n" +
                        $"Rating: {game.rating} out of 5\n" +
                        $"Release Date: {game.released}\n" +
                        $"Poster: {game.background_image} \n" +
                        $"More info on this link: https://rawg.io/games/{game.id}\n",
                        replyMarkup: keyboardMarkupForOneDay,
                        replyToMessageId: e.Message.MessageId
                        ).ConfigureAwait(false);
                    }
                    catch
                    {
                        await botClient.SendTextMessageAsync(
                        chatId: e.Message.Chat.Id,
                        text: "Sorry we weren't able to find anything",
                        replyToMessageId: e.Message.MessageId
                        ).ConfigureAwait(false);
                    }
                }
                else if (text.StartsWith("/showfavouritefilms"))
                {
                    try
                    {
                        var favouriteFilms = await _client.GetFavouriteFilms(e.Message.Chat.Id.ToString());
                        int count = 0;
                        foreach (var favouriteFilm in favouriteFilms)
                        {
                            count++;
                            var film = await _client.GetFilmById(favouriteFilm.film_id);
                            var keyboardMarkupForOneDay = new InlineKeyboardMarkup(GetInlineKeyboardForFilmFavourite(film.id));
                            string[] geners = film.genre_ids.Split(",");
                            if (film.genre_ids != "")
                            {
                                string geners_to_string = "";
                                foreach (var i in geners)
                                {
                                    geners_to_string += await _client.GetFilmGenreById(Int32.Parse(i)) + ", ";
                                }

                                film.genre_ids = geners_to_string.Substring(0, geners_to_string.Length - 2);
                            }
                            await botClient.SendTextMessageAsync(
                            chatId: e.Message.Chat.Id,
                            text: "That's what we found:\n" +
                            $"Title: {film.original_title}\n" +
                            $"Overview: {film.overview}\n" +
                            $"Genres: {film.genre_ids}\n" +
                            $"Average Mark: {film.vote_average} out of 10 \n" +
                            $"Release Date: {film.release_date}\n" +
                            $"Poster: https://image.tmdb.org/t/p/original/{film.poster_path} \n" +
                            $"More info by this link: https://www.themoviedb.org/movie/{film.id}",
                            replyMarkup: keyboardMarkupForOneDay
                            ).ConfigureAwait(false);
                        }
                        if (count == 0)
                        {
                            await botClient.SendTextMessageAsync(
                            chatId: e.Message.Chat.Id,
                            text: "You have no favourite films."
                            ).ConfigureAwait(false);
                        }
                    }
                    catch
                    {
                        await botClient.SendTextMessageAsync(
                            chatId: e.Message.Chat.Id,
                            text: "Something went wrong cause of a hosting issues."
                            ).ConfigureAwait(false);
                    }

                }
                else if (text.StartsWith("/showfavouritebooks"))
                {
                    try
                    {
                        var favouriteBooks = await _client.GetFavouriteBooks(e.Message.Chat.Id.ToString());
                        int count = 0;
                        foreach (var favouriteBook in favouriteBooks)
                        {
                            count++;
                            var book = await _client.GetBookById(favouriteBook.book_id);
                            var keyboardMarkupForOneDay = new InlineKeyboardMarkup(GetInlineKeyboardForBookFavourite(book.isbn_10));
                            await botClient.SendTextMessageAsync(
                            chatId: e.Message.Chat.Id,
                            text: "That's what we found:\n" +
                            $"Title: {book.title}\n" +
                            $"Authors: {book.authors}\n" +
                            $"Publish Date: {book.publish_date}\n" +
                            $"ISBN: {book.isbn_10} \n" +
                            $"More info by this link: {book.url}",
                            replyMarkup: keyboardMarkupForOneDay
                            ).ConfigureAwait(false);
                        }
                        if (count == 0)
                        {
                            await botClient.SendTextMessageAsync(
                            chatId: e.Message.Chat.Id,
                            text: "You have no favourite books."
                            ).ConfigureAwait(false);
                        }
                    }
                    catch
                    {
                        await botClient.SendTextMessageAsync(
                            chatId: e.Message.Chat.Id,
                            text: "Something went wrong cause of a hosting issues."
                            ).ConfigureAwait(false);
                    }

                }
                else if (text.StartsWith("/showfavouritegames"))
                {
                    try
                    {
                        int count = 0;
                        var favouriteGames = await _client.GetFavouriteGames(e.Message.Chat.Id.ToString());
                        foreach (var favouriteGame in favouriteGames)
                        {
                            count++;
                            var game = await _client.GetGameById(favouriteGame.game_id);
                            if (game.genres != "")
                            {
                                string[] geners = game.genres.Split(",");

                                string geners_to_string = "";
                                foreach (var i in geners)
                                {
                                    geners_to_string += await _client.GetGameGenreById(Int32.Parse(i)) + ", ";
                                }
                                game.genres = geners_to_string.Substring(0, geners_to_string.Length - 2);
                            }

                            var keyboardMarkupForOneDay = new InlineKeyboardMarkup(GetInlineKeyboardForGameFavourite(game.id));
                            await botClient.SendTextMessageAsync(
                            chatId: e.Message.Chat.Id,
                            text: "That's what we found:\n" +
                            $"Title: {game.name}\n" +
                            $"Genres: {game.genres}\n" +
                            $"Rating: {game.rating} out of 5\n" +
                            $"Release Date: {game.released}\n" +
                            $"Poster: {game.background_image} \n" +
                            $"More info on this link: https://rawg.io/games/{game.id}\n",
                            replyMarkup: keyboardMarkupForOneDay
                            ).ConfigureAwait(false);
                        }
                        if (count == 0)
                        {
                            await botClient.SendTextMessageAsync(
                            chatId: e.Message.Chat.Id,
                            text: "You have no favourite games."
                            ).ConfigureAwait(false);
                        }
                    }
                    catch
                    {
                        await botClient.SendTextMessageAsync(
                            chatId: e.Message.Chat.Id,
                            text: "Something went wrong cause of a hosting issues."
                            ).ConfigureAwait(false);
                    }
                }
                else if (text.StartsWith("/getrecommendationforfilms"))
                {
                    try
                    {
                        var film = await _client.GetRecomendationForFilms(e.Message.Chat.Id.ToString());
                        var keyboardMarkupForOneDay = new InlineKeyboardMarkup(GetInlineKeyboardForFilm(film.id));
                        await botClient.SendTextMessageAsync(
                            chatId: e.Message.Chat.Id,
                            text: "That's what we found:\n" +
                            $"Title: {film.original_title}\n" +
                            $"Overview: {film.overview}\n" +
                            $"Genres: {film.genre_ids}\n" +
                            $"Average Mark: {film.vote_average} out of 10 \n" +
                            $"Release Date: {film.release_date}\n" +
                            $"Poster: https://image.tmdb.org/t/p/original/{film.poster_path} \n" +
                            $"More info by this link: https://www.themoviedb.org/movie/{film.id}",
                            replyMarkup: keyboardMarkupForOneDay,
                            replyToMessageId: e.Message.MessageId
                            ).ConfigureAwait(false);
                    }
                    catch
                    {
                        await botClient.SendTextMessageAsync(
                            chatId: e.Message.Chat.Id,
                            text: "We weren't able to find a recomendation for you. Maybe you don't have any favourites or our servers are laggin"
                            ).ConfigureAwait(false);
                    }

                }
                else if (text.StartsWith("/getrecommendationforgames"))
                {
                    try
                    {
                        var game = await _client.GetRecomendationForGames(e.Message.Chat.Id.ToString());
                        var keyboardMarkupForOneDay = new InlineKeyboardMarkup(GetInlineKeyboardForGame(game.id));
                        await botClient.SendTextMessageAsync(
                        chatId: e.Message.Chat.Id,
                        text: "That's what we found:\n" +
                        $"Title: {game.name}\n" +
                        $"Genres: {game.genres}\n" +
                        $"Rating: {game.rating} out of 5\n" +
                        $"Release Date: {game.released}\n" +
                        $"Poster: {game.background_image} \n" +
                        $"More info on this link: https://rawg.io/games/{game.id}\n",
                        replyMarkup: keyboardMarkupForOneDay,
                        replyToMessageId: e.Message.MessageId
                        ).ConfigureAwait(false);
                    }
                    catch
                    {
                        await botClient.SendTextMessageAsync(
                            chatId: e.Message.Chat.Id,
                            text: "We weren't able to find a recomendation for you. Maybe you don't have any favourites or our servers are laggin"
                            ).ConfigureAwait(false);
                    }
                }

                else
                {
                    await botClient.SendTextMessageAsync(
                        chatId: e.Message.Chat.Id,
                        text: "Sorry we weren't able to identify your command",
                        replyToMessageId: e.Message.MessageId
                        ).ConfigureAwait(false);
                }
            }

            catch {
                await botClient.SendTextMessageAsync(
                        chatId: e.Message.Chat.Id,
                        text: "Sorry something went wrong. Please try again later",
                        replyToMessageId: e.Message.MessageId
                        ).ConfigureAwait(false);
            }
        }
       
    }
}
