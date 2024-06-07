namespace Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool Enable { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime? LastChangeDate { get; set; }
    }
}