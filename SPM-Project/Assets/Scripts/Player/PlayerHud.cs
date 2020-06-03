using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

//Author: Erik Pilström
public class PlayerHud : MonoBehaviour
{
	public static PlayerHud Instance;

	[Header("Player attributes")]
	[SerializeField] [Tooltip("The slider field for player healthbar.")] private Slider healthBar;
	[SerializeField] [Tooltip("Text for bullets left in current magazine.")] private Text bulletsInMag; 
	[SerializeField] [Tooltip("Text for bullets in reserve, not counting magazine.")] private Text bulletsInReserve;
	[SerializeField] private TextMeshProUGUI[] carriedWeapons;
	[SerializeField] private Material textMaterial;
	[SerializeField] private Material highlightedTextMaterial;
	[SerializeField] [Tooltip("Text for pick-up information")] private TextMeshProUGUI pickupText;

	[Header("Hud behaviour controls")]
	[SerializeField] [Tooltip("HUD border image.")] private Image hudBorder;
	//K: This reference to PlayerController seems outdated
	[SerializeField] [Tooltip("The Player the HUD belongs to.")] private PlayerController player;
	
	private Color defaultPanelColour = new Color(1, 1, 1, 0);
	private float flashDuration = 0.1f;
	private PlayerWeapon playerWeapon;
	private int activeWeapon;
	private List<TextMeshProUGUI> activePickupTexts = new List<TextMeshProUGUI>();
	private ScoreHandler scoreHandler;


	public ScoreHandler ScoreHandler { get => scoreHandler;}

    private void Start()
    {
		if (Instance == null) { Instance = this; }

		hudBorder.color = defaultPanelColour;
		playerWeapon = player.gameObject.GetComponent<PlayerWeapon>();
		scoreHandler = GetComponentInChildren<ScoreHandler>();
		/*foreach (TextMeshProUGUI weapon in carriedWeapons) {
			weapon.gameObject.SetActive(false);
		}*/
		for (int i = 0; i < gameObject.GetComponentInParent<PlayerWeapon>().GetWeapons().Count; i++) {
			carriedWeapons[i].gameObject.SetActive(true);
		}
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

	public void NewCarriedWeapon(int weapon) {
		carriedWeapons[weapon].gameObject.SetActive(true);
		if (PlayerWeapon.Instance.GetWeapons().Count == 1) UpdateActiveWeapon(0);
	}

	public void UpdateActiveWeapon(int weapon) {
		carriedWeapons[activeWeapon].fontMaterial = textMaterial;
		carriedWeapons[weapon].fontMaterial = highlightedTextMaterial;
		activeWeapon = weapon;
	}

	public void ShowPickupText(string type, float amount, string wordchoice) {
		if (activePickupTexts.Count > 0) {
			for (int i = activePickupTexts.Count - 1; i >= 0; i--) {
				TextMeshProUGUI text = activePickupTexts[i];
				if (text.alpha == 0) {
					activePickupTexts.Remove(text);
					Destroy(text.gameObject);
				}
				else text.rectTransform.anchoredPosition = new Vector2(text.rectTransform.anchoredPosition.x, text.rectTransform.anchoredPosition.y + 30);
			}
		}

		TextMeshProUGUI newText = Instantiate(pickupText, transform.GetChild(0));
		activePickupTexts.Add(newText);

		if (amount != 0) newText.text = amount + " " + type + " " + wordchoice;
		else newText.text = type + " " + wordchoice;
	}
}
