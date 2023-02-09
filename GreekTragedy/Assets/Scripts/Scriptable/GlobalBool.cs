using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Global Variables/New Bool")]
public sealed class GlobalBool : ScriptableObject
{
    public event Action<bool> OnValueChanged;
    public bool broadcast;
    public bool resetOnDisable;
    public bool resetValue;
    bool _value;

    public bool Value
    {
        get => _value;
        set
        {
            _value = value;
            if (broadcast)
                OnValueChanged?.Invoke(_value);
        }
    }

    private void OnDisable()
    {
        if (resetOnDisable)
            _value = resetValue;
    }
}
