using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Text;
using System;
using Cysharp.Threading.Tasks;

public class AuthorizationForm : MonoBehaviour
{
    [SerializeField] private WebApi _webApi;
    [SerializeField] private TextMeshProUGUI _errorMessage;
    [SerializeField] private TMP_InputField _loginInputField;
    [SerializeField] private TMP_InputField _passwordInputField;
    [SerializeField] private Button _signUpButton;
    [SerializeField] private Button _signInButton;

    public Action OnSignInEvent;
    public Action OnSignUpEvent;

    private void Start()
    {
        _signUpButton.onClick.AddListener(OnClickSignUp);
        _signInButton.onClick.AddListener(OnClickSignIn);

        _webApi.Error += OnError;
    }

    private void OnError(ResponceCode code)
    {
        StartCoroutine(HandleErrorRoutine(code));
    }

    private IEnumerator HandleErrorRoutine(ResponceCode code)
    {
        if (code == ResponceCode.NicknameAlredyUsed)
        {
            _errorMessage.text = "NicknameAlredyUsed";
        }
        else if (code == ResponceCode.IncorrectLoginOrPassword)
        {
            _errorMessage.text = "IncorrectLoginOrPassword";
        }

        _signUpButton.gameObject.SetActive(false);
        _signInButton.gameObject.SetActive(false);
        _errorMessage.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        _errorMessage.gameObject.SetActive(false);
        _signUpButton.gameObject.SetActive(true);
        _signInButton.gameObject.SetActive(true);
    }

    private async void OnClickSignUp()
    {
        string login = _loginInputField.text;
        string password = _passwordInputField.text;

        AuthorizationResponse response = await _webApi.SignUp(login, password);

        AppContext.Login = login;
        AppContext.Password = password;

        if (_webApi.ResponceCode == ResponceCode.Ok)
        {
            SaveController.SaveJwtTokens(response);
            OnSignUpEvent?.Invoke();     
        }
    }

    private async void OnClickSignIn()
    {
        string login = _loginInputField.text;
        string password = _passwordInputField.text;

        AppContext.Login = login;
        AppContext.Password = password;

        await _webApi.SignIn(login, password);
        if (_webApi.ResponceCode == ResponceCode.Ok)
            OnSignInEvent?.Invoke();
    }

    private void OnDestroy()
    {
        _signUpButton.onClick.RemoveListener(OnClickSignUp);
        _signInButton.onClick.RemoveListener(OnClickSignIn);
        _webApi.Error -= OnError;
    }


}
