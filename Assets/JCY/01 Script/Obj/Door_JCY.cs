using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class Door_JCY : MonoBehaviour , IObject_JCY
{
    [SerializeField] private Door_JCY _endDoor;
    [SerializeField] private CinemachineConfiner2D _camera;
    public BoxCollider2D _collider2D;
    public Transform _doorTfm;
    public static float _time;
    public bool _isKey = false;
    [SerializeField] private QuestStepData questStepOnUse;
    [SerializeField] private TalkTextSO talkTextOnUse;
    [SerializeField] private QuestStepData minimumQuestForTalk;
    private bool hasPlayedUseDialogue;

    public void SetLocked(bool locked)
    {
        _isKey = locked;
    }

    public void Interaction(PlayerInterecttion_JCY player, FadePanel_JCY fadePanelJcy = null)
    {
        if (Time.time < _time)
        {
            return;
        }
        
        if (_isKey)
        {
            Debug.Log("열쇠필요");   
            return;
        }
        
        if (_endDoor._collider2D != null && _camera != null)
        {
            _camera.BoundingShape2D = _endDoor._collider2D;
        }

        if (fadePanelJcy != null)
        {
            fadePanelJcy.Fade();
        }

        
        player.transform.position = _endDoor._doorTfm.position;
        if (questStepOnUse != null && QuestManager.instance != null)
            QuestManager.instance.ReportSignal(questStepOnUse);
        TryShowUseDialogue();
        //문 열리는 사운드 출력
        //이동
       
        
        if (GameManager.Instance._enemyManager._isChase)
        {
            _time = Time.time + 3f;
            GameManager.Instance._enemyManager.PlayerMove(_endDoor._doorTfm);
        }
        else
        {
             _time = Time.time + 0.5f;
        }
    }

    private void TryShowUseDialogue()
    {
        if (hasPlayedUseDialogue || talkTextOnUse == null || minimumQuestForTalk == null ||
            QuestManager.instance == null || QuestManager.instance.steps == null)
            return;

        int minimumQuestIndex = QuestManager.instance.steps.IndexOf(minimumQuestForTalk);
        if (minimumQuestIndex < 0 || QuestManager.instance.currentStep < minimumQuestIndex)
            return;

        TalkManager talkManager = TalkManager.Instance;
        if (talkManager == null)
            return;

        hasPlayedUseDialogue = true;
        talkManager.ShowTalk(talkTextOnUse);
    }
}
