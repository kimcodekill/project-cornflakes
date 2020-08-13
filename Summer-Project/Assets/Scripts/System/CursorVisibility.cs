using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
public class CursorVisibility : MonoBehaviour {

	#region Properties

	/// <summary>
	/// Singleton
	/// </summary>
	public static CursorVisibility Instance;
	
	/// <summary>
	/// Whether or not ESC and LMB can be used to change the cursor state.
	/// </summary>
	public bool IgnoreInput { get; set; }

	/// <summary>
	/// Whether or not the cursor is currently enabled.
	/// </summary>
	public bool IsEnabled { get => isEnabled; private set => isEnabled = value; }

	#endregion

	#region Serialized

	[SerializeField] [Tooltip("The initial and current state of the cursor")] private bool isEnabled;

	#endregion

	private void OnEnable() {
		if (Instance == null) Instance = this;
		SetCursorEnabled(isEnabled);
	}

	private void Update() {
		if (!IgnoreInput) {
			if (Input.GetKeyDown(KeyCode.Escape) && !isEnabled) EnableCursor();
			else if (Input.GetKeyDown(KeyCode.Mouse0) && isEnabled) DisableCursor();
		}
	}

	/// <summary>
	/// "Enables" the cursor, thereby unlocking it and making it visible.
	/// </summary>
	public void EnableCursor() {
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		isEnabled = true;
	}

	/// <summary>
	/// "Disables" the cursor, thereby locking it and making it invisible.
	/// </summary>
	public void DisableCursor() {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		isEnabled = false;
	}

	/// <summary>
	/// Does the same thing as the <c>EnableCursor()</c> and <c>DisableCursor()</c> functions,
	/// but provides a way to more easily bind the cursor state to an automated script,
	/// i.e. <c>SetCursorEnabled(someBool)</c>
	/// </summary>
	/// <param name="enabled">Whether or not the cursor should be enabled.</param>
	public void SetCursorEnabled(bool enabled) {
		if (enabled) EnableCursor();
		else DisableCursor();
	}

}