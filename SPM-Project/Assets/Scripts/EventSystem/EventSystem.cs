using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour {

	/// <summary>
	/// The current static instance of the EventSystem. Use this member to access the event system.
	/// </summary>
	public static EventSystem Current { get => current == null ? current = FindObjectOfType<EventSystem>() : current; }

	public delegate void EventListener(Event e);

	private static EventSystem current;

	private Dictionary<System.Type, List<EventListener>> eventListeners;

	/// <summary>
	/// Registers an event listener to a certain event.
	/// </summary>
	/// <typeparam name="T">The type of event the listener should listen to.</typeparam>
	/// <param name="eventListener">The callback function.</param>
	public void RegisterListener<T>(EventListener eventListener) where T : Event {
		System.Type type = typeof(T);
		eventListeners = eventListeners ?? new Dictionary<System.Type, List<EventListener>>();
		if (!eventListeners.ContainsKey(type) || eventListeners[type] == null) eventListeners[type] = new List<EventListener>();
		eventListeners[type].Add(eventListener);
	}

	/// <summary>
	/// Unregisters an event listener from a certain event.
	/// </summary>
	/// <typeparam name="T">The type of event the listener no longer should listen to.</typeparam>
	/// <param name="eventListener">The callback function.</param>
	public void UnRegisterListener<T>(EventListener eventListener) {
		System.Type type = typeof(T);
		if (eventListeners != null && eventListeners.ContainsKey(type) && eventListeners[type] != null)
			eventListeners[type].Remove(eventListener);
	}

	/// <summary>
	/// Fires an event to be recieved by the registered listeners.
	/// </summary>
	/// <param name="e">The event to be sent out.</param>
	public void FireEvent(Event e) {
		System.Type type = e.GetType();
		if (eventListeners == null || !eventListeners.ContainsKey(type) || eventListeners[type] == null) return;
		for (int i = 0; i < eventListeners[type].Count; i++) eventListeners[type][i](e);
	}

}