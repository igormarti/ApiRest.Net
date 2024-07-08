using System.ComponentModel.DataAnnotations.Schema;

namespace RestApiWithDontNet.Models.Base
{
    public class BaseEntity
    {
        [Column("id")]
        public long Id { get; set; }

    }
}
