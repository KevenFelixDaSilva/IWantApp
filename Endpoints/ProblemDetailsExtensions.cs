using Flunt.Notifications;
using Microsoft.AspNetCore.Identity;

namespace IWantApp.Endpoints;

public static class ProblemDetailsExtensions
{
    public static Dictionary<string, string[]> ConvertProblemDetails(this IReadOnlyCollection<Notification> notifications)
    {
       return notifications
            .GroupBy(g => g.Key)
            .ToDictionary(g => g.Key, g => g.Select(x => x.Message).ToArray());
    }
    public static Dictionary<string, string[]> ConvertProblemDetails(this IEnumerable<IdentityError> error)
    {
        var dictionary = new Dictionary<string, string[]>();
        dictionary.Add("Error", error.Select(g => g.Description).ToArray());
        return error
             .GroupBy(g => g.Code)
             .ToDictionary(g => g.Key, g => g.Select(x => x.Description).ToArray());
    }
}
