
#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using HtmlAgilityPack;

#endregion

namespace Helpers
{
    public class IMDB
    {
        #region Properties
        
        private const string GoogleSearch = "http://www.google.com/search?q=imdb+";
        
        private const string BingSearch = "http://www.bing.com/search?q=imdb+";
        
        private const string AskSearch = "http://www.ask.com/web?q=imdb+";

        private string FullCreditsURL { get { return string.Format("{0}fullcredits#directors", ImdbURL); } }

        public bool Inserted { get; set; }

        public string Id { get; set; }

        public string Title { get; set; }

        public string Year { get; set; }

        public List<string> Genres { get; set; }

        public List<string> Directors { get; set; }

        public Dictionary<string, string> CastCharacters { get; set; }

        public List<string> Cast { get; set; }

        public List<string> Writers { get; set; }

        public List<string> Characters { get; set; }

        public string MpaaRating { get; set; }

        public string Plot { get; set; }

        public string Runtime { get; set; }

        public string ImdbURL { get; set; }

        #endregion

        #region Constructor
            
        public IMDB(string movieName, bool movieTitleIsIMDBUrl = false)
        {
            
            string imDbUrl = ( movieTitleIsIMDBUrl ) ? movieName : GetIMDBURL(Uri.EscapeUriString(movieName));
            
            if (string.IsNullOrEmpty(imDbUrl))
                return;

            ParseMovieFromHTML(GetHTMLFromURL(imDbUrl));
        }

        #endregion

        #region Methods
        
        private string GetIMDBURL(string MovieName, string searchEngine = "google")
        {
            //string url = GoogleSearch + MovieName;
            //if (searchEngine.ToLower().Equals("bing"))
            //    url = BingSearch + MovieName;
            //if (searchEngine.ToLower().Equals("ask"))
            //    url = AskSearch + MovieName;
            //ArrayList arrayList = MatchAll("<a href=\"(http://www.imdb.com/title/tt\\d{7}/)\".*?>.*?</a>", GetHTMLFromURL(url));
            //if (arrayList.Count > 0)
            //    return (string)arrayList[0];
            //if (searchEngine.ToLower().Equals("google"))
            //    return GetIMDBURL(MovieName, "bing");
            //if (searchEngine.ToLower().Equals("bing"))
            //    return GetIMDBURL(MovieName, "ask");
            //return string.Empty;

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(GetHTMLFromURL( string.Format("{0}{1}", GoogleSearch, MovieName)));

            if (doc.DocumentNode == null) return string.Empty;
            HtmlNode htmlNode = doc.DocumentNode.SelectNodes("//div[@class = 'kv']/cite").FirstOrDefault(node => node.InnerText.Contains("www.imdb.com/title/tt"));
            return htmlNode != null ? string.Format("http://{0}",htmlNode.InnerText.Trim()) : string.Empty;
        }

        private ArrayList MatchAll(string regex, string html, int i = 1)
        {
            ArrayList arrayList = new ArrayList();
            foreach (Match match in new Regex(regex, RegexOptions.Multiline).Matches(html))
                arrayList.Add(match.Groups[i].Value.Trim());
            return arrayList;
        }

        private string GetHTMLFromURL(string url)
        {
            WebClient webClient = new WebClient();
            if (!string.IsNullOrWhiteSpace(url))
            {
                // ReSharper disable AssignNullToNotNullAttribute
                StreamReader streamReader = new StreamReader(webClient.OpenRead(url));
                // ReSharper restore AssignNullToNotNullAttribute
                StringBuilder stringBuilder = new StringBuilder();
                while (!streamReader.EndOfStream)
                    stringBuilder.Append(streamReader.ReadLine());
                return (stringBuilder).ToString();
            }

            Log.Error( new Exception("URL provided was null or whitespace"));
            return string.Empty;

        }

