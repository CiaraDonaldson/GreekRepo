using System;
using UnityEngine;
using UnityEngine.UI;

public sealed class ImageAnimation : MonoBehaviour
{
    public static event Action<GameObject> AnimationTickerCreated;
    [SerializeField] float spriteChangeRate;
    float _currentRate;
    public Sprite[] sprites;

    private int index = 0;
    public Image image;

    void Start() => AnimationTickerCreated?.Invoke(gameObject);

    public void FixedUpdate()
    {
        _currentRate = _currentRate < 0 ? 0 : _currentRate -= Time.deltaTime;
        if (_currentRate.Equals(0))
        {
            _currentRate = spriteChangeRate;
            image.sprite = sprites[index];
            index++;
            if (index > sprites.Length - 1)
                index = 0;
        }
    }
}
