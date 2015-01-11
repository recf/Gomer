using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Gomer.Core
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Pile
    {
        public Pile()
        {
            Games = new List<PileGame>();

            Version = Assembly.GetExecutingAssembly()
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                .InformationalVersion;
        }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("games", Required = Required.Always)]
        public IList<PileGame> Games { get; set; }

        public IList<PileGame> Search(
            string name = null, 
            List<string> platforms = null, 
            List<int> priorities = null,
            List<string> genres = null, 
            bool? playing = null,
            bool? finished = null,
            DateTime? addedOnOrAfter = null,
            DateTime? addedBeforeOrOn = null,
            DateTime? finishedOnOrAfter = null, 
            DateTime? finishedBeforeOrOn = null, 
            bool? hidden = false)
        {
            if (platforms == null)
            {
                platforms = new List<string>();
            }

            if (priorities == null)
            {
                priorities = new List<int>();
            }

            if (genres == null)
            {
                genres = new List<string>();
            }

            IEnumerable<PileGame> games = Games;
            if (hidden.HasValue)
            {
                games = games.Where(g => g.IsHidden == hidden);
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                var pattern = GetPattern(name);
                var regex = new Regex(pattern, RegexOptions.IgnoreCase);

                games = games.Where(g => regex.IsMatch(g.Name));
            }

            if (platforms.Any())
            {
                games =
                    games.Where(
                        g =>
                            platforms.Any(p => string.Equals(g.Platform, p, StringComparison.InvariantCultureIgnoreCase)));
            }

            if (priorities.Any())
            {
                games = games.Where(g => priorities.Any(p => g.Priority == p));
            }

            if (genres.Any())
            {
                games = games.Where(g => (g.Tags ?? new string[0]).Any(gg => genres.Any(fg =>
                    gg.IndexOf(fg, StringComparison.CurrentCultureIgnoreCase) >= 0)));
            }

            if (playing.HasValue)
            {
                games = games.Where(g => g.Playing == playing.Value);
            }

            if (finished.HasValue)
            {
                games = games.Where(g => g.FinishedDate.HasValue == finished.Value);
            }

            if (addedOnOrAfter.HasValue)
            {
                games = games.Where(g => g.AddedDate >= addedOnOrAfter.Value);
            }

            if (addedBeforeOrOn.HasValue)
            {
                games = games.Where(g => g.AddedDate <= addedBeforeOrOn.Value);
            }

            if (finishedOnOrAfter.HasValue)
            {
                games = games.Where(g => g.FinishedDate.HasValue && g.FinishedDate.Value >= finishedOnOrAfter.Value);
            }

            if (finishedBeforeOrOn.HasValue)
            {
                games = games.Where(g => g.FinishedDate.HasValue && g.FinishedDate.Value <= finishedBeforeOrOn.Value);
            }

            return games.ToList();
        }

        private static string GetPattern(string term)
        {
            var prefixes = new[] { '=', '*', '~' };
            var suffixes = new[] { '*' };

            var prefix = term.First();
            var suffix = term.Last();

            var search = term;
            if (prefixes.Contains(prefix))
            {
                search = search.Substring(1);
            }

            if (suffixes.Contains(suffix))
            {
                search = search.Substring(0, search.Length - 1);
            }

            if (prefix == '=')
            {
                return "^" + search + "$";
            }

            if (prefix == '*' && suffix != '*')
            {
                return search + "$";
            }

            if (prefix != '*' && suffix == '*')
            {
                return "^" + search;
            }

            if (prefix == '*' && suffix == '*')
            {
                return search;
            }

            return string.Join(".*", search.ToCharArray());
        }
    }
}
