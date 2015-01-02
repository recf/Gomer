using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
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
            bool? finished = null)
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

            IEnumerable<PileGame> games = this.Games;

            if (!string.IsNullOrWhiteSpace(name))
            {
                games = games.Where(g =>
                    g.Name.IndexOf(name, StringComparison.CurrentCultureIgnoreCase) >= 0
                    || (g.Alias ?? string.Empty).IndexOf(name, StringComparison.CurrentCultureIgnoreCase) >= 0);
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
                games = games.Where(g => (g.Genres ?? new string[0]).Any(gg => genres.Any(fg =>
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

            return games.ToList();
        }
    }
}
