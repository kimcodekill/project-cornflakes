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
	[SerializeField] [Tooltip("Text for pickup information")] private Text pickupText;

	[Header("Hud behaviour controls")]
	[SerializeField] [Tooltip("HUD border image.")] private Image hudBorder;
	[SerializeField] [Tooltip("The Player the HUD belongs to.")] private PlayerController player;
	private Color defaultPanelColour = new Color(1, 1, 1, 0);
	private float flashDuration = 0.1f;
	private PlayerWeapon playerWeapon;
	private Animator anim;

	private void Start()
    {
		hudBorder.color = defaultPanelColour;
		playerWeapon = player.gameObject.GetComponent<PlayerWeapon>();
		anim = GetComponentInChildren<Animator>();
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
	public void PickupText(string type, float amount)
	{
		pickupText.text = amount + " " + type + " picked up";
		//anim.SetBool("Pickedup", true);
		anim.ResetTrigger("PickedUp");
		anim.SetTrigger("PickedUp");
	}
}
