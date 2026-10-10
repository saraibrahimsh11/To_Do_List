namespace To_Do_List.Models
{
    public class ToDoList
    {
        public int Id { get; set; }
        public string Tiltle { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public string FileUrl { get; set; } = "default.jpg";
    }
}
