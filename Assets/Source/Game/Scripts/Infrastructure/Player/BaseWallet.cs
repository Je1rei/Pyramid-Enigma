using System;

namespace Source.Game.Scripts
{
    public abstract class BaseWallet : IService
    {
        private readonly int _decreaseValue = 1;
        private int _value;
        
        public event Action<int> Changed;
        
        public int Value => _value;

        public void Init(int value)
        {
            _value = value;
            Changed?.Invoke(_value);
        }
        
        public virtual void Increase(int amount)
        {
            _value += amount;
            Changed?.Invoke(_value);
        }        
        
        public virtual void Decrease()
        {
            if (_value > 0)
            {
                _value -= _decreaseValue;
                Changed?.Invoke(_value);
            }
        }
    }
}