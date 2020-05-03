using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawnable {

	/// <summary>
	/// If this object is spawnable, we can use this to determine whether or not it was manually created or spawned.
	/// </summary>
	void Spawned();

}