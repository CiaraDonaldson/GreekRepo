using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Global Variables/New Bool")]
public sealed class GlobalBool : ScriptableObject
{
    public event Action<bool> OnValueChanged;
    public bool broadcast;
    public bool resetOnDisable;
    public bool resetImmediately;
    public bool resetValue;
    [SerializeField] bool _currentValue;

    public bool Value
    {
        get => _currentValue;
        set
        {
            _currentValue = value;
            if (broadcast)
                OnValueChanged?.Invoke(_currentValue);
            if (resetImmediately)
                _currentValue = resetValue;
        }
    }

    private void OnDisable()
    {
        if (resetOnDisable)
            _currentValue = resetValue;
    }
}
