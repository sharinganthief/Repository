using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using DB;


namespace Helpers
{
    public static class SearchHelpers
    {
		//public static readonly int MaxLength = MovieAppHelpers.GetMaxMovieLength().ToIntSafe();
		//public static readonly int MinLength = MovieAppHelpers.GetMinMovieLength().ToIntSafe();


        public static IList<Film> RunSearch(
        string paramActorLast = null,
        string paramActorFirst = null,
        string paramWriterLast = null,
        string paramWriterFirst = null,
        string paramDirectorLast = null,
        string paramDirectorFirst = null,
        List<string> paramGenres = null,
        List<string> paramRatings = null,
        int paramMax = 0,
        int paramMin = 0)
        {

            using (MovieCollectionEntities context = new MovieCollectionEntities())
            {
                IQueryable<Film> mq = context.Films.Include("Rating")
                                                    .Include("GenreFilmIndexes.Genre")
                                                    .Include("PersonFilmIndexes.Person")
                                                    .Select(o => o);
                if (paramRatings != null && paramRatings.Count > 0) mq = mq.Where(o => paramRatings.Contains(o.Rating.MPAARating));
                if (paramMin > 0) mq = mq.Where(o => o.Length >= paramMin);
                if (paramMax > 0) mq = mq.Where(o => o.Length <= paramMax);
                if (paramGenres != null && paramGenres.Count > 0)
                    mq = mq.Where(o => paramGenres.Any(gn => (o.GenreFilmIndexes
                    .Select(gfi => gfi.Genre.Name)).Contains(gn)));

				mq = mq.SearchByPersonNameAndRole(paramActorFirst, FilmRole.ActorRole.RoleID, true);
				mq = mq.SearchByPersonNameAndRole(paramActorLast, FilmRole.ActorRole.RoleID);

				mq = mq.SearchByPersonNameAndRole(paramWriterFirst, FilmRole.ActorRole.RoleID, true);
				mq = mq.SearchByPersonNameAndRole(paramWriterLast, FilmRole.ActorRole.RoleID);

				mq = mq.SearchByPersonNameAndRole(paramDirectorFirst, FilmRole.DirectorRole.RoleID, true);
				mq = mq.SearchByPersonNameAndRole(paramDirectorLast, FilmRole.DirectorRole.RoleID);

                return mq.Distinct().ToList();

            }
        }

        private static IQueryable<Film> SearchByPersonNameAndRole(this IQueryable<Film> queryable, string personName, int roleId, bool firstName = false)
        {
            return
                !string.IsNullOrEmpty(personName)
                ? queryable.Where(o => o.PersonFilmIndexes
                                                .Any(pfi =>  (firstName ? pfi.Person.FirstName == personName 
                                                                        : pfi.Person.LastName == personName) 
                                                                        && pfi.RoleID == roleId)) :
                queryable;
        }
    }
}

#region Archived Code

// Length
//List<bool> x = context.Films.Where(LengthQuery, min, max)


//Expression<Func<MovieCollectionEntities, bool>> predicate = PredicateBuilder.True<MovieCollectionEntities>();
//IQueryable<Film> prod = context.Films.AsExpandable();

//if (!genreName.IsNullOrWhiteSpace())
//    prod = prod.Join(context.GenreFilmIndexes, film => film.FilmId, index => index.FilmId, (film, index) => index.GenreId)
//               .Join(context.Genres, id => id, genre => genre.GenreId, (i, genre) => genre.Name == "GenreName"); 

// Genre

// Actor

//


//filmsToReturn = context.Films.Where(predicate).ToList();
//LinqTextQueryBuilder<MovieCollectionEntities> queryBuilder = new LinqTextQueryBuilder<MovieCollectionEntities>();
//queryBuilder.Profiler.AddNamespace("MovieCollectionEntities");
//queryBuilder.Profiler.AddNamespace("Helpers");
//queryBuilder.SetSource(context);

////string queryText = string.Format("(from movie in context.Films.Where({0}, {1}, {2})", LengthQuery, min, max);
//////if(!string.IsNullOrWhiteSpace(genreName))
//////queryText += " join gfi in context.GenreFilmIndexes on movie.FilmId equals gfi.FilmId" +
//////             " join genre in context.Genres where on gfi.GenreId equals genre.GenreId";
//////if(!string.IsNullOrWhiteSpace(personFirst))
//////queryText += " join pfi in context.PersonFilmIndexes on movie.FilmId equals pfi.FilmId" +
//////             " join person in context.People.Where(FirstName == @0 paramPersonFirst) on pfi.PersonId equals person.PersonId";

////queryText += " select new Film() { Title = movie.Title,Year = movie.Year, Length = movie.Length } ).ToList();";



//string queryText = @"from movie in context.Films select movie";
//var result = queryBuilder.Query(queryText);



//List<Film> selectAllQry = (from movie in context.Films
//                                select movie).Distinct().ToList();

//    //IQueryable<GenreFilmIndex> genreFilmIndexes = from gfi in context.GenreFilmIndexes select gfi;
//    //IQueryable<Genre> genres = from gfi in context.Genres select gfi;


//    //var x = selectAllQry.Join(genreFilmIndexes, movie => movie.FilmId, gfi => gfi.FilmId,
//    //                          (movie, gfi) => movie);

//List<Film> results =
//(from movie in context.Films
// //.Where("Title == @0","Se7en")//.Where(LengthQuery, min, max)
// join gfi in context.GenreFilmIndexes on movie.FilmId equals gfi.FilmId
// join genre in context.Genres.Where("Name == @0", genreName) on gfi.GenreId equals genre.GenreId
// join pfi in context.PersonFilmIndexes on movie.FilmId equals pfi.FilmId
// join person in context.People.Where("FirstName == @0", paramPersonFirst) on pfi.PersonId equals person.PersonId  //works, just need to make it better :)
// select new Film()
//           {
//               Title = movie.Title,
//               Year = movie.Year,
//               Length = movie.Length
//               //Person = person.FirstName + ", " + person.LastName
//           }
//     ).ToList();

//var source = personList.AsQueryable();

#endregion