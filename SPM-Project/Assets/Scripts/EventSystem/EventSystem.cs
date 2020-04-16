using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour {

	public static EventSystem Current { get => current == null ? current = FindObjectOfType<EventSystem>() : current; }

	public delegate void EventListener(Event e);

	private static EventSystem current;

	private Dictionary<System.Type, List<EventListener>> eventListeners;

	public void RegisterListener<T>(EventListener eventListener) where T : Event {
		System.Type type = typeof(T);
		eventListeners = eventListeners ?? new Dictionary<System.Type, List<EventListener>>();
		if (!eventListeners.ContainsKey(type) || eventListeners[type] == null) eventListeners[type] = new List<EventListener>();
		eventListeners[type].Add(eventListener);
	}

	public void UnRegisterListener<T>(EventListener eventListener) {
		System.Type type = typeof(T);
		if (eventListeners != null && eventListeners.ContainsKey(type) && eventListeners[type] != null)
			eventListeners[type].Remove(eventListener);
	}

	public void FireEvent(Event e) {
		System.Type type = e.GetType();
		if (eventListeners == null || !eventListeners.ContainsKey(type) || eventListeners[type] == null) return;
		for (int i = 0; i < eventListeners[type].Count; i++) eventListeners[type][i](e);
	}

}