using System.Text.RegularExpressions;

namespace VoyagerTravelBlog.Application.Helpers
{
    public static partial class RegexHelper
    {
        [GeneratedRegex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&#])[A-Za-z\\d@$!%*?&#]{8,}$")]
        public static partial Regex ValidPassword();
    }
}
