using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Business;
using NHibernate.Linq;

namespace Helpers
{
    public static class DatabaseHelpers
    {
        public static void Delete(Movie existingMovie)
        {
            using (var session = NHibernateHelpers.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Delete(existingMovie);
                    transaction.Commit();
                    MessageBox.Show("Deleted Movie: " + existingMovie.Title);
                }
            }
        }

        public static void Update(Movie newMovie)
        {
            using (var session = NHibernateHelpers.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(newMovie);
                    transaction.Commit();
                    MessageBox.Show("Updated Movie: " + newMovie.Title);
                }
            }
        }

        public static Movie Read(string MovieTitle)
        {
            using (var session = NHibernateHelpers.OpenSession())
            {
                Movie MovieQuery = new Movie();
                try
                {
                    MovieQuery = (from movie in session.Query<Movie>()
                                  where movie.Title == MovieTitle
                                  select movie).Single();
                }
                catch (Exception exception)
                {
                    return new Movie();
                }

                //NhQueryProvider.cs

                MessageBox.Show("Read Movie: " + MovieQuery.Title);
                return MovieQuery;
            }
        }

        public static void Create(Movie newMovie)
        {

            Movie tryRead = Read(newMovie.Title);
            if (tryRead != new Movie())
            {

                using (var session = NHibernateHelpers.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        var Movie = new Movie
                        {
                            Title = newMovie.Title,
                            Runtime = newMovie.Runtime,
                            ReleaseYear = newMovie.ReleaseYear,

                        };
                        session.Save(Movie);

                        transaction.Commit();
                        MessageBox.Show("Created Movie: " + Movie.Title);

                    }
                }
            }
            else
            {
                MessageBox.Show("Already Exists! Movie: " + newMovie.Title);
            }
        }
    }
}