using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] private TextMeshPro _text;
    [SerializeField] private float _upSpeed;
    [SerializeField] private float _popScale;
    [SerializeField] private float _lifeTime;
    private float _lastSetTime;
    private float _defaultSize;

    private Sequence _scaleSeq;

    public void Init(Vector3 pos, string text, Color color)
    {
        transform.position = pos;
        _text.text = text;
        _text.color = color;
        _lastSetTime = Time.time;
        _defaultSize = _text.fontSize;

        if (_scaleSeq != null && _scaleSeq.IsActive()) _scaleSeq.Kill();
        _scaleSeq = DOTween.Sequence();
        _scaleSeq.Append(DOTween.To(() => _defaultSize, value => _text.fontSize = value, _defaultSize * _popScale, 0.05f).SetEase(Ease.OutExpo))
            .Append(DOTween.To(() => _defaultSize * _popScale, value => _text.fontSize = value, _defaultSize, 0.2f).SetEase(Ease.InQuad));
    }

    private void Update()
    {
        float lifeAmount = Time.time - _lastSetTime / _lifeTime;
        if (lifeAmount < 1)
        {
            transform.position += Vector3.up * _upSpeed * Time.deltaTime;
            _text.alpha = 1 - Mathf.Pow(lifeAmount, 10);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
