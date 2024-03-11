using DG.Tweening;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private GameObject _mainMenu;
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
        _mainMenu = GameObject.FindWithTag("MainMenu");
    }

    private void Update()
    {
        if (!_parabolaController.Animation && _isFirst)
        {
            _mainMenu.SetActive(true);
            PanelFadeIn();
            _isFirst = false;
        }
        
        else if (_isFirst)
        {
            _mainMenu.SetActive(false);
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
