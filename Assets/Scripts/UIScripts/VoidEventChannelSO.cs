using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Void Event")]
public class VoidEventChannelSO : ScriptableObject
{
    public event Action OnVoidEvent;

    public void RaiseEvent()
    {
        OnVoidEvent?.Invoke();
    }
}
