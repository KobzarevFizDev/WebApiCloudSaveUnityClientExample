public class NullAuthorizationResponse : AuthorizationResponse
{
    public NullAuthorizationResponse()
    {
        AccessToken = "NullAccessToken";
        RefreshToken = "NullRefreshToken";
    }
}
