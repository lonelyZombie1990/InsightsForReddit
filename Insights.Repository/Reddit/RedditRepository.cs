using Insights.Repository.Auth;
using Insights.Repository.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Reddit;
using Reddit.Controllers;


namespace Insights.Repository.Reddit;

/// <summary>
/// Repository for Reddit posts.
/// </summary>
public class RedditRepository : IDisposable
{
    private readonly HttpClient _httpClient;
    private RedditClient _reddit;
    private readonly ILogger<RedditRepository> _logger;
    private readonly AuthInfo _authInfo;
    private readonly IAuthRepository _authRepository;
    private Lazy<bool> IsAuthorized;

    public RedditRepository(IAuthRepository authRepository, IOptions<AuthInfo> authInfoOption, ILogger<RedditRepository> logger)
    {
        _authInfo = authInfoOption.Value;
        _logger = logger;
        _authRepository = authRepository;

        if (string.IsNullOrWhiteSpace(_authRepository.GetAccessToken()))
        {
            _reddit = new RedditClient(appId: _authInfo.ClientId, appSecret: _authInfo.ClientSecret);
            IsAuthorized = new Lazy<bool>(() => _authRepository.AuthorizeUser(_authInfo));
            return;
        }

        _reddit = new RedditClient(_authInfo.ClientId, _authRepository.GetRefreshToken(), _authInfo.ClientSecret, _authRepository.GetAccessToken());
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
        GC.SuppressFinalize(this);
    }

    public IEnumerable<InsightPost> GetPostsWithMostUpVotes(int count = 10)
    {
        try
        {
            _logger.LogInformation("RedditReposity: Get User with Most Up Votes");
            Subreddit subreddit = _reddit.Subreddit("funny");

            var topPosts = subreddit.Posts.Top;
            List<InsightPost> postsWithMostUpvotes = topPosts.OrderByDescending(p => p.Score).Take(count).Select(x => new InsightPost
            {
                Author = x.Author,
                UpVotes = x.Score
            }).ToList();

            _logger.LogInformation("RedditReposity: GetPosts");
            return postsWithMostUpvotes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RedditReposity: GetPosts");
        }

        return null;
    }

    public IEnumerable<UserPosts> GetUsersWithMostPosts(int count = 10)
    {
        try
        {
            _logger.LogInformation("RedditReposity: Get User with Most Posts");
            Subreddit subreddit = _reddit.Subreddit("funny");
            var topPosters = subreddit.Posts.INew.Take(count) // Adjust the number of posts to fetch as needed
                .GroupBy(post => post.Author)
                .OrderByDescending(group => group.Count())
                .Select(group => new UserPosts { Author = group.Key, postCount = group.Count() })
                .ToList();
            return topPosters;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RedditReposity: GetUsersWithMostPosts");
        }

        // Return null if an exception occurs
        return null;
    }

    public IEnumerable<UserPosts> GetTopPostsInfo(int count = 10)
    {
        try
        {
            _logger.LogInformation("RedditReposity: GetPosts");
            Subreddit subreddit = _reddit.Subreddit("funny");

            var topPosts = subreddit.Posts.Top;
            var postsWithMostUpvotes = topPosts.OrderByDescending(p => p.Score).Take(count);

            var usersWithMostPosts = topPosts
                .Select(p => p.Author)
                .Where(author => author != null)
                .GroupBy(author => author)
                .OrderByDescending(group => group.Count())
                .Take(count)
                .Select(group => new UserPosts { Author = group.Key, postCount = group.Count() }).ToList();
            _logger.LogInformation("RedditReposity: GetPosts");
            return usersWithMostPosts;


        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RedditReposity: GetPosts");
        }
        // Return null if an exception occurs
        return null;
    }
}