        private void ParseMovieFromHTML( string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml( html );

            if (doc.DocumentNode == null) return;

            #region Id and URL

            HtmlNode htmlNode = doc.DocumentNode.SelectNodes("//link[@rel='canonical']").FirstOrDefault();
            if (htmlNode != null)
            {
                const string pattern = "http://www.imdb.com/title/(tt\\d{7})/";
                Match IDAndURL = Regex.Match(htmlNode.GetAttributeValue("href", string.Empty), pattern);
                ImdbURL = IDAndURL.Groups[0].ToString();
                Id = IDAndURL.Groups[1].ToString();
            }

            #endregion

            if (string.IsNullOrEmpty(Id)) return;

            #region Cast, Characters, Cast Characters, Genres

            // Cast
            Cast = doc.DocumentNode.SelectNodes("//td[@class='name']/a[contains(@href, 'name')]")
                .Select(node => HttpUtility.HtmlDecode(node.InnerText).Trim()).ToList();

            // Characters
            Characters = doc.DocumentNode.SelectNodes("//td[@class='character']/div")
                .Select(node => Regex.Replace(HttpUtility.HtmlDecode(node.InnerText).Trim(), @"\s+", " ")).ToList();

            // Genres
            Genres = doc.DocumentNode.SelectNodes("//a[contains(@itemprop, 'genre')]")
                               .Select(node => HttpUtility.HtmlDecode(node.InnerText).Trim()).ToList();

            // Cast Characters
            CastCharacters = Cast.Zip(Characters, (k, v) => new { k, v }).ToDictionary(x => x.k, x => x.v);
            
            #endregion

            #region Plot

            // Plot
            HtmlNodeCollection retrievedPlot = doc.DocumentNode.SelectNodes("//p[contains(@itemprop, 'description')]");
            if (retrievedPlot != null)
            {
                var firstOrDefault = retrievedPlot.FirstOrDefault();
                if (firstOrDefault != null)
                    Plot = HttpUtility.HtmlDecode(firstOrDefault.InnerText);
            }

            #endregion

            #region Duration

            // Duration
            HtmlNodeCollection durationNode = doc.DocumentNode.SelectNodes("//time[contains(@itemprop, 'duration')]");
            if (durationNode != null)
            {
                HtmlNode firstOrDefault = durationNode.FirstOrDefault();
                if (firstOrDefault != null)
                    Runtime = firstOrDefault.InnerText.Trim().Split().FirstOrDefault();
            }

            #endregion

            #region Rating and Length

            // Rating and length - if not found above
            HtmlNode orDefault = doc.DocumentNode.SelectNodes("//div[@class='infobar']").FirstOrDefault();
            if (orDefault != null)
            {
                MpaaRating = orDefault.FirstChild.GetAttributeValue("title", string.Empty).Replace('_', '-');
                if (string.IsNullOrWhiteSpace(MpaaRating))
                {
                    MpaaRating = "NOT-RATED";
                }
                if (string.IsNullOrWhiteSpace(Runtime))
                {
                    string runtime = HttpUtility.HtmlDecode(orDefault.InnerText).Trim().Split(' ').ToList().FirstOrDefault();
                    double Num;

                    bool isNum = double.TryParse(runtime, out Num);

                    Runtime = isNum ? runtime : "0";


                }
                    
            }

            #endregion

            #region Title and Year

            // Title and year
            HtmlNode metaTag = doc.DocumentNode.SelectNodes("//meta[@name='title']").FirstOrDefault();
            if (metaTag != null)
            {
                string content = metaTag.GetAttributeValue("content", string.Empty);
                const string pattern = @"(.*)\(.*(\d{4}).*\).*";
                Match matches = Regex.Match(content, pattern);

                try
                {
                    Title = HttpUtility.HtmlDecode(matches.Groups[1].ToString());
                    Year = HttpUtility.HtmlDecode(matches.Groups[2].ToString());
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    ex.ThrowFormattedException();
                    throw;
                }
            }
            //a[contains(@itemprop, 'director')]")
            ////td[@class='character']/div/a[contains(@href, 'character')] |

            #endregion

            #region Directors and Writers

            //Check for extra directors
            var directorCreditNode =
                doc.DocumentNode.SelectNodes("//a[contains(@href, 'fullcredits#directors')]");
            if (directorCreditNode == null)
            {
                // get writer node
                var writerCreditNode =
                    doc.DocumentNode.SelectNodes("//a[contains(@href, 'fullcredits#writers')]");
                if (writerCreditNode == null) // No extras Writers or Directors
                {
                    Directors = doc.DocumentNode.SelectNodes("//a[@itemprop = 'director']").Select(node => HttpUtility.HtmlDecode(node.InnerText).Trim()).ToList();

                    Writers = doc.DocumentNode.SelectNodes("//a[contains(@onclick, 'writer')]")
                    .Select(node => HttpUtility.HtmlDecode(node.InnerText).Trim()).ToList();

                    //writers
                }
                else // More Writers
                {
                    GetWritersAndDirectorFromFullCredits();    
                }
            }
            else // More Directors 
            {
                GetWritersAndDirectorFromFullCredits();
            }

            #endregion
        }

