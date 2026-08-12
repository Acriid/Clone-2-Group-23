using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TimeManager", menuName = "Time/TimeManager")]
public class TimeManager : ScriptableObject
{
    public event Action<float> OnPlayerTimeChange;
    public event Action<float> OnEnemyTimeChange;
    public event Action<float> OnBulletTimeChange;
    
    public void ChangeGameTime(float newTime)
    {
        OnPlayerTimeChange?.Invoke(newTime);
        OnEnemyTimeChange?.Invoke(newTime);
        OnBulletTimeChange?.Invoke(newTime);
    }

    public void ChangeGameTime(float playerTime, float otherTime)
    {
        OnPlayerTimeChange?.Invoke(playerTime);
        OnEnemyTimeChange?.Invoke(otherTime);
        OnBulletTimeChange?.Invoke(otherTime);        
    }
}
