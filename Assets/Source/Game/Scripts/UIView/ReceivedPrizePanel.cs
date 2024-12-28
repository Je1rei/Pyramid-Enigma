﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class ReceivedPrizePanel : UIPanel
{
    private const string RewardID = "1";
    
    [SerializeField] private RewardPanel _rewardPanel;
    [SerializeField] private Button _backButton;
    [SerializeField] private TMP_Text _prizeText;
    
    private int _rewardValue = 3;

    private void OnEnable()
    {
        SetAudioService();
        AddButtonListener(_backButton, OnClickBackToGameplay);
    }

    public void RewardAd()
    {
        YG2.RewardedAdvShow(RewardID, () =>
        {
            ServiceLocator.Current.Get<BombWallet>().IncreaseScore(_rewardValue);
        });

        ServiceLocator.Current.Get<InputPause>().ActivateInputCooldown();
        Show();

        _prizeText.text += _rewardValue;
    }

    private void OnClickBackToGameplay()
    {
        Hide();
        _rewardPanel.Show();
    }
}