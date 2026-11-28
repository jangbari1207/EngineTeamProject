using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TalkManager : MonoSingleton<TalkManager>
{
    [SerializeField] private RectTransform talkPanel;
    [SerializeField] private float duration;
    [SerializeField] private TextMeshProUGUI titleTxt;
    [SerializeField] private TextMeshProUGUI descTxt;
    [SerializeField] private Button speedButton;
    [SerializeField] private TMP_Text speedButtonLabel;
    public bool _isDisplay;

    private bool _isFastMode;
    private bool _isDialogue;
    private bool _isTyping;
    private bool _skipTypingRequested;
    private bool _advanceRequested;
    private bool _closeOnAdvance;

    private float CurrentSpeedMultiplier => _isFastMode ? 1.5f : 1f;

    protected override void Awake()
    {
        base.Awake();
        if (speedButton == null && talkPanel != null)
            speedButton = talkPanel.GetComponentInChildren<Button>(true);

        if (speedButton != null)
        {
            speedButton.onClick.AddListener(ToggleFastMode);
            if (speedButtonLabel == null)
                speedButtonLabel = speedButton.GetComponentInChildren<TMP_Text>(true);
        }

        RefreshSpeedButtonLabel();
    }

    protected override void OnDestroy()
    {
        if (speedButton != null)
            speedButton.onClick.RemoveListener(ToggleFastMode);
        base.OnDestroy();
    }

    private void Update()
    {
        if (!_isDisplay || !_isDialogue || Keyboard.current == null ||
            !Keyboard.current.spaceKey.wasPressedThisFrame)
            return;

        if (_isTyping)
            _skipTypingRequested = true;
        else if (_closeOnAdvance)
            CloseTalk();
        else
            _advanceRequested = true;
    }

    public void ToggleFastMode()
    {
        _isFastMode = !_isFastMode;
        RefreshSpeedButtonLabel();
    }

    private void RefreshSpeedButtonLabel()
    {
        if (speedButtonLabel != null)
            speedButtonLabel.text = _isFastMode ? "1.5x" : "1x";
    }

    private void Start()
    {
        talkPanel.localScale = new Vector3(0, 0, 0);
    }

    public void TalkOn()
    {
        TalkStart();
    }

    public void ShowObjectInfo(string title, string description)
    {
        _isDialogue = false;
        talkPanel.DOKill();
        titleTxt.text = title;
        descTxt.text = description;
        descTxt.maxVisibleCharacters = int.MaxValue;
        _isDisplay = true;
        talkPanel.DOScale(1, duration).SetEase(Ease.OutQuart);
    }

    public void ShowTalk(string speakerName, string dialogue)
    {
        StartCoroutine(ShowTalkRoutine(speakerName, dialogue));
    }

    public void ShowTalk(TalkTextSO dialogue)
    {
        if (dialogue != null)
            StartCoroutine(ShowTalkRoutine(dialogue));
    }

    public IEnumerator ShowTalkRoutine(TalkTextSO dialogue, bool isLastLine = true)
    {
        if (dialogue == null)
            yield break;

        yield return ShowTalkRoutine(dialogue.speakerName, dialogue.dialogueText, isLastLine);
    }

    public IEnumerator ShowTalkRoutine(string speakerName, string dialogue, bool isLastLine = true)
    {
        _isDialogue = true;
        _isTyping = true;
        _closeOnAdvance = isLastLine;
        _skipTypingRequested = false;
        _advanceRequested = false;
        talkPanel.DOKill();
        titleTxt.text = speakerName ?? string.Empty;
        descTxt.text = string.Empty;
        descTxt.maxVisibleCharacters = int.MaxValue;
        if (!_isDisplay)
            talkPanel.localScale = new Vector3(1,1, 1);

        _isDisplay = true;

        yield return TextTypewriter.Instance.TypeText(
            descTxt,
            dialogue,
            0.06f,
            () => CurrentSpeedMultiplier,
            () => _skipTypingRequested);
        _isTyping = false;
        _skipTypingRequested = false;
    }

    public IEnumerator WaitForNextLine(float normalDelay)
    {
        float elapsed = 0f;
        float delay = Mathf.Max(0f, normalDelay);
        while (elapsed < delay && !_advanceRequested && _isDisplay)
        {
            elapsed += Time.deltaTime * CurrentSpeedMultiplier;
            yield return null;
        }

        _advanceRequested = false;
    }

    public void CloseTalk()
    {
        _isDialogue = false;
        _isTyping = false;
        _closeOnAdvance = false;
        _skipTypingRequested = false;
        _advanceRequested = false;
        talkPanel.DOKill();
        talkPanel.localScale = new Vector3(0, 0, 0);
        _isDisplay = false;
    }

    public void TalkStart(string title = null , string desc = null)
    {
        talkPanel.DOKill();
        titleTxt.text = title != null ? title : "오브젝트 이름 등";
        descTxt.text = desc != null ? desc : "그에따른 설명";
        descTxt.maxVisibleCharacters = int.MaxValue;

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
