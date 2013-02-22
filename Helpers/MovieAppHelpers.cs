using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using DB;
using LinqKit;

using SmartFormat;

//using System.Linq.Dynamic;

namespace Helpers
{
    public static class MovieAppHelpers
    {
        public static Film Film;
		//public static MovieCollectionEntities Context = new MovieCollectionEntities();
		public static MovieCollectionEntities Context = new MovieCollectionEntities();
        public static List<string> Genres = new List<string>
                                                {
                                                    "Action",
                                                    "Comedy",
                                                    "Drama",
                                                    "Family",
                                                    "Romance",
                                                    "Sci-Fi",
                                                    "Musical",
                                                    "Horror"
                                                };

	    public static List<string> FilmTitles
	    {
		    get
		    {
			    using (MovieCollectionEntities context = new MovieCollectionEntities())
			    {
				    return context.Films.Select(film => film.Title).ToList();
			    }
		    }
	    }

	    private static int _actorRoleId = 0;
        private static int _directorRoleId;

		private static Dictionary<string, int> _ratings;
		public  static Dictionary<string, int> Ratings
		{
			get
			{
				return _ratings ??
					   (_ratings =
						(from rating in Context.Ratings
						 select rating.MPAARating).ToList().ToDictionary((from rating in Context.Ratings
																  select rating.RatingID).ToList()));
			}
		}

		#region MovieHelpers

		public static bool RemoveMovieFomCollection( this string title)
		{
			try
			{
				//Film retrievedMovie = (from movie in Context.Films where movie.Title == title select movie).FirstOrDefault();
				//if (retrievedMovie != null && retrievedMovie.Title == title)
				//{
				//	List<PersonFilmIndex> pfis =
				//		(from pfi in Context.PersonFilmIndexes where pfi.FilmID == retrievedMovie.FilmID select pfi).
				//			ToList();
				//	pfis.ForEach(pfi => Context.PersonFilmIndexes.DeleteObject(pfi));
				//	List<GenreFilmIndex> gfis =
				//		(from gfi in Context.GenreFilmIndexes where gfi.FilmID == retrievedMovie.FilmID select gfi).
				//			ToList();
				//	gfis.ForEach(gfi => Context.GenreFilmIndexes.DeleteObject(gfi));

				//	Context.Films.DeleteObject(retrievedMovie);

				//	Context.SaveChanges();
				//	return true;
				//}
				//return false;
			}
			catch (Exception ex)
			{
				Log.Error( ex );
				ex.ThrowFormattedException();
				throw;
			}
			return true;
		}

		public static bool AddMovieToCollection(this IMDB movie)
		{
			try
			{
				//if (AddMovieToDB(movie))
				//{
					//AddGenresFromMovie(movie);
					//AddPeopleFromMovie(movie);
					//SaveAndRefreshDBConnection();
                    
					////retrieve for DB assigned id
					//Film = ReadMovieFromCollection(movie.Title);

					//AddGenreFilmIndexes(Film.FilmID, movie);

					//AddPersonFilmIndexesFromMovie(Film.FilmID, movie);

                    

					//SaveAndCloseDBConnection();
					return true;
				//}
				//return false;
			}
			catch (Exception ex)
			{
				Log.Error(ex);
				ex.ThrowFormattedException();
			}
			return false;

		}

		public static void SaveAndRefreshDBConnection()
		{
			Context.SaveChanges();
			Context = new MovieCollectionEntities();
		}

		public static void SaveAndCloseDBConnection()
		{
			Context.SaveChanges();
			//Context.Connection.Close();
		}

		public static void InitializeDBConnection()
		{
			if (Context == null)
			{
				Context = new MovieCollectionEntities();
			}
		}

		public static Film ReadMovieFromCollection(this string title)
		{

			Film = (from flick in Context.Films
					where flick.Title == title
					select flick).ToList().FirstOrDefault();
			return Film;

		}

