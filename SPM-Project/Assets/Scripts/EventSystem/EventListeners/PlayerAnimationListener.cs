using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author Joakim Linna
//We could combine this with the animator but i havent, so we push that to the future and ill fix it myself if it proves to be a problem
public class PlayerAnimationListener : MonoBehaviour
{
    private void Start()
    {
        EventSystem.Current.RegisterListener<PlayerJumpEvent>(OnPlayerJump);
        EventSystem.Current.RegisterListener<PlayerDashEvent>(OnPlayerDash);
    }

    private void OnPlayerJump(Event e)
    {
        PlayerJumpEvent pje = e as PlayerJumpEvent;

        Debug.Log("Delete this Debug log | " + pje.Description);
    }

    private void OnPlayerDash(Event e)
    {
        PlayerDashEvent pde = e as PlayerDashEvent;

        Debug.Log("Delete this Debug log | " + pde.Description);
    }
}
