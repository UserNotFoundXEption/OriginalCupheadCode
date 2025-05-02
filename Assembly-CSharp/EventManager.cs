using System;
using System.Collections.Generic;

// Token: 0x02000058 RID: 88
public class EventManager
{
	// Token: 0x1700012B RID: 299
	// (get) Token: 0x060004CE RID: 1230 RVA: 0x0000569B File Offset: 0x0000389B
	public static EventManager Instance
	{
		get
		{
			if (EventManager._instance == null)
			{
				EventManager._instance = new EventManager();
			}
			return EventManager._instance;
		}
	}

	// Token: 0x060004CF RID: 1231 RVA: 0x0006AF34 File Offset: 0x00069134
	public void AddListener<T>(EventManager.EventDelegate<T> del) where T : GameEvent
	{
		if (this.delegateLookup.ContainsKey(del))
		{
			return;
		}
		EventManager.EventDelegate eventDelegate = delegate(GameEvent e)
		{
			del((T)((object)e));
		};
		this.delegateLookup[del] = eventDelegate;
		EventManager.EventDelegate a;
		if (this.delegates.TryGetValue(typeof(T), out a))
		{
			a = (this.delegates[typeof(T)] = (EventManager.EventDelegate)Delegate.Combine(a, eventDelegate));
		}
		else
		{
			this.delegates[typeof(T)] = eventDelegate;
		}
	}

	// Token: 0x060004D0 RID: 1232 RVA: 0x0006AFE0 File Offset: 0x000691E0
	public void RemoveListener<T>(EventManager.EventDelegate<T> del) where T : GameEvent
	{
		EventManager.EventDelegate value;
		if (this.delegateLookup.TryGetValue(del, out value))
		{
			EventManager.EventDelegate eventDelegate;
			if (this.delegates.TryGetValue(typeof(T), out eventDelegate))
			{
				eventDelegate = (EventManager.EventDelegate)Delegate.Remove(eventDelegate, value);
				if (eventDelegate == null)
				{
					this.delegates.Remove(typeof(T));
				}
				else
				{
					this.delegates[typeof(T)] = eventDelegate;
				}
			}
			this.delegateLookup.Remove(del);
		}
	}

	// Token: 0x060004D1 RID: 1233 RVA: 0x0006B070 File Offset: 0x00069270
	public void Raise(GameEvent e)
	{
		EventManager.EventDelegate eventDelegate;
		if (this.delegates.TryGetValue(e.GetType(), out eventDelegate))
		{
			eventDelegate(e);
		}
	}

	// Token: 0x0400047C RID: 1148
	public static EventManager _instance;

	// Token: 0x0400047D RID: 1149
	public Dictionary<Type, EventManager.EventDelegate> delegates = new Dictionary<Type, EventManager.EventDelegate>();

	// Token: 0x0400047E RID: 1150
	public Dictionary<Delegate, EventManager.EventDelegate> delegateLookup = new Dictionary<Delegate, EventManager.EventDelegate>();

	// Token: 0x020008A8 RID: 2216
	// (Invoke) Token: 0x060051FC RID: 20988
	public delegate void EventDelegate<T>(T e) where T : GameEvent;

	// Token: 0x020008A9 RID: 2217
	// (Invoke) Token: 0x06005200 RID: 20992
	public delegate void EventDelegate(GameEvent e);
}
