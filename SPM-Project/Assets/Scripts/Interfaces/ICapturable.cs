using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
/// <summary>
/// Everything that wants to be captured should implement this interface,
/// provided whether or not the GameObject is active is the only saved parameter.
/// </summary>
public interface ICapturable {

	/// <summary>
	/// Ran on checkpoint load.
	/// </summary>
	/// <param name="wasEnabled">Whether or not the loading of the GameObject resulted in it being enabled.</param>
	void OnLoad(bool wasEnabled);

	/// <summary>
	/// Some form of UUID that can be associated with one and only GameObject at any time.
	/// It is recommended that this is the position of the object, since GameObjects very rarely share positions.
	/// If the object can move, the origin of the object may be provided instead.
	/// </summary>
	/// <returns>The ID that will be used to restore the captured object.</returns>
	object GetPersistentCaptureID();

}