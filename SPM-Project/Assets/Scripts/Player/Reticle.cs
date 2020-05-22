using UnityEngine;
using UnityEngine.UI;

//Author: Joakim Linna
//Co Author: Viktor Dahlberg
public class Reticle : MonoBehaviour
{
    public static Reticle Instance;

    [SerializeField] private LayerMask rayMask;
    [SerializeField] private Image reticleImage;
	[SerializeField] private float playerTransparentViewportYTreshold;

    private Camera cam;
    private Transform Muzzle { get => PlayerWeapon.Instance.Muzzle; }

    private RectTransform rect;

	//the center of the screen in viewport coordinates
	private Vector2 center = new Vector2(0.5f, 0.5f);

	private PlayerRenderer playerRenderer;

    private void Start()
    {
        if (Instance == null) { 
            Instance = this;
            Init();
        }
    }

    private void Init()
    {
        cam = Camera.main;

        rect = reticleImage.rectTransform;

        playerRenderer = cam.GetComponent<PlayerRenderer>();

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
			Vector2 offset = GetOffset();
            rect.anchorMin = rect.anchorMax = offset;
			if (offset.y < playerTransparentViewportYTreshold) playerRenderer.SetRenderMode(RenderMode.Transparent);
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
            dir = (camHit.point - Muzzle.position).normalized;
        }
            
        if (Physics.Raycast(Muzzle.position, dir, out RaycastHit muzzleHit, float.PositiveInfinity, rayMask))
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
