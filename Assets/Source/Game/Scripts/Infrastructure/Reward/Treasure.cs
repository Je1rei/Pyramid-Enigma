using UnityEngine;

[RequireComponent(typeof(TreasureMover))]
public class Treasure: MonoBehaviour
{
    [SerializeField] private int _value = 1;

    private TreasureMover _mover;

    public int Value => _value;

    public void Init()
    {
        _mover = GetComponent<TreasureMover>();
        _mover.Move();
    }
}
