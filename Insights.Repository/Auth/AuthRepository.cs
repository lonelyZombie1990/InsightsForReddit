
using Insights.Repository.Models;
using Microsoft.Extensions.Logging;
using Reddit.AuthTokenRetriever;
using System.Diagnostics;
using System.Runtime.ExceptionServices;

namespace Insights.Repository.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ILogger<AuthRepository> _logger;
        public AuthRepository(ILogger<AuthRepository> logger)
        {
            _logger = logger;

        }
        // This method is used to get the access token from Reddit.
        // It uses the AuthTokenRetrieverLib library to open a browser to the Reddit authentication page, where the user can click "accept" to allow the app to access their Reddit account.
        // Reddit will then redirect the browser to localhost:8080, where the library will take over and retrieve the access token.

        // PLEASE NOTE THAT THIS IS ONLY USED TO GET ACCESS TOKENS, I AM USING ACCESS TOKEN IN ENVIRONMENT VARIABLES IN MY PROJECT.
        // I USED THIS TO GET MY FIRST ACCESS TOKEN AND THEN STORED IT IN ENVIRONMENT VARIABLES.
        // YOU CAN USE THIS TO GET ACCESS TOKENS FOR YOUR PROJECTS, BUT MAKE SURE TO STORE THEM IN A SECURE WAY.
        // ALSO, MAKE SURE TO KEEP YOUR CLIENT SECRET SECURE.
        // YOU CAN USE ENVIRONMENT VARIABLES TO STORE YOUR CLIENT SECRET -- I AM USING IT FOR NOW
        // YOU CAN ALSO USE AZURE KEY VAULT TO STORE YOUR CLIENT SECRET
        // YOU CAN ALSO USE USER SECRETS TO STORE YOUR CLIENT SECRET LOCALLY (I AM NOT USING IT FOR NOW)

        public  bool AuthorizeUser(AuthInfo authinfo)
        {
            try
            {
                _logger.LogInformation($"Initiating Authentication for Client {authinfo.ClientId}");
                // Create a new instance of the auth token retrieval library.  --Kris
                AuthTokenRetrieverLib authTokenRetrieverLib = new(appId: authinfo.ClientId, port: 8080, null, null, appSecret: authinfo.ClientSecret);
                // Note - Ignore the logging exception message if you see it.  You can use Console.Clear() after this call to get rid of it if you're running a console app.
                var authURL = authTokenRetrieverLib.AuthURL();
                OpenBrowser(authURL);
                authTokenRetrieverLib.AwaitCallback();
                // Open the browser to the Reddit authentication page.  Once the user clicks "accept", Reddit will redirect the browser to localhost:8080, where AwaitCallback will take over.  --Kris
                authTokenRetrieverLib.StopListening();
                var AccessToken = authTokenRetrieverLib.AccessToken;
                var RefreshToken = authTokenRetrieverLib.RefreshToken;
                _logger.LogInformation($"Authentication Sucessful for Client {authinfo.ClientId}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in Autherizing Client {0}",authinfo.ClientId);
                _logger.LogError(ex.Message);
               return false;
            }
        }

        public static void OpenBrowser(string url)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
        public  string GetAccessToken()
        {

            return Environment.GetEnvironmentVariable("REDDIT_ACCESS_TOKEN");
        }
        public  string GetRefreshToken()
        {
            return Environment.GetEnvironmentVariable("REDDIT_REFRESH_TOKEN");
        }
    }
}
