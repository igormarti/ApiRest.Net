using System.ComponentModel.DataAnnotations.Schema;

namespace RestApiWithDontNet.Models
{
    [Table("users")]
    public class User
    {
        [Column("id")]
        public long Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("address")]
        public string Address { get; set; }

        [Column("gender")]
        public string ?Gender { get; set; }

    }
}
