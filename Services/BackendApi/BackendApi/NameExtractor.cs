namespace BackendApi
{
    using System.Security.Claims;
    using System.Security.Principal;

    public static class NameExtractor
    {

        public static string? ExtractDisplayName(this ClaimsPrincipal claim)
        {
            var identity = claim.Identity;

            string? name = claim.Claims.Where(c => c.Type=="displayname").Select(c => c.Value).FirstOrDefault();

            if (!string.IsNullOrEmpty(name))
            {
                return name;
            }

            return identity?.Name;
        }
    }
}
