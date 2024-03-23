namespace TaskManager.Domain;

public class Tasks
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreateDt { get; set; }
    public DateTime FinishDt { get; set; }
    public int UserId { get; set; }

    public Tasks(string title, string description, DateTime createDt, DateTime finishDt, int userId)
    {
        Title = title;
        Description = description;
        CreateDt = createDt;
        FinishDt = finishDt;
        UserId = userId;
    }
}
