namespace SnusPunch.Shared.Models.Auth
{
    public class RoleClaimModel
    {
        public string? Issuer { get; set; }
        public string? OriginalIssuer { get; set; }
        public string? Type { get; set; }
        public string? Value { get; set; }
        public string? ValueType { get; set; }
    }
}