        private void GetWritersAndDirectorFromFullCredits()
        {
            //determin if more than shown directors
            string fullCreditsHTML = GetHTMLFromURL(FullCreditsURL);
            HtmlDocument fullCreditsDoc = new HtmlDocument();
            fullCreditsDoc.LoadHtml(fullCreditsHTML);

            if (fullCreditsDoc.DocumentNode != null)
            {

                Directors =
                    fullCreditsDoc.DocumentNode.SelectNodes(
                        "/html[1]/body[1]/div[1]/div[1]/layer[1]/div[4]/div[3]/div[3]/div[2]/table[1]/tr/td/a[contains(@href, 'name')]")
                        .Select(node => HttpUtility.HtmlDecode(node.InnerText).Trim()).ToList();

                // TODO - get what the actors wrote in there somehow

                Writers =
                    fullCreditsDoc.DocumentNode.SelectNodes(
                        "/html[1]/body[1]/div[1]/div[1]/layer[1]/div[4]/div[3]/div[3]/div[2]/table[2]/tr/td/a[contains(@href, 'name')]")
                        .Select(node => HttpUtility.HtmlDecode(node.InnerText).Trim()).ToList();
            }
        }

        #endregion
    }
}


#region Archived Code

//Random random = new Random();
//((NameValueCollection)webClient.Headers)["X-Forwarded-For"] = 
//    (string)(object)random.Next(0, (int)byte.MaxValue) + 
//    (object)"." + (string)(object)random.Next(0, (int)byte.MaxValue) + "." + 
//    (string)(object)random.Next(0, (int)byte.MaxValue) + "." + (string)(object)random.Next(0, (int)byte.MaxValue);
//((NameValueCollection)webClient.Headers)["User-Agent"] = "Mozilla/" + (object)random.Next(3, 5) + ".0 (Windows NT " + (string)(object)random.Next(3, 5) + "." + (string)(object)random.Next(0, 2) + "; rv:2.0.1) Gecko/20100101 Firefox/" + (string)(object)random.Next(3, 5) + "." + (string)(object)random.Next(0, 5) + "." + (string)(object)random.Next(0, 5);

//private ArrayList getReleaseDates()
//{
//    ArrayList arrayList = new ArrayList();
//    foreach (string input in this.matchAll("<tr>(.*?)</tr>", this.match("Date</th></tr>\\n*?(.*?)</table>", this.getUrlData("http://www.imdb.com/title/" + this.Id + "/releaseinfo"), 1), 1))
//    {
//        Match match = new Regex("<td>(.*?)</td>\\n*?.*?<td align=\"right\">(.*?)</td>", RegexOptions.Multiline).Match(input);
//        arrayList.Add((object)(IMDB.StripHTML(match.Groups[1].Value.Trim()) + " = " + IMDB.StripHTML(match.Groups[2].Value.Trim())));
//    }
//    return arrayList;
//}

//public List<string> Writers { get; set; }
//public string OriginalTitle { get; set; }

//public string ReleaseDate { get; set; }

//public string Poster { get; set; }

//public string PosterLarge { get; set; }

//public string PosterSmall { get; set; }

//public string PosterFull { get; set; }


//public string Top250 { get; set; }

//public string Oscars { get; set; }

//public string Awards { get; set; }

//public string Nominations { get; set; }

//public string Storyline { get; set; }

//public string Tagline { get; set; }

//public string Votes { get; set; }

//public ArrayList Languages { get; set; }

//public ArrayList Countries { get; set; }

//public ArrayList ReleaseDates { get; set; }

//public ArrayList MediaImages { get; set; }

//public ArrayList RecommendedTitles { get; set; }



#endregion