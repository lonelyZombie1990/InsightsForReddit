using Newtonsoft.Json;

namespace Insights.Repository.Models;


/// <summary>
/// Represents a user's posts.
/// </summary>
public class UserPosts
{
    [JsonProperty("userid")]
    public string Id { get; set; }

    [JsonProperty("author")]
    public string Author { get; set; }

    [JsonProperty("postCount")]
    public int postCount { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the post was created.
    /// </summary>
    public DateTime Created { get; set; }
}

