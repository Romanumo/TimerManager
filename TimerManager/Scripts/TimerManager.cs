using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TimerManager : MonoBehaviour
{
    static List<Timer> timers;
    static TimerManager Tmanager;

    public static TimerManager manager 
    { 
        get 
        {
            return (Tmanager == null) ? (Tmanager = (new GameObject("TimerManager")).AddComponent<TimerManager>()) : Tmanager;
        }
    }

    public void Awake()
    {
        timers = new List<Timer>();
    }

    public void Update()
    {
        if (timers.Count > 0)
        {
            foreach(Timer timer in timers)
            {
                if (timer != null)
                    timer.Update();
            }
        }
    }

    public void AddTimer(Action onFinished, float timer)
    {
        timers.Add(new Timer(onFinished, timer));
    }

    public void AddProgressiveTimer(Action onFinished, Action<float> onUpdate, float timer)
    {
        timers.Add(new ProgressiveTimer(onFinished, timer, onUpdate));
    }

    void RemoveTimer(Timer timer)
    {
        timers.Remove(timer);
    }

    #region TimerClass
    protected class Timer
    {
        protected Action onFinished;
        protected float timer;

        public Timer(Action onFinished, float maxTimer)
        {
            this.onFinished = onFinished;
            timer = maxTimer;
        }

        public virtual void Update()
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                if (onFinished != null)
                {
                    onFinished.Invoke();
                    TimerManager.manager.RemoveTimer(this);
                }
            }
        }
    }

    protected class ProgressiveTimer : Timer
    {
        Action<float> onUpdate;
        float maxTimer;

        public ProgressiveTimer(Action onFinished, float maxTimer, Action<float> onUpdate) : base(onFinished, maxTimer)
        {
            this.onUpdate = onUpdate;
            this.maxTimer = maxTimer;
        }

        public override void Update()
        {
            base.Update();

            if (onUpdate != null)
                onUpdate.Invoke((float)(timer / maxTimer));
        }
    } 
    #endregion
}

