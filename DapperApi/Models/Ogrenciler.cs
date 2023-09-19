using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DapperApi
{
    [Table("Ogrenciler")]
    public class Ogrenciler
    {
        [Key]
        public int id { get; set; }
        public string ad { get; set; }
        public int yas { get; set; }
    }
}
