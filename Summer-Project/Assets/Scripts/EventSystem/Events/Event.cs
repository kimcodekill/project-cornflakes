using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
public abstract class Event {

	public object Description { get => description; set => description = value.ToString(); }

	private string description;

}