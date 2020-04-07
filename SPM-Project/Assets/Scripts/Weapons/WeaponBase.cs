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
    [SerializeField] [Tooltip("Bullet Travel Distance")] protected float range;
    [SerializeField] [Tooltip("Layers bullets can hit")] protected LayerMask bulletMask;

    private Ray lastShot;

    public virtual void Trigger() { }

    public virtual void Reload() { }

    public void DrawBulletDebug(RaycastHit hit) 
    { 
        Instantiate(Resources.Load("Debug/BulletHit"), hit.point, Quaternion.identity);
        lastShot = new Ray(transform.position, (hit.point - transform.position).normalized);
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(lastShot.origin, lastShot.direction, Color.red);
    }
}
