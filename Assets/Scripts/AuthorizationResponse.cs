using Newtonsoft.Json;

public class AuthorizationResponse
{
    [JsonProperty(PropertyName = "accessToken")]
    public string AccessToken { get; set; }
    [JsonProperty(PropertyName = "refreshToken")]
    public string RefreshToken { get; set; }
}
