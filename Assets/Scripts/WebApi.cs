using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Cysharp.Threading.Tasks;
using System;

public enum ResponceCode 
{ 
    Ok, 
    NicknameAlredyUsed, 
    NotAuthorization,
    IncorrectLoginOrPassword 
}

public class WebApi : MonoBehaviour
{
    private const string WebApiUrl = "https://localhost:7029/api/";

    public Action<ResponceCode> Error;
    public ResponceCode ResponceCode { private set; get; }

    public async UniTask<PlayerSave> GetSave(string login)
    {
        string url = $"{WebApiUrl}saveStorage/getSaveOfPlayer/{login}";
        using(UnityWebRequest req = UnityWebRequest.Get(url))
        {
            try
            {
                string authorizationHeaderContent = $"Bearer {SaveController.AccessToken}";
                req.SetRequestHeader("Authorization", authorizationHeaderContent);
                await req.SendWebRequest();
            }
            catch(UnityWebRequestException ex)
            {
                Error?.Invoke(GetResponceCode(req));
            }
            finally
            {
                ResponceCode = GetResponceCode(req);
            }

            if (ResponceCode == ResponceCode.Ok)
            {
                string json = req.downloadHandler.text;
                ResponceCode = GetResponceCode(req);
                var playerSave = JsonConvert.DeserializeObject<PlayerSave>(json);
                return playerSave;
            }
            else
            {
                return new NullPlayerSave();
            }
        }
    }

    public async UniTask<AuthorizationResponse> SignUp(string login, string password)
    {
        string url = $"{WebApiUrl}account/signUp";

        WWWForm form = new WWWForm();
        form.AddField("login", login);
        form.AddField("password", password);

        using(UnityWebRequest req = UnityWebRequest.Post(url, form))
        {
            try
            {
                await req.SendWebRequest();
            }
            catch(UnityWebRequestException ex)
            {
                Error?.Invoke(GetResponceCode(req));
            }
            finally
            {
                ResponceCode = GetResponceCode(req);
            }

            if (ResponceCode == ResponceCode.Ok)
            {
                string json = req.downloadHandler.text;
                Debug.Log("WebApi::SignUp(). Result: " + json);
                var responce = JsonConvert.DeserializeObject<AuthorizationResponse>(json);
                return responce;
            }
            else
            {
                return new NullAuthorizationResponse();
            }
        }
    }

    public async UniTask SignIn(string login, string password)
    {
        string url = $"{WebApiUrl}account/signIn";
        WWWForm form = new WWWForm();
        form.AddField("login", login);
        form.AddField("password", password);

        using (UnityWebRequest req = UnityWebRequest.Post(url, form))
        {
            try
            {
                await req.SendWebRequest();
            }
            catch (UnityWebRequestException ex)
            {
                Error?.Invoke(GetResponceCode(req));
            }
            finally
            {
                ResponceCode = GetResponceCode(req);
            }
        }
    }

    public async UniTask SetNickname(string login, string newNickname)
    {
        string url = $"{WebApiUrl}saveStorage/updateNickname/{login}/{newNickname}";
        using (UnityWebRequest req = UnityWebRequest.Put(url, ""))
        {
            await req.SendWebRequest();
            ResponceCode = GetResponceCode(req);
        }
    }

    public async UniTask SetMoney(string login, int money)
    {
        string url = $"{WebApiUrl}saveStorage/updateAmountOfMoney/{login}/{money}";
        using (UnityWebRequest req = UnityWebRequest.Put(url, ""))
        {
            await req.SendWebRequest();
            ResponceCode = GetResponceCode(req);
        }
    }

    public async UniTask<string> UpdateAccessToken(string login)
    {
        string expiredAccessToken = SaveController.AccessToken;
        string refreshToken = SaveController.RefreshToken;
        string url = $"{WebApiUrl}token/updateAccessToken";

        WWWForm form = new WWWForm();
        form.AddField("expiredAccessToken", expiredAccessToken);
        form.AddField("refreshToken", refreshToken);

        using (UnityWebRequest req = UnityWebRequest.Post(url, form))
        {
            try
            {
                await req.SendWebRequest();
            }
            catch (UnityWebRequestException ex)
            {
                Error?.Invoke(GetResponceCode(req));
            }
            finally
            {
                ResponceCode = GetResponceCode(req);
            }

            if (ResponceCode == ResponceCode.Ok)
            {
                string newAccessToken = req.downloadHandler.text;
                return newAccessToken;
            }
            else
            {
                return "failed to refresh token";
            }
        }

    }

    private ResponceCode GetResponceCode(UnityWebRequest req)
    {
        switch (req.responseCode) 
        { 
            case 409:
                return ResponceCode.NicknameAlredyUsed;

            case 401:
                return ResponceCode.NotAuthorization;

            case 403:
                return ResponceCode.IncorrectLoginOrPassword;

            case 200:
                return ResponceCode.Ok;


            default:
                throw new System.InvalidOperationException($"Unknow code = {req.responseCode}");
        }
    }
}
