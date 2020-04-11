using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FpStd = FunctionPlayerStandardLibrary;

public class FunctionPlayerDemo : MonoBehaviour {

	private void Start() {
		FunctionPlayer.EnqueueFunction(FpStd.SayHi);
	}

}