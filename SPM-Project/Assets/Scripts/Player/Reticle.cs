using UnityEngine;
using UnityEngine.UI;

//Author: Joakim Linna
//Co Author: Viktor Dahlberg
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
            //Viktor wrote this
            //Kim: im not sure we should actually be moving the anchors, but i dont have a better idea so :shrug:
            rect.anchorMin = rect.anchorMax = GetOffset();
        }
    }

    /// <summary>
    /// Throws rays from camera and muzzle, checks if the reticle should move to indicate obstruction.
    /// </summary>
    /// <returns>pixel offset vector</returns>
    private Vector2 GetOffset()
    {
        Vector3 dir = cam.transform.forward;
        
        //Thows a ray forward from the camera
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit camHit, float.PositiveInfinity, rayMask))
        {
            dir = (camHit.point - muzzle.position).normalized;
        }
            
        if (Physics.Raycast(muzzle.position, dir, out RaycastHit muzzleHit, float.PositiveInfinity, rayMask))
        {
            if (muzzleHit.point != camHit.point)
            {
                //Viktor wrote this
                return cam.WorldToViewportPoint(muzzleHit.point);
            }
        }
        
        return center;
    }
}
