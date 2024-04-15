using Insights.Domain.Interfaces;
using Insights.Repository.Auth;
using Insights.Repository.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insights.Domain.Managers
{
    public class AuthManager : IAuthManager
    {

        private readonly IAuthRepository _authRepository;
        private readonly ILogger<AuthManager> _logger; // Update the logger type to ILogger<AuthManager>
        private readonly IOptions<AuthInfo> _authinfoOption;

        public AuthManager(IAuthRepository authRepository, IOptions<AuthInfo> authinfoOption, ILogger<AuthManager> logger) // Update the logger parameter type to ILogger<AuthManager>
        {
            _authinfoOption = authinfoOption;
            _logger = logger;
            _authRepository = authRepository;
        }
        /// <summary>
        /// This methods queries and retrieves the access token from the Reddit API for the given credentials
        /// </summary>
        /// <returns>String Representation of Access Token</returns>
        public  string GetRedditAccessToken()
        {
            var accessToken =  _authRepository.GetAccessToken();
            return accessToken;
        }

    }
}
