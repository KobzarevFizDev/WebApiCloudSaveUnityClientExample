using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NicknameForm : MonoBehaviour
{
    [SerializeField] private StateMachine _sm;
    [SerializeField] private WebApi _webApi;
    [SerializeField] private TMP_InputField _nicknameField;
    [SerializeField] private Button _okButton;

    private void Start()
    {
        _okButton.onClick.AddListener(OnClickOk);
        _webApi.Error += OnError;
    }

    private void OnError(ResponceCode code)
    {
        Debug.LogError($"NicknameForm::OnError(). Code = {code}");
    }

    private async void OnClickOk()
    {
        string login = AppContext.Login;
        string newNickname = _nicknameField.text;
        await _webApi.SetNickname(login, newNickname);
        _sm.SetAuthorizedState();
    }

    private void OnDestroy()
    {
        _okButton.onClick.RemoveListener(OnClickOk);
        _webApi.Error -= OnError;
    }
}
