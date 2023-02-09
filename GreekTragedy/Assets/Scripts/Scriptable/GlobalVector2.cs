using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Global Variables/New Vector 2")]
public sealed class GlobalVector2 : ScriptableObject
{
    public event Action<Vector2> OnValueChanged;
    public bool broadcast;
    public bool resetOnDisable;
    public Vector2 resetValue;
    Vector2 _value;

    public Vector2 Value
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
