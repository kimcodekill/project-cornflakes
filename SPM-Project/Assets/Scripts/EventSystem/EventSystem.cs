using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
		SceneManager.sceneLoaded += OnSceneLoaded;
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
		UnRegisterListener(typeof(T), eventListener);
	}

	private void UnRegisterListener(System.Type type, EventListener eventListener) {
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
		}
	}

	private void RemoveUnRegistered() {
		if (toRemove == null || toRemove.Count == 0) return;
		foreach (KeyValuePair<System.Type, List<EventListener>> kv in toRemove) {
			for (int i = 0; i < toRemove[kv.Key].Count; i++) {
				eventListeners[kv.Key].Remove(toRemove[kv.Key][i]);
			}
		}

		toRemove = null;
	}

	private void OnSceneLoaded(Scene s, LoadSceneMode lsm) {
		if (eventListeners == null || eventListeners.Count == 0) return;
		foreach (KeyValuePair<System.Type, List<EventListener>> kv in eventListeners) {
			for (int i = 0; i < kv.Value.Count; i++) {
				EventListener el = kv.Value[i];;
				if (el.Target.Equals(null)) UnRegisterListener(kv.Key, el);
			}
		}
	}

}