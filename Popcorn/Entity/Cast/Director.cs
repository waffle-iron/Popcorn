using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Popcorn.Entity.Cast
{
    /// <summary>
    /// Represents a movie director
    /// </summary>
    public class Director
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string SmallImage { get; set; }
        public string MediumImage { get; set; }
        public string SmallImagePath { get; set; }
    }
}
