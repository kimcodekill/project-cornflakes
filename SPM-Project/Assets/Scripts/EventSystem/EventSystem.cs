using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
public class EventSystem : MonoBehaviour {

	//K: Using FindObjectOfType feels dangerous, but what do i know.
	/// <summary>
	/// The current static instance of the EventSystem. Use this member to access the event system.
	/// </summary>
	public static EventSystem Current;

	public delegate void EventListener(Event e);

	private Dictionary<System.Type, List<EventListener>> eventListeners;

	private Dictionary<System.Type, List<EventListener>> toRemove;

	private void OnEnable()
	{
		if(Current == null) { 
			Current = this;
			DontDestroyOnLoad(gameObject);
		}
		else if (Current != this) { Destroy(gameObject); }
	}

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
		//Debug.Log("Registered " + eventListener.Target + ":" + eventListener.Method.Name);
	}

	/// <summary>
	/// Unregisters an event listener from a certain event.
	/// It has no explicit security check for unregistering nonexistent listeners.
	/// </summary>
	/// <typeparam name="T">The type of event the listener no longer should listen to.</typeparam>
	/// <param name="eventListener">The callback function.</param>
	public void UnRegisterListener<T>(EventListener eventListener) {
		System.Type type = typeof(T);
		toRemove = toRemove ?? new Dictionary<System.Type, List<EventListener>>();
		if (!toRemove.ContainsKey(type) || toRemove[type] == null) toRemove[type] = new List<EventListener>();
		toRemove[type].Add(eventListener);
	}

	/// <summary>
	/// Fires an event to be recieved by the registered listeners.
	/// </summary>
	/// <param name="e">The event to be sent out.</param>
	public void FireEvent(Event e) {
		RemoveUnRegistered();
		System.Type type = e.GetType();
		if (eventListeners == null || !eventListeners.ContainsKey(type) || eventListeners[type] == null) return;
		for (int i = 0; i < eventListeners[type].Count; i++) {
			eventListeners[type][i](e);
			//Debug.Log(e + " sent to " + eventListeners[type][i].Target + ":" + eventListeners[type][i].Method.Name);
		}
	}

	private void RemoveUnRegistered() {
		if (toRemove == null || toRemove.Count == 0) return;
		foreach (KeyValuePair<System.Type, List<EventListener>> kv in toRemove) {
			for (int i = 0; i < toRemove[kv.Key].Count; i++) {
				eventListeners[kv.Key].Remove(toRemove[kv.Key][i]);
				//Debug.Log("Unregistered " + toRemove[kv.Key][i].Target + ":" + toRemove[kv.Key][i].Method.Name);
			}
		}

		toRemove = null;
	}
}