		//public static bool AddMovieToDB(this IMDB movie)
		//{
		//	if (!CheckDBForMovie(movie.Title))
		//	{
		//		int rating = Ratings.GetValue(movie.MpaaRating);
		//		if (rating == default(int))
		//		{
		//			Context.AddToRatings(new Rating()
		//									 {
		//										 MPAARating = movie.MpaaRating 
		//									 });
		//			Context.SaveChanges();
		//			_ratings = null;
		//			rating = Ratings.GetValue(movie.MpaaRating);
		//		}


		//		try
		//		{
		//			Context.AddToFilms
		//				(
		//					new Film
		//						{
		//							Title = movie.Title,
		//							Length = movie.Runtime.ToIntSafe(),
		//							Year = movie.Year.ToIntSafe(),
		//							Plot = movie.Plot,
		//							ImdbURL = movie.ImdbURL,
		//							MovieRating = rating

		//						}
		//				);
		//			return true;

		//		}
		//		catch (Exception exception)
		//		{
		//			exception.ThrowFormattedException();
		//		}
		//	}

		//	return false;


		//}

		public static bool CheckDBForMovie(string movieTitle)
		{
			return (from film in Context.Films where film.Title == movieTitle select film).Any();
		}

		#endregion

		#region Genre

		public static void AddGenresFromMovie(this IMDB movie)
		{
			List<string> genresAlreadyInDB = (from genre in Context.Genres select genre.Name).ToList();

			List<string> genresToAdd =
				movie.Genres.Select(s => s.ToString(CultureInfo.InvariantCulture)).Where(
					genre => !genresAlreadyInDB.Contains(genre.ToString(CultureInfo.InvariantCulture))).ToList();

			//add the genres
			//foreach (string genre in genresToAdd)
			//{
			//	Context.AddToGenres(new Genre
			//							{
			//								Name = genre
			//							});
			//}

		}

		public static void AddGenresFromFilm(this Film filmWithGenresToAdd)
		{
			//add the genres
			//foreach (GenreFilmIndex genre in filmWithGenresToAdd.GenreFilmIndexes)
			//	Context.AddToGenres(genre.Genre);
		}

		public static List<Genre> GetAllGenres()
		{
			return Context.Genres.Select(genre => genre).Distinct().ToList();
		}

		public static List<string> GetAllGenreNames()
		{
			return (from genre in Context.Genres
					let name = genre.Name
					select name).Distinct().ToList();

		}

		public static List<string> GetFilmGenreNames(Film filmToSearchOn)
		{
			return filmToSearchOn.GenreFilmIndexes.Select(index => index.Genre.Name).ToList();
		}

		#endregion

		#region Person


		public static List<string> GetAllPeopleByQuery(string query, params object[] values)
		{
			//TODO - fix 
			//return (from personIndex in Context.PersonFilmIndexes.Where(query, values)
			//				  join person in Context.People on personIndex.PersonID equals person.PersonID
			//				  select person.FirstName + " " + person.LastName).Distinct().ToList();
			return new List<string>();
		}

		public static void AddPeopleFromMovie(this IMDB movie)
		{
			List<string> peopleAlreadyInDB =
				(from person in Context.People select person.FirstName + " " + person.LastName).ToList();

			List<string> people = movie.Cast.ToList();
			people.AddRange(movie.Directors.ToList());
			people.AddRange(movie.Writers.ToList());

			people = people.Distinct().ToList();

			List<string> peopleToAdd =
				people.Where(person => !peopleAlreadyInDB.Contains(person.ToString(CultureInfo.InvariantCulture))).
					ToList();


			//add the people
			foreach (string person in peopleToAdd)
			{
				Tuple<string, string> personName = person.SplitNameToFirstAndRemainingAsLast();

				//Context.AddToPeople(new Person
				//						{
				//							FirstName = personName.Item1,
				//							LastName = personName.Item2
				//						});
			}

		}

		public static List<Person> GetAllPeople()
		{
			return (from person in Context.People select person).ToList();
		}

		#endregion

		#region GenreFilmIndexs

