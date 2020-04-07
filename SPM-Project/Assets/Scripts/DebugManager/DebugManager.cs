using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour {
	
	private static Text contents;

	private static Dictionary<string, Section> sectionDictionary = new Dictionary<string, Section>();

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
			for (int i = 0; i < Rows.Length; i++) {
				result += (Rows[i] ?? "") + "\n";
			}
			result += "\n";
			lastString = result;
			lastHashCode = newHashCode;
			return result;
		}

	}

	void Start() {
		contents = GetComponentInChildren<Text>();
	}

	public static void AddSection(string header, string[] rows) {
		if (rows == null || rows.Length == 0) throw new ArgumentException("You need to specify a value for every row you wish to reserve, even if each row only contains null for the time being.");
		sectionDictionary.Add(header, new Section(header, rows));
	}

	public static void UpdateRow(string header, int row, string content) {
		sectionDictionary[header].Rows[row] = content;
	}

	public static void UpdateRows(string header, int[] rows, string[] contents) {
		if (rows.Length != contents.Length) throw new ArgumentException("You need to specify an index for every row you wish to update, no more, no less.");
		else for (int i = 0; i < rows.Length; i++) UpdateRow(header, rows[i], contents[i]);
	}

	public static void UpdateAll(string header, string[] contents) {
		if (contents.Length != sectionDictionary[header].Rows.Length) throw new ArgumentException("You need to provide a value for every row, even if it that value is null");
	}

	void Update() {
		contents.text = "";
		foreach (KeyValuePair<string, Section> section in sectionDictionary) {
			contents.text += section.Value.ToString();
		}
	}
}
