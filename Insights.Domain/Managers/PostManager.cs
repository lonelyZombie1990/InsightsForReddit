using Insights.Repository.Models;
using Insights.Repository.Reddit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Insights.Domain.Managers
{
    /// <summary>
    /// Manager class for processing posts from Reddit.
    /// </summary>
    public class PostManager : IPostManager
    {
        private RedditRepository _redditRepository;
        private readonly ILogger _logger;


        public PostManager(RedditRepository redditRepository, ILogger<PostManager> logger )
        {
            _redditRepository = redditRepository;
            _logger = logger;
        }

        private ConcurrentDictionary<string, string> _redditPosts = new ConcurrentDictionary<string, string>();

        public  IEnumerable<UserPosts>  GetRedditTopUserPostsInfo(int count)
        {
            return _redditRepository.GetTopPostsInfo(count);
        }
        public IEnumerable<UserPosts>  GetRedditUsersWithMostPosts(int count)
        {
            return _redditRepository.GetUsersWithMostPosts(count);

        }
        public IEnumerable<InsightPost>   GetRedditPostsWithMostUpVotes(int count)
        {
           return _redditRepository.GetPostsWithMostUpVotes(count);
        }
    }
}
