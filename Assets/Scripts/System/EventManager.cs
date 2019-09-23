using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EVENT_ID
{
	NONE,
	START_BATTLE,
}

public class EventUserDara { }

public class EventManager
{
	public class EventData
	{
		public EVENT_ID eventId;
		public EventUserDara userData;
		public onEventFunction function;
	}

	public delegate void onEventFunction(EventUserDara userData);

	static Dictionary<int, List<EventData>> eventListeners = new Dictionary<int, List<EventData>>();

	public static void AddEventListener(EVENT_ID eventId, onEventFunction function)
	{
		if (!eventListeners.ContainsKey((int)eventId))
		{
			var eventDatas = new List<EventData>();
			eventDatas.Add(new EventData()
			{
				eventId = eventId,
				function = function,
				userData = null
			});

			eventListeners.Add((int)eventId, eventDatas);
		}
		else
		{
			var eventDatas = eventListeners[(int)eventId];
			eventDatas.Add(new EventData()
			{
				eventId = eventId,
				function = function,
				userData = null
			});
		}
	}

	public static void Dispatcher(EVENT_ID eventId, EventUserDara userData = null)
	{
		var eventDatas = eventListeners[(int)eventId];
		for (int i = 0; i < eventDatas.Count; i++)
		{
			eventDatas[i].function(eventDatas[i].userData);
		}
	}

	public static void ClearEventListener(EVENT_ID eventId)
	{
		var eventDatas = eventListeners[(int)eventId];
		eventDatas.Clear();
	}
}
