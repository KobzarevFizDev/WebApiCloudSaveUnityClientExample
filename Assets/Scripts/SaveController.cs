using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveController
{
    private const string ACCESS_TOKEN_KEY = "access";
    private const string REFRESH_TOKEN_KEY = "refresh";

    public static string AccessToken
    {
        get
        {
            return PlayerPrefs.GetString(ACCESS_TOKEN_KEY, "NullAccessToken");
        }
    }

    public static string RefreshToken
    {
        get
        {
            return PlayerPrefs.GetString(REFRESH_TOKEN_KEY, "NullRefreshToken");
        }
    }

    public static void SaveJwtTokens(AuthorizationResponse authorizationResponse)
    {
        PlayerPrefs.SetString(ACCESS_TOKEN_KEY, authorizationResponse.AccessToken);
        PlayerPrefs.SetString(REFRESH_TOKEN_KEY, authorizationResponse.RefreshToken);
    }

    public static void UpdateAccessToken(string newAccessToken)
    {
        PlayerPrefs.SetString(ACCESS_TOKEN_KEY, newAccessToken);
    }
}
