using Newtonsoft.Json;

namespace WebApiOne.Api.Operations;

public class CommonEntity
{
    [JsonProperty(PropertyName = "id")] 
    public string? Id { get; set; }
    [JsonProperty(PropertyName = "partitionKey")]
    public string? PartitionKey { get; set; }

    public int Throughput { get; set; }
}
