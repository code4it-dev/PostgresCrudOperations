using System.ComponentModel.DataAnnotations.Schema;

namespace PostgresCrudOperations
{
    // Lowercase  https://github.com/npgsql/efcore.pg/issues/21
    //[Table("games")]
    public class BoardGame
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("minplayers")]
        public int MinPlayers { get; set; }

        [Column("maxplayers")]
        public int MaxPlayers { get; set; }

        [Column("averageduration")]
        public int AverageDuration { get; set; }
    }
}