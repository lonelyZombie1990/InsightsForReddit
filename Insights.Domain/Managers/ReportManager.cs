using System.Collections.Concurrent;
using Insights.Domain.Interfaces;
using Insights.Repository.Models;
using Microsoft.Extensions.Logging;

namespace Insights.Domain.Managers;

public class ReportManager : IReportManager
{
    private ConcurrentBag<UserPosts> _popularUsers;
    private ConcurrentBag<InsightPost> _upVotes;
    private ConcurrentBag<UserPosts> _topUserPosts;
    private ILogger<ReportManager> _logger;
    public ReportManager(ILogger <ReportManager> logger)
    {
        _logger = logger;
    }

    public bool IsReportEnabled()
    {
        // We can use this to check hooks or other settings to see if the report is enabled
        return true;
    }

    public bool ReportPostsWithMostVotes(ConcurrentBag<InsightPost> _upVotes)
    {
        try
        {
            Console.WriteLine("Thread {0},    Reporting: Posts with Most Votes ", Thread.CurrentThread.ManagedThreadId);

            // Get the posts with the most upvotes
            var topUpvotedPosts = _upVotes.OrderByDescending(kv => kv.UpVotes).ToList();
            if(topUpvotedPosts.Count == 0)
            {
                Console.WriteLine("Thread {0},   No New Posts Found", Thread.CurrentThread.ManagedThreadId);
                return true;
            }
            foreach (var post in topUpvotedPosts)
            {
                Console.WriteLine($"\nThread {{0}},    User : {post.Author}, Vote Count: {post.UpVotes} \n", Thread.CurrentThread.ManagedThreadId);
                Task.Delay(10);//Delay for 10ms to simulate a long running task and visualize the non blocking nature of the async method as asked in the question

            }
            Console.WriteLine("Thread {0},   End Reporting: Posts with Most Votes  ",Thread.CurrentThread.ManagedThreadId);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;           
        }

    }




    public bool ReportUsersWithMostPosts(ConcurrentBag<UserPosts> _popularUsers)
    {
        try
        {
            Console.WriteLine("Thread {0},   Reporting: Users with Most Posts ", Thread.CurrentThread.ManagedThreadId);
            // Get the posts with the most upvotes
            var topUpvotedPosts = _popularUsers.OrderByDescending(kv => kv.postCount).ToList();
            if(topUpvotedPosts.Count == 0)
            {
                Console.WriteLine("Thread {0},   No New Posts Found", Thread.CurrentThread.ManagedThreadId);
                return true;
            }
            foreach (var post in topUpvotedPosts)
            {
                Console.WriteLine($"\nThread {{0}},    User : {post.Author}, Post Cont: {post.postCount} \n", Thread.CurrentThread.ManagedThreadId);
                Task.Delay(10);//Delay for 10ms to simulate a long running task and visualize the non blocking nature of the async method as asked in the question

            }
            Console.WriteLine("Thread {0},   End Reporting: Users with Most Posts ", Thread.CurrentThread.ManagedThreadId);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine( ex.Message);
            return false;
        }




    }

    public bool CleanUpReport(bool cleanup)
    {
        Console.WriteLine("Thread {0},   Cleaning up the report", Thread.CurrentThread.ManagedThreadId);
        if (cleanup)
        {
            // Every 10 minutes, clean up the report
            _upVotes = new ConcurrentBag<InsightPost>();
            _popularUsers = new ConcurrentBag<UserPosts>();
            _topUserPosts = new ConcurrentBag<UserPosts>();
            return true;
        }
        Console.WriteLine("Thread {0},   End Cleaning up the report", Thread.CurrentThread.ManagedThreadId);
        return false;

    }


    //Create a method that creates the Trend Report
    public void TrendReport()
    {
        //Console.WriteLine("Trend Report:");
        //// Get the posts with the most upvotes
        //var topUpvotedPosts = PostUpVotes.OrderByDescending(kv => kv.Value).Take(10);
        //Console.WriteLine("Top Upvoted Posts:");
        //foreach (var post in topUpvotedPosts)
        //{
        //    Console.WriteLine($"Post ID: {post.Key}, Upvotes: {post.Value}");
        //}

        //// Get the users with the most posts
        //var topPosters = UserPostCounts.OrderByDescending(kv => kv.Value).Take(10);
        //Console.WriteLine("Top Posters:");
        //foreach (var user in topPosters)
        //{
        //    Console.WriteLine($"User: {user.Key}, Post Count: {user.Value}");
        //}

        //Console.WriteLine();
    }

    public bool ReportStatisticsAsync()
    {
        return true;
    }
}

