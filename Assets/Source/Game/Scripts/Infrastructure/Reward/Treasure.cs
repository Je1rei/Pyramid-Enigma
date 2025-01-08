using UnityEngine;

public class Treasure: MonoBehaviour
{
    [SerializeField] private int _value;

    public int Value => _value;
}
