using RestApiWithDontNet.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestApiWithDontNet.Models
{
    [Table("users")]
    public class User:BaseEntity
    {

        [Column("name")]
        public string Name { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("address")]
        public string Address { get; set; }

        [Column("gender")]
        public string ?Gender { get; set; }

        [Column("enabled")]
        public bool Enabled { get; set; }
    }
}
