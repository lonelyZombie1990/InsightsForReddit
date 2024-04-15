using Insights.Domain.Interfaces;
using Insights.Domain.Managers;
using Insights.Repository.Models;
using System.Collections.Concurrent;
namespace Insights.Service;



public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IAuthManager _authManager;
    private readonly IPostManager _postManager;
    private readonly IReportManager _reportManager;
    private Task _postworkerTask; // Add a private field to hold the worker task
    private Task _reportworkerTask; // Add a private field to hold the report task
    private Task _cleanUPTask;
    private ConcurrentBag<UserPosts> _popularUsers;
    private ConcurrentBag<InsightPost> _upVotes;
    private ConcurrentBag<UserPosts> _topUserPosts;
    public Worker(IAuthManager authManager, IPostManager postManager, IReportManager reportManager, ILogger<Worker> logger)
    {
        _postManager = postManager;
        _reportManager = reportManager;
        _authManager = authManager;
        _logger = logger;
        _popularUsers = new ConcurrentBag<UserPosts>();
        _upVotes = new ConcurrentBag<InsightPost>();
        _topUserPosts = new ConcurrentBag<UserPosts>();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _postworkerTask = Task.Run(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                IEnumerable<Repository.Models.UserPosts> popularUsers = _postManager.GetRedditUsersWithMostPosts(100);
                foreach (var userPosts in popularUsers)
                {
                    _popularUsers.Add(userPosts);
                }
                IEnumerable<Repository.Models.InsightPost> upVotes = _postManager.GetRedditPostsWithMostUpVotes(100);
                upVotes.ToList().ForEach(x => _upVotes.Add(x));
                IEnumerable<Repository.Models.UserPosts> topUserPosts = _postManager.GetRedditTopUserPostsInfo(100);
                foreach (var userPosts in topUserPosts)
                {
                    _topUserPosts.Add(userPosts);
                }
                await Task.Delay(1000, stoppingToken);
            }
        }, stoppingToken);

        _reportworkerTask = Task.Run(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var reports = _reportManager.ReportStatisticsAsync();
                _reportManager.ReportPostsWithMostVotes(_upVotes);
                _reportManager.ReportUsersWithMostPosts(_popularUsers);
                await Task.Delay(5000, stoppingToken);
            }
        }, stoppingToken);

        _cleanUPTask = Task.Run(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(10000, stoppingToken);
                _reportManager.CleanUpReport(true);
                await Task.Delay(10000, stoppingToken);
            }
        }, stoppingToken);

        return Task.CompletedTask; // Add a return statement to indicate task completion
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        if (_postworkerTask != null)
        {
            await Task.WhenAny(_postworkerTask, Task.Delay(Timeout.Infinite, stoppingToken));
        }

        if (_reportworkerTask != null)
        {
            await Task.WhenAny(_reportworkerTask, Task.Delay(Timeout.Infinite, stoppingToken));
        }
        if (_cleanUPTask != null)
        {
            await Task.WhenAny(_cleanUPTask, Task.Delay(Timeout.Infinite, stoppingToken));
        }

        await base.StopAsync(stoppingToken);
    }
}
