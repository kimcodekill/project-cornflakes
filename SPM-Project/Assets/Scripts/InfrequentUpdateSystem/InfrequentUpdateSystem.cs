using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfrequentUpdateSystem : MonoBehaviour {

	/// <summary>
	/// I made this to improve code readability in array accesses.
	/// 0 is the function delegate,
	/// 1 is the update interval,
	/// 2 is the currently elapsed time since the last function call.
	/// </summary>
	private struct DataIndex {
		public const int
			Function = 0,
			Interval = 1,
			Elapsed = 2;
	}

	/// <summary>
	/// The current static instance of the InfrequentSystem. Use this member to access the system.
	/// </summary>
	public static InfrequentUpdateSystem Current { get => current == null ? current = FindObjectOfType<InfrequentUpdateSystem>() : current; }

	private static InfrequentUpdateSystem current;

	public delegate void InfrequentUpdate();

	/// <summary>
	/// The data structure for the listener.
	/// </summary>
	private List<object[]> infrequentUpdateListeners;

	/// <summary>
	/// Adds a function to be played at some interval.
	/// </summary>
	/// <param name="infrequentUpdate">The delegate function.</param>
	/// <param name="updateIntervalSeconds">The time between updates in seconds.</param>
	public void Register(InfrequentUpdate infrequentUpdate, float updateIntervalSeconds) {
		if (infrequentUpdateListeners == null) infrequentUpdateListeners = new List<object[]>();
		infrequentUpdateListeners.Add(new object[3] { infrequentUpdate, updateIntervalSeconds, 0f });
	}

	/// <summary>
	/// Removes a function from the infrequent update system.
	/// </summary>
	/// <param name="infrequentUpdate">The delegate function to be removed.</param>
	public void Unregister(InfrequentUpdate infrequentUpdate) {
		if (infrequentUpdateListeners == null || infrequentUpdateListeners.Count == 0) return;
		int toRemoveIndex = 0;
		for (int i = 0; i < infrequentUpdateListeners.Count; i++) {
			if (infrequentUpdateListeners[i][DataIndex.Function] == infrequentUpdate) toRemoveIndex = i;
		}
		infrequentUpdateListeners.RemoveAt(toRemoveIndex);
	}

	private void Update() {
		if (infrequentUpdateListeners == null || infrequentUpdateListeners.Count == 0) return;
		for (int i = 0; i < infrequentUpdateListeners.Count; i++) {
			float interval = (float) infrequentUpdateListeners[i][DataIndex.Interval];
			float elapsedTime = (float) infrequentUpdateListeners[i][DataIndex.Elapsed];
			if (IsTimeToRetrigger(elapsedTime += Time.deltaTime, interval)) {
				elapsedTime = 0;
				CallRegisteredFunction((InfrequentUpdate) infrequentUpdateListeners[i][DataIndex.Function]);
			}
			if (infrequentUpdateListeners.Count > 0) infrequentUpdateListeners[i][DataIndex.Elapsed] = elapsedTime;
		}
	}

	#region Helper Functions

	private void CallRegisteredFunction(InfrequentUpdate infrequentUpdate) {
		infrequentUpdate();
	}

	private bool IsTimeToRetrigger(float accumulatedTime, float interval) {
		return accumulatedTime > interval;
	}

	#endregion

}