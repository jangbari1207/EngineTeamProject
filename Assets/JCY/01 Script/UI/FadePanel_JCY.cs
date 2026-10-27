using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadePanel_JCY : MonoBehaviour
{
    private Image _image;
    [SerializeField] private float duration;
    private Sequence _fadeSequence;

    private void Awake()
    {
        _image = GetComponent<Image>();
        gameObject.SetActive(false);
    }
    
    public void Fade()
    {
        _fadeSequence?.Kill();
        _image.gameObject.SetActive(true);
        gameObject.SetActive(true);
        _image.color = Color.black;
        Debug.Log( _image.color.a);
        
         _fadeSequence = DOTween.Sequence()
         .Append(_image.DOFade(0, duration))
         .OnComplete(() => gameObject.SetActive(false));
    }
}
