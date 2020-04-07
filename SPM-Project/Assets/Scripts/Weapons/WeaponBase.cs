using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public enum WeaponType
    {
        Primary,
        Special,
        Heavy
    }

    [SerializeField] [Tooltip("Enable to show bullet information")] protected bool bulletDebug;
    [SerializeField] [Tooltip("Rounds per Minute")] protected float fireRate;
    [SerializeField] [Tooltip("Single Magazine Bullet Amount")] protected float magazineSize;
    [SerializeField] [Tooltip("Max Bullet Count")] protected float maxBullets;
    [SerializeField] [Tooltip("Degrees of Variability")] protected float spread;
    [SerializeField] [Tooltip("Time to Reload")] protected float reloadTime;
    [SerializeField] [Tooltip("Bullet Max Distance")] protected float range;
    [SerializeField] [Tooltip("Bullet Travel Distance")] protected WeaponType weaponType;
    [SerializeField] [Tooltip("Layers bullets can hit")] protected LayerMask bulletMask;

    protected float shotDelay;
    protected float currentBulletsMagazine;
    protected float currentBulletsPack; 

    private Ray lastShot;

    public virtual void Trigger() 
    {
        if (shotDelay >= GetTimeBetweenShots())
        {
            Debug.Log(string.Format("SPS: {0} | TBS: {1} sec ", GetTimeBetweenShots(), shotDelay));
            Shoot();
            shotDelay = 0;
        }
    }

    protected virtual void Shoot() { }

    private void Awake()
    {
        currentBulletsMagazine = magazineSize;
        currentBulletsPack = maxBullets;
    }

    private void FixedUpdate()
    {
        if (shotDelay < GetTimeBetweenShots())
            shotDelay += Time.fixedDeltaTime;
    }

    public void DrawBulletDebug(RaycastHit hit) 
    { 
        Instantiate(Resources.Load("Debug/BulletHit"), hit.point, Quaternion.identity, hit.collider.gameObject.transform);
        lastShot = new Ray(transform.position, (hit.point - transform.position).normalized);
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(lastShot.origin, lastShot.direction, Color.red);
    }


    /// <summary>
    /// Tries reload and returns true if it succeded
    /// </summary>
    /// <returns>
    /// The boolean result
    /// </returns>
    public bool DoReload() 
    {
        if (currentBulletsPack > 0 && currentBulletsMagazine != magazineSize)
        {
            float usedBullets = magazineSize - currentBulletsMagazine;
            currentBulletsPack = currentBulletsPack - usedBullets;
            currentBulletsMagazine = currentBulletsPack > magazineSize ? magazineSize : currentBulletsPack;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool HasAmmo()
    {
        return currentBulletsMagazine > 0;
    }

    protected Vector3 AddSpread(Vector3 v)
    {
        return new Vector3(Random.Range(-spread, spread) + v.x, Random.Range(-spread, spread) + v.y, Random.Range(-spread, spread) + v.z).normalized;
    }

    private float GetTimeBetweenShots()
    {
        return 60.0f / fireRate;
    }
}
