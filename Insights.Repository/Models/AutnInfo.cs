using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insights.Repository.Models;
/// <summary>
/// Represents the authentication information.
/// </summary>
public class AuthInfo
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string UserAgent { get; set; }
}

