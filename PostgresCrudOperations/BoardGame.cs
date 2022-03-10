namespace PostgresCrudOperations
{
    public class BoardGame
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public int AverageDuration { get; set; }
    }
}