using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour {
	
	private static Text contents;

	private static Dictionary<string, Section> sectionDictionary = new Dictionary<string, Section>();

	private static bool registered;

	private struct Section {
		
		public string Header;
		public string[] Rows;

		private int lastHashCode;
		private string lastString;

		public Section(string header, string[] rows) {
			Header = header;
			Rows = rows;
			
			lastHashCode = -1;
			lastString = null;
		}
		
		public override string ToString() {
			int newHashCode = Rows.GetHashCode();
			if (newHashCode == lastHashCode) return lastString;
			string result = "[" + Header + "]\n";
			for (int i = 0; i < Rows.Length; i++) result += (Rows[i] ?? "") + "\n";
			result += "\n";
			lastString = result;
			lastHashCode = newHashCode;
			return result;
		}

	}

	private static void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
		sectionDictionary = new Dictionary<string, Section>();
	}

	/// <summary>
	/// Create a new debug information section to be displayed on the onscreen overlay.
	/// To reserve a row, simply add it to the string parameter list as null.
	/// </summary>
	/// <param name="header">The name of the section.</param>
	/// <param name="contents">The rows to be reserved for usage by the section</param>
	/// <exception cref="System.ArgumentException">Thrown if no rows have been provided.</exception>
	public static void AddSection(string header, params string[] contents) {
		if (!registered) {
			SceneManager.sceneLoaded += OnSceneLoaded;
			registered = true;
		}
		if (contents == null || contents.Length == 0) throw new ArgumentException("You need to specify a value for every row you wish to reserve, even if some row(s) only contain null for the time being.");
		sectionDictionary.Add(header, new Section(header, contents));
	}

	/// <summary>
	/// Updates the contents of a specified row with the specified content in the specified section.
	/// </summary>
	/// <param name="header">The name of the section whose row to update.</param>
	/// <param name="row">The index of the row to update.</param>
	/// <param name="content">The content to be placed on the specified row.</param>
	public static void UpdateRow(string header, int row, string content) {
		sectionDictionary[header].Rows[row] = content;
	}

	/// <summary>
	/// Updates the first row with the specified content in the specified section.
	/// </summary>
	/// <param name="header">The name of the section whose row to update.</param>
	/// <param name="content">The content to be placed on the first row.</param>
	public static void UpdateRow(string header, string content) {
		UpdateRow(header, 0, content);
	}

	/// <summary>
	/// Updates the contents of the specified rows with the specified contents in the specified section.
	/// </summary>
	/// <param name="header">The name of the section whose rows to update.</param>
	/// <param name="rows">The indexes of the rows to update.</param>
	/// <param name="contents">The contents to be placed in the specified rows.</param>
	/// <exception cref="System.ArgumentException">Thrown if not enough row indexes have been provided to assign the contents, or vice versa.</exception>
	public static void UpdateRows(string header, int[] rows, params string[] contents) {
		if (rows.Length != contents.Length) throw new ArgumentException("You need to specify an index for every row you wish to update, no more, no less.");
		else for (int i = 0; i < rows.Length; i++) UpdateRow(header, rows[i], contents[i]);
	}

	/// <summary>
	/// Updates the contents of every row with the specified contents in the specified section.
	/// </summary>
	/// <param name="header">The name of the section to update.</param>
	/// <param name="contents">The contents to be placed.</param>
	/// <exception cref="System.ArgumentException">Thrown if not enough values have been provided.</exception>
	public static void UpdateAll(string header, params string[] contents) {
		if (contents.Length != sectionDictionary[header].Rows.Length) throw new ArgumentException("You need to provide a value for every row, even if some value(s) only contain null for the time being.");
		for (int i = 0; i < sectionDictionary[header].Rows.Length; i++) UpdateRow(header, i, contents[i]);
	}

	private void Start() {
		contents = GetComponentInChildren<Text>();
	}

	private void Update() {
		contents.text = "";
		foreach (KeyValuePair<string, Section> section in sectionDictionary) contents.text += section.Value.ToString() + "\n";
	}

}