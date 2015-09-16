using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Popcorn.Entity.Movie
{
    /// <summary>
    /// Represents a movie genre
    /// </summary>
    public class Genre
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}