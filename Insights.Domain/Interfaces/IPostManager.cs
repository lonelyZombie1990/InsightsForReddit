
using Insights.Repository.Models;
namespace Insights.Domain.Managers;
public interface IPostManager
{
    IEnumerable<UserPosts> GetRedditTopUserPostsInfo(int count);
    IEnumerable<UserPosts> GetRedditUsersWithMostPosts(int count);
    IEnumerable<InsightPost> GetRedditPostsWithMostUpVotes(int count);
}

