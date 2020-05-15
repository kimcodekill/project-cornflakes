using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UnityEngine.Windows.WebCam;

public class Reticle : MonoBehaviour
{
    [SerializeField] private LayerMask rayMask;
    [SerializeField] private Image reticleImage;

    private Camera cam;
    private Transform muzzle;

    private RectTransform rect;

	//the center of the screen in viewport coordinates
	private Vector2 center = new Vector2(0.5f, 0.5f);

    private void Start()
    {

        
        cam = Camera.main;
        Debug.Log(cam.pixelWidth + ":" + cam.pixelHeight);
        //I'm using the PlayerWeaponInstanceMuzzle bc the weapons dont have it themselves
        muzzle = PlayerWeapon.Instance.Muzzle;

        rect = reticleImage.rectTransform;

        EventSystem.Current.RegisterListener<WeaponPickUpEvent>(OnWeaponPickup);
        EventSystem.Current.RegisterListener<WeaponSwitchedEvent>(OnWeaponSwitch);
    }

    private void OnWeaponSwitch(Event e)
    {
        WeaponSwitchedEvent wse = e as WeaponSwitchedEvent;

        reticleImage.sprite = wse.SelectedWeapon.Reticle;
    }

    /// <summary>
    /// Only registered for the first Fire.
    /// Enables inner reticle and sets Muzzle for first time.
    /// </summary>
    /// <param name="e"></param>
    private void OnWeaponPickup(Event e)
    {
        reticleImage.enabled = true;

        EventSystem.Current.UnRegisterListener<WeaponPickUpEvent>(OnWeaponPickup);
    }

    private void Update()
    {
        if (reticleImage.enabled)
        {
            //commented out by Viktor//rect.localPosition = GetOffset();
			rect.anchorMin = GetOffset();
			rect.anchorMax = GetOffset();
        }
    }

    /// <summary>
    /// Throws rays from camera and muzzle, checks if the reticle should move to indicate obstruction.
    /// </summary>
    /// <returns>pixel offset vector</returns>
    private Vector2 GetOffset()
    {
        //Thows a ray forward from the camera
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit camHit, float.PositiveInfinity, rayMask))
        {
            Vector3 hitDirection = (camHit.point - muzzle.position).normalized;

            //Throws a ray from the muzzle to the camera ray hitpoint
            if (Physics.Raycast(muzzle.position, hitDirection, out RaycastHit muzzleHit, float.PositiveInfinity, rayMask))
            {
                if (muzzleHit.point != camHit.point)
                {
					//commented out by Viktor//float muzzleHitCanvasY = (cam.WorldToViewportPoint(muzzleHit.point).y * cam.scaledPixelHeight) - (cam.scaledPixelHeight / 2.0f);
					return cam.WorldToViewportPoint(muzzleHit.point);
					//commented out by Viktor//return new Vector2(0.0f, muzzleHitCanvasY);
				}
			}
        }
        return center;
    }
}
