using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Event {

	public object Description { get => description; set => description = value.ToString(); }

	private string description;

}