		public static void AddGenreFilmIndexes(int filmId, IMDB movie)
		{
			//get genre list from db
			List<Genre> allGenres = GetAllGenres();

			List<Genre> genreFilmIndexesToAdd = allGenres.Where(p => movie.Genres.Contains(p.Name)).ToList();

			//for each genre from the db, if it's the movie's genre => add an index for it, better way?
			//foreach (Genre genre in genreFilmIndexesToAdd)
			//{
			//	Context.AddToGenreFilmIndexes(
			//		new GenreFilmIndex
			//			{
			//				FilmID = filmId,
			//				GenreID = genre.GenreID
			//			}
			//		);
			//}

		}

		#endregion

		#region PersonFilmIndexs

		public static void AddPersonFilmIndexes(this List<Person> peopleFilmIndexesToAdd, int filmID, Dictionary<string, string> castCharacters, int roleID )
		{ 
			foreach (Person person in peopleFilmIndexesToAdd)
			{

				//Context.AddToPersonFilmIndexes(
				//	new PersonFilmIndex
				//		{
				//			FilmID = filmID,
				//			PersonID = person.PersonID,
				//			Character = castCharacters.GetValue(string.Format("{0} {1}", person.FirstName, person.LastName)),
				//			RoleID = roleID
				//		}
				//	);

			}

		}

		public static void AddPersonFilmIndexesFromMovie(int filmId, IMDB movie)
		{
			//get person list from db
			List<Person> allPeople = GetAllPeople();

			AddPersonFilmIndexes( allPeople.Where(p => movie.Directors.Contains(string.Format("{0} {1}", p.FirstName, p.LastName))).ToList(),
													 filmId, movie.CastCharacters, FilmRole.DirectorRole.RoleID);
            
			 AddPersonFilmIndexes(allPeople.Where(p => movie.Cast.Contains(string.Format("{0} {1}", p.FirstName, p.LastName))).ToList(),
													 filmId, movie.CastCharacters, FilmRole.ActorRole.RoleID);


			 AddPersonFilmIndexes(allPeople.Where(p => movie.Writers.Contains(string.Format("{0} {1}", p.FirstName, p.LastName))).ToList(), 
													 filmId, movie.CastCharacters, FilmRole.WriterRole.RoleID);


		}

		#endregion

		#region Length Helpers

		public static IEnumerable<Tuple<int, int>> GetLengthsInRanges()
		{
			return (from movie in Context.Films
					let length = movie.Length
					select length).Distinct().ToList().GetContiguousRanges();
		}



		#endregion

		#region Picker Form Helpers

		public static int GetMaxMovieLength()
		{
			return (from movie in Context.Films
					select movie.Length).Max();
		}

		public static int GetMinMovieLength()
		{
			return (from movie in Context.Films
					select movie.Length).Min();
		}

		public static List<string> RetrieveAllActors()
		{
			return (from person in Context.People
					select person.FirstName + " " + person.LastName).ToList();
		}

		public static List<string> RetrieveAllTitles()
		{
			return (from movie in Context.Films
					select movie.Title).ToList();
		}

		#endregion

		#region Results Helpers

		public static List<TreeViewItem> GetTreeItems(this IEnumerable<Film> movies)
		{
			return movies.Select(GetTreeItem).ToList();
		}

