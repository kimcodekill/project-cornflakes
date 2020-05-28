using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {

	[SerializeField] private TextMeshProUGUI highScoreText;

	private float highScore;
	private string name;

	//K: THIS SHOULD NOT FIRE A LEVEL END EVENT
	//   bad.
	public void NewGame() {
		EventSystem.Current.FireEvent(new LevelEndEvent(-1, Time.time));
	}

	private void OnEnable()
	{
		name = PlayerPrefs.GetString("scoreName", string.Empty);
		highScore = PlayerPrefs.GetFloat("scoreValue", 0);

		if (!name.Equals(string.Empty) && highScore != 0)
		{
			highScoreText.text = string.Format("High Score: {0} - {1}", name, (int)highScore);
		}
		else
		{
			highScoreText.gameObject.SetActive(false);
		}
	}
}
