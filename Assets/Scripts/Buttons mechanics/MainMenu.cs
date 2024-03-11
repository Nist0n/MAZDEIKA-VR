using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;
    private ParabolaController _parabolaController;
    
    private float _fadeTime = 2f;
    private bool _isFirst = true;
    
    private void Start()
    {
        _parabolaController = FindObjectOfType<ParabolaController>();
        _canvasGroup = GameObject.FindWithTag("MainMenu").GetComponent<CanvasGroup>();
        _rectTransform = GameObject.FindWithTag("MainMenu").GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (!_parabolaController.Animation && _isFirst)
        {
            PanelFadeIn();
            _isFirst = false;
        }
    }

    private void PanelFadeIn()
    {
        _canvasGroup.alpha = 0f;
        _rectTransform.transform.localPosition = new Vector3(0f, -1000f, 0f);
        _rectTransform.DOAnchorPos(new Vector2(0f, 0f), _fadeTime, false).SetEase(Ease.OutElastic);
        _canvasGroup.DOFade(1, _fadeTime);
    }
    
}
