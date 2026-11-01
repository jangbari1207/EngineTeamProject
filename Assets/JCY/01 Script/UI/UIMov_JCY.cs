using DG.Tweening;
using UnityEngine;

public class UIMov_JCY : MonoBehaviour
{
   [SerializeField] private RectTransform panel;
   [SerializeField] private bool _isDisplay;
   [SerializeField] private float duration;
   [SerializeField] private float xValue;
   [SerializeField] private float yValue;
   public void Move()
   {
      Debug.Log("무브");
      panel.DOKill();
      if (_isDisplay)
      {
         panel.DOAnchorPosX(-xValue, duration).SetEase(Ease.InOutCubic);
      }
      else
      { 
         panel.DOAnchorPosX(0, duration).SetEase(Ease.InOutCubic);;
      }

      _isDisplay = !_isDisplay;
   }
}
