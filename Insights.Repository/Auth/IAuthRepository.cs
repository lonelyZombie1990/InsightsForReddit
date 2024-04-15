using Insights.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insights.Repository.Auth;

public interface IAuthRepository
{
    bool AuthorizeUser(AuthInfo info);
    string GetAccessToken();
    string GetRefreshToken();
}



