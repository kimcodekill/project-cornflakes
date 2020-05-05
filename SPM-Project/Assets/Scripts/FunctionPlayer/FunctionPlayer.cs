using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
public class FunctionPlayer : MonoBehaviour {

	public delegate void FunctionToPlay();

	private static Dictionary<FunctionToPlay, DisposalTracker> functionDictionary = new Dictionary<FunctionToPlay, DisposalTracker>();

	private List<FunctionToPlay> toRemove;

	private class DisposalTracker {

		public enum DisposalMode {
			Keep, 
			Iterations,
			Lifetime
		}

		public DisposalMode Mode { get; private set; }
		
		public int iterations;

		public float lifeTime;

		public DisposalTracker() => Mode = DisposalMode.Keep;

		public DisposalTracker(int iterations) {
			Mode = DisposalMode.Iterations;
			this.iterations = iterations;
		}

		public DisposalTracker(float lifeTime) {
			Mode = DisposalMode.Lifetime;
			this.lifeTime = lifeTime;
		}
	
	}

	private void LateUpdate() {
		toRemove = new List<FunctionToPlay>();
		foreach (KeyValuePair<FunctionToPlay, DisposalTracker> function in functionDictionary) {
			function.Key();
			switch (function.Value.Mode) {
				case DisposalTracker.DisposalMode.Keep:
					break;
				case DisposalTracker.DisposalMode.Iterations:
					if (--function.Value.iterations <= 0)
						toRemove.Add(function.Key);
					break;
				case DisposalTracker.DisposalMode.Lifetime:
					if ((function.Value.lifeTime -= Time.deltaTime) <= 0)
						toRemove.Add(function.Key);
					break;
			}
		}
		for (int i = 0; i < toRemove.Count; i++)
			functionDictionary.Remove(toRemove[i]);
	}

	/// <summary>
	/// Adds a function to the Function Player.
	/// </summary>
	/// <param name="function">The function to be added.</param>
	public static void AddFunction(FunctionToPlay function) {
		functionDictionary.Add(function, new DisposalTracker());
	}

	/// <summary>
	/// Adds a function to the Function Player that is removed after the specified iterations.
	/// </summary>
	/// <param name="function">The function to be added.</param>
	/// <param name="iterations">The amount of times it should be played.</param>
	public static void AddFunction(FunctionToPlay function, int iterations = 1) {
		functionDictionary.Add(function, new DisposalTracker(iterations));
	}

	/// <summary>
	/// Adds a function to the Function Player that is removed after the specified lifetime.
	/// </summary>
	/// <param name="function">The function to be added.</param>
	/// <param name="lifeTime">The amount of time it should be played.</param>
	public static void AddFunction(FunctionToPlay function, float lifeTime = 0) {
		functionDictionary.Add(function, new DisposalTracker(lifeTime));
	}

	/// <summary>
	/// Removes a function from the Function Player.
	/// </summary>
	/// <param name="function">The function to be removed.</param>
	public static void RemoveFunction(FunctionToPlay function) {
		functionDictionary.Remove(function);
	}

}