		public static TreeViewItem GetTreeItem(this Film movie)
		{
			//Film movie = Context.Films.Where(film => film.Title = movieToRetrieve.Title);
			TreeViewItem root = new TreeViewItem { Header = movie.Title, Foreground = new SolidColorBrush(Colors.DarkOrange) };
			//IsExpanded = true

			#region QuickInfo

			TreeViewItem subTreeItem = new TreeViewItem { Foreground = new SolidColorBrush(Colors.DarkOrange), Header = "QuickInfo" };

			#region Length to Quickinfo
            

			TimeSpan span = TimeSpan.FromMinutes(movie.Length.ToIntSafe());
			int hours = ((int) span.TotalHours);
			int minutes = span.Minutes;
			string length = (hours >= 1)
								? Smart.Format("Length: {0} {0:hour|hours} and {1} {1:minute|minutes}", hours, minutes)
								: Smart.Format("Length: {1} {1:minute|minutes}", hours, minutes);

			subTreeItem.Items.Add(new TreeViewItem {Header = length, Foreground = new SolidColorBrush(Colors.DarkOrange)});

			#endregion

			#region Year to QuickInfo

			subTreeItem.Items.Add(new TreeViewItem { Header = string.Format("Year: {0}", movie.Year), Foreground = new SolidColorBrush(Colors.DarkOrange) });
            
			#endregion

			#region Rating to QuickInfo

			subTreeItem.Items.Add(new TreeViewItem { Header = string.Format("Rating: {0}", movie.Rating.MPAARating), Foreground = new SolidColorBrush(Colors.DarkOrange) });

			#endregion

			#region Directors to QuickInfo
            
			List<string> directors = movie.PersonFilmIndexes.Where(
				pfi => pfi.RoleID == FilmRole.DirectorRole.RoleID).Select(
					director => director.Person.FirstName.ToString(CultureInfo.InvariantCulture) + " " + director.Person.LastName)
				.ToList();

			string directorsAsString = directors.ToCommaSeparatedString().SmartSplit(20).ToList().ToDeliminatedString(Environment.NewLine);


			TreeViewItem directorRoot = new TreeViewItem {Header = Smart.Format("Director{0:|s}: {1}", directors.Count, directorsAsString), Foreground = new SolidColorBrush(Colors.DarkOrange)};

			//directors.ForEach(person => directorRoot.Items.Add(new TreeViewItem { Header = person, Foreground = new SolidColorBrush(Colors.DarkOrange) }));

			subTreeItem.Items.Add(directorRoot);

			root.Items.Add(subTreeItem);

			#endregion

			#endregion

			#region Genres

			subTreeItem = new TreeViewItem {Header = "Genres", Foreground = new SolidColorBrush(Colors.DarkOrange)};
			movie.GenreFilmIndexes.Select(gfi => gfi.Genre.Name.ToString(CultureInfo.InvariantCulture))
				.ToList().ForEach(genre => subTreeItem.Items.Add(new TreeViewItem { Header = genre, Foreground = new SolidColorBrush(Colors.DarkOrange) }));

			root.Items.Add(subTreeItem);

			//tvGenre.Items.Add(treeItem);

			#endregion

			#region Actors / Writers

			subTreeItem = new TreeViewItem {Header = "Actors", Foreground = new SolidColorBrush(Colors.DarkOrange)};
			movie.PersonFilmIndexes.Where(
				pfi => pfi.RoleID == FilmRole.ActorRole.RoleID).Select(
					pfi => pfi.Person.FirstName.ToString(CultureInfo.InvariantCulture) + " " + pfi.Person.LastName)
				.ToList().ForEach(person => subTreeItem.Items.Add(new TreeViewItem { Header = person, Foreground = new SolidColorBrush(Colors.DarkOrange) }));
            
			root.Items.Add(subTreeItem);

			subTreeItem = new TreeViewItem { Header = "Writers", Foreground = new SolidColorBrush(Colors.DarkOrange) };
			movie.PersonFilmIndexes.Where(
				pfi => pfi.RoleID == FilmRole.WriterRole.RoleID).Select(
					pfi => pfi.Person.FirstName.ToString(CultureInfo.InvariantCulture) + " " + pfi.Person.LastName)
				.ToList().ForEach(person => subTreeItem.Items.Add(new TreeViewItem { Header = person, Foreground = new SolidColorBrush(Colors.DarkOrange) }));

			root.Items.Add(subTreeItem);

			//tvCast.Items.Add(treeItem);

			#endregion

			#region Plot

			subTreeItem = new TreeViewItem { IsExpanded = true, Header = "Plot", Foreground = new SolidColorBrush(Colors.DarkOrange) };
			string moviePlot = movie.Plot;
			string plot = string.Empty;
			if (moviePlot.Length > 40)
			{
				List<string> split = moviePlot.SmartSplit(40).ToList();
				plot = split.ToDeliminatedString(Environment.NewLine);
			}
            
			subTreeItem.Items.Add(new TreeViewItem
									  {Header = ( plot == string.Empty ) ? moviePlot : plot , Foreground = new SolidColorBrush(Colors.DarkOrange)});

			root.Items.Add(subTreeItem);

			//tvGenre.Items.Add(treeItem);

			#endregion

			return root;
		}

