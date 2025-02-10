namespace WebApiOne.Api.Operations;

public class LogEntity : CommonEntity
{
    public LogEntity()
    {
        Id = Guid.NewGuid().ToString();
        PartitionKey = nameof(LogEntity);
    }
    public string? Message { get; set; }
    public string? LongMessage { get; set; }
    public string? Path { get; set; }
    public string? Source { get; set; }
    public string? UserId { get; set; }
    public int? StatusCode { get; set; }
    public DateTime? CreateDate { get; set; }
}
