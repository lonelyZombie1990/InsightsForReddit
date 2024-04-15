using Insights.Repository.Models;
using System.Collections.Concurrent;

namespace Insights.Domain.Interfaces
{
    public interface IReportManager
    {
        bool ReportStatisticsAsync();
        bool IsReportEnabled();
        bool ReportPostsWithMostVotes(ConcurrentBag<InsightPost> input);
        bool ReportUsersWithMostPosts(ConcurrentBag<UserPosts> input);
        bool CleanUpReport(bool cleanup);
    }
}