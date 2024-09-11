using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerSaveForm : MonoBehaviour
{
    [SerializeField] private StateMachine _sm;
    [SerializeField] private WebApi _webApi;
    [SerializeField] private Button _increaseMoneyButton;
    [SerializeField] private Button _decreaseMoneyButton;
    [SerializeField] private Button _increaseLevelButton;
    [SerializeField] private Button _decreaseLevelButton;
    [SerializeField] private Button _applyNickname;
    [SerializeField] private Button _backToForm;
    [SerializeField] private TextMeshProUGUI _moneyValue;
    [SerializeField] private TextMeshProUGUI _levelValue;
    [SerializeField] private TextMeshProUGUI _nicknameValue;
    [SerializeField] private TMP_InputField _nicknameField;

    private PlayerSave _playerSave;

    private async void OnEnable()
    {
        _nicknameField.text = "Default";
        ShowCurrentPlayerSave();
    }

    private void Start()
    {
        _increaseMoneyButton.onClick.AddListener(OnIncreaseMoneyClick);
        _decreaseMoneyButton.onClick.AddListener(OnDecreaseMoneyClick);
        _applyNickname.onClick.AddListener(OnApplyNickname);
        _backToForm.onClick.AddListener(OnBackToForm);
    }

    private async void ShowCurrentPlayerSave()
    {
        string login = AppContext.Login;
        _playerSave = await _webApi.GetSave(login);
        if (_webApi.ResponceCode == ResponceCode.NotAuthorization) 
        {
            string newAccessToken = await _webApi.UpdateAccessToken(login);
            print("The access token has been updated.");
            SaveController.UpdateAccessToken(newAccessToken);
            _playerSave = await _webApi.GetSave(login);
        }

        _moneyValue.text = $"Money: {_playerSave.Money}";
        _levelValue.text = $"Level: {_playerSave.Level}";
        _nicknameValue.text = $"Nickname: {_playerSave.Nickname}";
    }

    private async void OnIncreaseMoneyClick()
    {
        string login = AppContext.Login;
        int newMoneyAmount = _playerSave.Money + 50;
        await _webApi.SetMoney(login, newMoneyAmount);
        ShowCurrentPlayerSave();
    }

    private async void OnDecreaseMoneyClick()
    {
        string login = AppContext.Login;
        int newMoneyAmount = _playerSave.Money - 50;
        await _webApi.SetMoney(login, newMoneyAmount);
        ShowCurrentPlayerSave();
    }

    private void OnIncreaseLevelClick()
    {

    }

    private void OnDecreaseLevelClick()
    {

    }

    private async void OnApplyNickname()
    {
        string login = AppContext.Login;
        string newNickname = _nicknameField.text;
        await _webApi.SetNickname(login, newNickname);
        ShowCurrentPlayerSave();
    }

    private void OnBackToForm()
    {
        _sm.SetAuthorizationFormState();
    }

    private void OnDestroy()
    {
        _increaseMoneyButton.onClick.RemoveListener(OnIncreaseMoneyClick);
        _decreaseMoneyButton.onClick.RemoveListener(OnDecreaseMoneyClick);
        _applyNickname.onClick.RemoveListener(OnApplyNickname);
        _backToForm.onClick.RemoveListener(OnBackToForm);
    }

}
