using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insights.Repository.Constants;
    /// <summary>
    /// Contains the URLs for various Reddit endpoints.
    /// </summary>
    public static class RedditURLs
    {
        // URLs for Reddit user endpoints
        public const string RedditBaseURL = "https://www.reddit.com";
        public const string RedditLoginURL = "https://www.reddit.com/login";
        public const string RedditLoginPostURL = "https://www.reddit.com/api/login";
        public const string RedditUserURL = "https://www.reddit.com/user";
        public const string RedditUserCommentsURL = "https://www.reddit.com/user/comments";
        public const string RedditUserSubmissionsURL = "https://www.reddit.com/user/submitted";
        public const string RedditUserUpvotedURL = "https://www.reddit.com/user/upvoted";
        public const string RedditUserDownvotedURL = "https://www.reddit.com/user/downvoted";
        public const string RedditUserHiddenURL = "https://www.reddit.com/user/hidden";
        public const string RedditUserSavedURL = "https://www.reddit.com/user/saved";
        public const string RedditUserGildedURL = "https://www.reddit.com/user/gilded";

    }