		#endregion

		public static IEnumerable MoviesByGenre()
		{
			return (from genre in Context.Genres select genre)
				.ToList().Where(genre => Genres.Contains(genre.Name)).Select(genre =>
								 new KeyValuePair<string, int>(genre.Name,
															   (from index in Context.GenreFilmIndexes
																where index.GenreID == genre.GenreID
																select index).Count()))
				.ToList();
		}

		public static IEnumerable MoviesByActor()
		{
			return
				(from person in
					(from person in Context.People select person).ToList()
				let movies = (from index in Context.PersonFilmIndexes
							  where index.PersonID == person.PersonID
							  select index).Count()
				where movies > 5
				select new KeyValuePair<string, int>(person.FirstName + " " + person.LastName, movies)).ToList();
		}

		public static void FixMoviesInDB()
		{
			//(from movie in Context.Films select Film.Title).ToList().FillNewFieldsInMovieTable();
			List<string> movies = (from movie in Context.Films select movie.Title).ToList();
			movies.FillNewFieldsInMovieTable();
		}

		public static void FillNewFieldsInMovieTable(this IEnumerable<string> titlesToFix)
		{
			titlesToFix.ForEach( s =>  s.FillNewFieldsInMovieTable() );
		}

		public static void FillNewFieldsInMovieTable(this string titleToFix)
		{
			 if (!string.IsNullOrWhiteSpace(titleToFix))
			 {
				 IMDB movie;
					if (titleToFix.Contains(@"http://www.imdb.com/title"))
					{
						movie = new IMDB(titleToFix, true);
						FixMovie(movie);
					}
					else
					{
						try
							{
								movie = new IMDB(titleToFix);

								if (movie.Title.ToLower() == titleToFix.ToLower())
								{
								   FixMovie(movie);
								}
							}
							catch (Exception ex)
							{
								Log.Trace("Could not complete movie retrieval and insert", ex, titleToFix);
								//Log.Error("Could not complete movie insert, exception: [ {0} ]", ex.ToString());
							}
						}
					}
            
            
		}

		public static void FixMovie(IMDB movie)
		{
			try
			{
				Film dbMovie = (from film in Context.Films where film.Title == movie.Title select film).SingleOrDefault();
				if (dbMovie == null || dbMovie == new Film())
				{
					movie.AddMovieToCollection();
				}
				else
				{
					dbMovie.Plot = movie.Plot;
					dbMovie.ImdbURL = movie.ImdbURL;
					//dbMovie.Rating = movie.MpaaRating.Replace('_', '-');
				}
				Context.SaveChanges();

			}
			catch (Exception ex)
			{
				Log.Trace("Could not complete movie insert", movie.Title);
				Log.Error(ex);
				ex.ThrowFormattedException();

			}           
		}

		public static List<string> GetAllRatings()
		{
			return (from rating in Context.Ratings select rating.MPAARating).ToList();

		}
    }
}

#region Archived Code


//public static void AddPeoplFromFilm(this Film filmWithPeopleToAdd)
//{

//    foreach (PersonFilmIndex personFilmIndex in filmWithPeopleToAdd.PersonFilmIndexes)
//    {
//        Context.AddToPeople(personFilmIndex.Person);
//    }
//}


//public static List<Person> GetFilmCast(Film filmToSearchOn)
//{
//    return filmToSearchOn.PersonFilmIndexes.Select(index => index.Person).ToList();
//}



#endregion