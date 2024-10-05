using System.Collections;
using System.IO.Pipes;
using UnityEngine;

public class GridRotator : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _duration = 1f;
    [SerializeField] private int _endAngleRotation = 90;

    private Coroutine _coroutine;

    private void Awake()
    {
        transform.rotation = Quaternion.identity;   
    }

    public void SetupRotate(DirectionType direction)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine); 

        _coroutine = StartCoroutine(Rotate(direction));
    }

    private IEnumerator Rotate(DirectionType direction) 
    {
        Vector3Int rotateDirection = direction.ToVector3Int() * _endAngleRotation;

        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(rotateDirection);

        float elapsed = 0f;

        while (elapsed < _duration)
        {
            elapsed += Time.deltaTime;
            float timer = elapsed / _duration;

            transform.localRotation = Quaternion.Slerp(startRotation, endRotation, timer);

            yield return null;
        }

        transform.rotation = endRotation;
    }
}