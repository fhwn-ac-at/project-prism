namespace BackendApi
{
    using System.Security.Claims;
    using System.Security.Principal;

    public static class NameExtractor
    {

        public static string? ExtractDisplayName(this ClaimsPrincipal claim)
        {
            IIdentity? identity = claim.Identity;

            string? name = claim.Claims.Where(c => c.Type=="displayname").Select(c => c.Value).FirstOrDefault();

            return !string.IsNullOrEmpty(name) ? name : identity==null ? "No Name" : (identity?.Name);
        }
    }
}
