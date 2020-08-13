using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FpStd = FunctionPlayerStandardLibrary;

//Author: Viktor Dahlberg
public class FunctionPlayerDemo : MonoBehaviour {

	[SerializeField] private GameObject someGameObject;

	private void Start() {
		FunctionPlayer.AddFunction(FpStd.SayHi, 2f);
		FunctionPlayer.AddFunction(InstantiateSomeGameObject, 2);
	}

	private void InstantiateSomeGameObject() {
		Instantiate(someGameObject);
	}

}