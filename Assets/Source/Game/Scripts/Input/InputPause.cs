using UnityEngine;
using System.Collections.Generic;

public class InputPause : MonoBehaviour
{
    [SerializeField] private GridRotator _rotator;
    [SerializeField] private Grid _grid;
    
    private List<BlockMover> _blockMovers = new List<BlockMover>();

    private readonly float _delay = 0.02f;

    private float _currentDelay = 0;
    private bool _canMove = true;

    public bool CanMove => _canMove;

    private void OnEnable()
    {
        _grid.BlockCreated += AddBlock;
    }

    private void Update()
    {
        if (_currentDelay > 0)
        {
            _currentDelay -= Time.deltaTime;
        }

        if (_canMove != TryCanInput())
        {
            if (_canMove)
            {
                _canMove = false;
            }
            else
            {
                _canMove = true;
                Detain();
            }
        }
    }

    private void OnDisable()
    {
        _grid.BlockCreated -= AddBlock;
    }

    public bool CanInput() => _canMove && _currentDelay <= 0;

    private void AddBlock(BlockMover mover)
    {
        _blockMovers.Add(mover);
    }

    private void Detain()
    {
        _currentDelay = _delay;
    }

    private bool TryCanInput()
    {
        foreach (BlockMover blockMover in _blockMovers)
        {
            if (blockMover.IsMoving)
                return false;
        }

        if (_rotator.IsRotating)
            return false;

        return true;
    }
}
