using UnityEngine;

public class Treasure: MonoBehaviour
{
    [SerializeField] private int _value = 1;

    public int Value => _value;
}
