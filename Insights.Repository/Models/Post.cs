using Newtonsoft.Json;

namespace Insights.Repository.Models;

public class InsightPost
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("author")]
    public string Author { get; set; }

    [JsonProperty("ups")]
    public int UpVotes { get; set; }
    public DateTime Created { get; set; }
}

