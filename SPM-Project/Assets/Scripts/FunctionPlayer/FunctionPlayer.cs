using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionPlayer : MonoBehaviour {

	private static Queue<FunctionToPlay> functionQueue = new Queue<FunctionToPlay>();

	public delegate void FunctionToPlay();

	private void Update() {
		while (functionQueue.Count > 0)
			functionQueue.Dequeue()();
	}

	public static void EnqueueFunction(FunctionToPlay function) {
		functionQueue.Enqueue(function);
	}

}