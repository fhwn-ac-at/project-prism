namespace BackendApi
{
    using System.Security.Claims;
    using System.Security.Principal;

    public static class ClaimExtractor
    {

        public static string? ExtractDisplayName(this ClaimsPrincipal claim)
        {
            IIdentity? identity = claim.Identity;

            string? name = claim.Claims.Where(c => c.Type=="displayname").Select(c => c.Value).FirstOrDefault();

            return !string.IsNullOrEmpty(name) ? name : identity==null ? "No Name" : (identity?.Name);
        }

        public static string? ExtractIdentifier(this ClaimsPrincipal claim)
        {
            return claim.Claims.Where(c => c.Type=="http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Select(c => c.Value).FirstOrDefault();
        }
    }
}
