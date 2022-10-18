using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TimerManager : MonoBehaviour
{
    static HashSet<Timer> timers;
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
        timers = new HashSet<Timer>();
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

    #region AddTimer
    //With timer return
    public TimerForUser AddGetTimer(Action onFinished, float timer, bool isCycled = false)
    {
        TimerForUser timerForUsers = new TimerForUser(onFinished, timer, isCycled);
        return timerForUsers;
    }

    public TimerForUser AddGetProgressiveTimer(Action onFinished, Action<float> onUpdate, float timer, bool isCycled = false)
    {
        TimerForUser timerForUsers = new TimerForUser(onFinished, onUpdate, timer, isCycled);
        return timerForUsers;
    }

    //Without timer return
    public void AddTimer(Action onFinished, float timer, bool isCycled = false) => timers.Add(new Timer(onFinished, timer, isCycled));

    public void AddProgressiveTimer(Action onFinished, Action<float> onUpdate, float timer, bool isCycled = false) => timers.Add(new ProgressiveTimer(onFinished, timer, isCycled, onUpdate));

    private void AddTimer(Timer timer) => timers.Add(timer);
    #endregion

    private void RemoveTimer(Timer timer) => timers.Remove(timer);

    #region TimerClass
    public class TimerForUser
    {
        private Timer timer;

        public TimerForUser(Action onFinished, float maxTimer, bool isCycled)
        {
            timer = new Timer(onFinished, maxTimer, isCycled);
            TimerManager.manager.AddTimer(timer);
        }

        public TimerForUser(Action onFinished, Action<float> onUpdate, float maxTimer, bool isCycled)
        {
            timer = new ProgressiveTimer(onFinished, maxTimer, isCycled, onUpdate);
            TimerManager.manager.AddTimer(timer);
        }

        /// <summary>
        /// Changes timer countdown
        /// If isGoing = false, then the timer stops
        /// If isGoing = true, then the timer resumes
        /// </summary>
        public void ChangeTimerCountdown(bool isGoing) => timer.CountdownMode(isGoing);

        public void ChangeTimerDuration(float duration) => timer.SetMaxDuration(duration);

        public void RemoveTimer() => TimerManager.manager.RemoveTimer(this.timer);

        public void ResetTimer() => timer.ResetTimer();
    }

    protected class Timer
    {
        protected float duration;
        protected float timer;
        protected bool isGoing;
        protected Action onFinished;

        public Timer(Action onFinished, float duration, bool isCycled)
        {
            this.onFinished = onFinished;
            if (!isCycled)
                this.onFinished += () => TimerManager.manager.RemoveTimer(this);
            else
                this.onFinished += () => ResetTimer();

            isGoing = true;
            timer = duration;
            this.duration = duration;
        }

        public virtual void Update()
        {
            if (!isGoing)
                return;

            timer -= Time.deltaTime;
            if (timer < 0)
            {
                if (onFinished != null)
                    onFinished.Invoke();
            }
        }

        public void SetMaxDuration(float duration) => this.duration = Mathf.Clamp(duration, 0, float.MaxValue);

        public void CountdownMode(bool isGoing) => this.isGoing = isGoing;

        public void ResetTimer() => timer = duration;
    }

    protected class ProgressiveTimer : Timer
    {
        protected Action<float> onUpdate;

        public ProgressiveTimer(Action onFinished, float duration, bool isCycled, Action<float> onUpdate) : base(onFinished, duration, isCycled)
        {
            this.onUpdate = onUpdate;
        }

        public override void Update()
        {
            base.Update();

            if (onUpdate != null)
                onUpdate.Invoke((float)(timer / duration));
        }
    } 
    #endregion
}

