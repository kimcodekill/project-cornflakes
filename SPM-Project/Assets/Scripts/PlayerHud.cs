using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHud : MonoBehaviour
{
	[SerializeField] Image panel;
	[SerializeField] Text bulletsInMag, bulletsInReserve;
	[SerializeField] Slider healthBar;
	[SerializeField] PlayerController player;
	// Start is called before the first frame update
	private Color defaultPanelColour = new Color(1, 1, 1, 0);
	private float flashTime;
	private float flashDuration = 0.1f;
    void Start()
    {
		panel.color = defaultPanelColour;
    }

    // Update is called once per frame
    void Update()
    {
		healthBar.value = player.PlayerCurrentHealth;
		//Debug.Log(player.PlayerCurrentHealth);
		if ((Time.time - flashTime) > flashDuration)
			ResetColor();
		bulletsInMag.text = player.CurrentPlayerWeapon().GetBulletsLeftInMagazine().ToString();
		bulletsInReserve.text = player.CurrentPlayerWeapon().GetBulletsInReserve().ToString();

    }

	public void FlashColor(Color color) {
		panel.color = color;
		flashTime = Time.time;
	}

	private void ResetColor() {
		panel.color = defaultPanelColour;
	}
}
