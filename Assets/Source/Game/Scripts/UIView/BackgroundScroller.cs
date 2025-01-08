using System;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] private float _scrollSpeed; // Скорость движения
    [SerializeField] private float _moveAmount; // Максимальное расстояние движения влево и вправо

    private RectTransform _rectTransform;

    private void Update()
    {
        _rectTransform = GetComponent<RectTransform>();
        ScrollImage();
    }

    private void ScrollImage()
    {
        float newX = Mathf.Sin(Time.time * _scrollSpeed) * _moveAmount;
        _rectTransform.anchoredPosition = new Vector2(newX, _rectTransform.anchoredPosition.y);
    }
}