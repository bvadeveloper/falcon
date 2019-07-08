using System.Linq;

namespace Falcon.Profiles
{
    public static class ProfileExtensions
    {
        public static bool HasTools(this IToolProfile profile)
        {
            return profile.Tools != null && profile.Tools.Any();
        }
    }
}