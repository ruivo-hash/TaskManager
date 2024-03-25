namespace TaskManager.Endpoints.Tasks
{
    public class TasksResponse
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public DateTime CreateDt { get; set; }
        public DateTime FinishDt { get; set; }
    }
}