﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
public class DebugListener : MonoBehaviour {

	private void Start() => EventSystem.Current.RegisterListener<HitEvent>(OnHitEvent);

	private void OnHitEvent(Event e) {
		HitEvent he = (HitEvent) e;
		Instantiate(Resources.Load("Debug/BulletHit"), he.HitPoint, Quaternion.identity);
	}

}