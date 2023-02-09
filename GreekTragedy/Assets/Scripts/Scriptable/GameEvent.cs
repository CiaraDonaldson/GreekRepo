using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewGameEvent", menuName = "New Game Event")]
public sealed class GameEvent : ScriptableObject
{
    public event Action<GameObject> OnEventRaised;
    readonly List<GameEventListener> listeners = new();
    public void Invoke(GameObject obj) { for (int i = listeners.Count - 1; i >= 0; i--) listeners[i].OnEventRaised(obj); OnEventRaised?.Invoke(obj); }
    public void RegisterListener(GameEventListener listener) => listeners.Add(listener);
    public void UnregisterListener(GameEventListener listener) => listeners.Remove(listener);
}
