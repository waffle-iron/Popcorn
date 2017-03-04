using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Popcorn.Models.Movie
{
    public class MovieResponse
    {
        [JsonProperty("totalMovies")]
        public int TotalMovies { get; set; }

        [JsonProperty("movies")]
        public List<MovieJson> Movies { get; set; }
    }
}
