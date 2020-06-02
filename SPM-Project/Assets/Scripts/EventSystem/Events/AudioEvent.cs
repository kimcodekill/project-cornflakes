using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AudioEvent : Event
{
    public AudioEvent(AudioClip audioClip, AudioSource audioSource)
    {
        AudioClip = audioClip;
        AudioSource = audioSource;
    }

    public AudioClip AudioClip { get; }
    public AudioSource AudioSource { get; }
}

public class WeaponFiredEvent : AudioEvent
{
    public WeaponFiredEvent(AudioClip audioClip, AudioSource audioSource, bool loop, bool interrupt) : base(audioClip, audioSource) 
    {
        Loop = loop;
    }

    public bool Loop { get; }
}

public class WeaponReloadingEvent : AudioEvent
{
    public WeaponReloadingEvent(AudioClip audioClip, AudioSource audioSource, bool interrupt) : base(audioClip, audioSource) { }
}

public class WeaponSwitchedEvent : AudioEvent
{
    public WeaponSwitchedEvent(AudioClip audioClip, AudioSource audioSource, bool interrupt, Weapon selectedWeapon) : base(audioClip, audioSource)
    {
        SelectedWeapon = selectedWeapon;
    }

    public Weapon SelectedWeapon { get; }
}

public class WeaponFireStoppedEvent : AudioEvent
{
    public WeaponFireStoppedEvent(AudioClip audioClip, AudioSource audioSource, bool interrupt) : base(audioClip, audioSource) { }
}