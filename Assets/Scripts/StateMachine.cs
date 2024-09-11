using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class StateMachine : MonoBehaviour
{
    [SerializeField] private AuthorizationForm _authorizationForm;
    [SerializeField] private NicknameForm _nicknameForm;
    [SerializeField] private PlayerSaveForm _playerSaveForm;

    private void Start()
    {
        _authorizationForm.OnSignInEvent += SetAuthorizedState;
        _authorizationForm.OnSignUpEvent += SetChoosingNicknameState;

        SetAuthorizationFormState();
    }

    public void SetAuthorizationFormState()
    {
        _authorizationForm.gameObject.SetActive(true);
        _nicknameForm.gameObject.SetActive(false);
        _playerSaveForm.gameObject.SetActive(false);
    }

    public void SetChoosingNicknameState()
    {
        _authorizationForm.gameObject.SetActive(false);
        _nicknameForm.gameObject.SetActive(true);
        _playerSaveForm.gameObject.SetActive(false);
    }

    public void SetAuthorizedState()
    {
        _authorizationForm.gameObject.SetActive(false);
        _nicknameForm.gameObject.SetActive(false);
        _playerSaveForm.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        _authorizationForm.OnSignInEvent -= SetAuthorizedState;
        _authorizationForm.OnSignUpEvent -= SetChoosingNicknameState;
    }
}
