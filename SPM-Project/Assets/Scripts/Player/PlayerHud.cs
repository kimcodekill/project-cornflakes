using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHud : MonoBehaviour
{
	[Header("Player attributes")]
	[SerializeField] [Tooltip("The slider field for player healthbar.")] private Slider healthBar;
	[SerializeField] [Tooltip("Text for bullets left in current magazine.")] private Text bulletsInMag; 
	[SerializeField] [Tooltip("Text for bullets in reserve, not counting magazine.")] private Text bulletsInReserve;
	[SerializeField] [Tooltip("Text for pick-up information")] private Text pickupText;

	[Header("Hud behaviour controls")]
	[SerializeField] [Tooltip("HUD border image.")] private Image hudBorder;
	[SerializeField] [Tooltip("The Player the HUD belongs to.")] private PlayerController player;
	private Color defaultPanelColour = new Color(1, 1, 1, 0);
	private float flashDuration = 0.1f;
	private PlayerWeapon playerWeapon;
	private Animator anim;
	private List<Text> activepickupTexts = new List<Text>();

	private void Start()
    {
		hudBorder.color = defaultPanelColour;
		playerWeapon = player.gameObject.GetComponent<PlayerWeapon>();
		//anim = GetComponentInChildren<Animator>();
	}

    private void Update()
    {
		UpdateHealthBar();
		UpdateBulletCount();
    }

	/// <summary>
	/// Flashes the HUD border in the given colour for flashDuration.
	/// </summary>
	/// <param name="color">The desired colour to flash the HUD border (RGBA).</param>
	public void FlashColor(Color color) {
		hudBorder.color = color;
		Invoke("ResetColor", flashDuration);
	}

	private void ResetColor() {
		hudBorder.color = defaultPanelColour;
	}

	private void UpdateBulletCount() {
		bulletsInMag.text = playerWeapon.CurrentWeapon?.AmmoInMagazine.ToString();
		bulletsInReserve.text = playerWeapon.CurrentWeapon?.AmmoInReserve.ToString();
	}

	private void UpdateHealthBar() {
		healthBar.value = player.PlayerCurrentHealth;

	}

	public void ShowPickupText(string type, float amount) {
		if (activepickupTexts.Count > 0) foreach (Text text in activepickupTexts) {
				text.rectTransform.anchoredPosition = new Vector2(text.rectTransform.anchoredPosition.x, text.rectTransform.anchoredPosition.y + 30);
			}
		else foreach (Text text in activepickupTexts) {
				activepickupTexts.Remove(text);
				Destroy(text.gameObject);
			}
		Text newText = Instantiate(pickupText, this.gameObject.transform.GetChild(0));
		activepickupTexts.Add(newText);

		if (amount != 0) newText.text = amount + " " + type + " picked up";
		else newText.text = type + " picked up";
	}
}
