using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    [SerializeField] private RectTransform talkPanel;
    [SerializeField] private float duration;
    [SerializeField] private TextMeshProUGUI titleTxt;
    [SerializeField] private TextMeshProUGUI descTxt;
    public bool _isDisplay;

    private void Start()
    {
        talkPanel.localScale = new Vector3(0, 0, 0);
    }

    public void TalkOn()
    {
        TalkStart();
    }

    public void TalkStart(string title = null , string desc = null)
    {
        talkPanel.DOKill();
        titleTxt.text = title != null ? title : "오브젝트 이름 등";
        descTxt.text = desc != null ? desc : "그에따른 설명";

        if (_isDisplay)
        {
            talkPanel.DOScale(0, duration).SetEase(Ease.OutQuart);
        }
        else
        { 
            talkPanel.DOScale(1, duration).SetEase(Ease.OutQuart);
        }

        _isDisplay = !_isDisplay;
    }
}
