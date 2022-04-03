namespace Project_Manager.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public bool Urgency { get; set; }

        public int TodoId { get; set; }
        public Todo Todo { get; set; }
    }
}
