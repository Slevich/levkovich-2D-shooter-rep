using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharSounds : MonoBehaviour
{
    #region Поля
    [Header("Audio Sources which used by other classes to play some sounds.")]
    [SerializeField] private AudioSource shootSounds;
    [SerializeField] private AudioSource movingSounds;
    [SerializeField] private AudioSource voiceSounds;
    [SerializeField] private AudioSource itemSounds;
    [SerializeField] private AudioSource reloadingSounds;
    [SerializeField] private AudioSource emptyClipSounds;
    [Header("Audio Clips which playing in upper Audio Sources.")]
    [SerializeField] private AudioClip[] pistolShootSFX;
    [SerializeField] private AudioClip rifleShootSFX;
    [SerializeField] private AudioClip pistolReloadSound;
    [SerializeField] private AudioClip rifleReloadSound;
    [SerializeField] private AudioClip pistolEmptyClipSound;
    [SerializeField] private AudioClip rifleEmptyClipSound;
    [SerializeField] private AudioClip pistolPickUpSound;
    [SerializeField] private AudioClip riflePickUpSound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip[] footStepsSFX;
    [SerializeField] private AudioClip jumpStartSound;
    [SerializeField] private AudioClip jumpEndSound;
    [SerializeField] private AudioClip niceSound;
    [SerializeField] private AudioClip syringeSound;
    [SerializeField] private AudioClip ammoSound;
    [SerializeField] private AudioClip deathSound;

    /* Переменная типа int необходимая для присваивани ей рандомного числа, 
     * являющегося номером аудио-клипа из массива.
     */
    private int clipNumber;
    #endregion

    #region Методы
    /* Далее идут методы, которые передают в Audio Source
     * определенный Audio Clip, затем проигрывают его.
     */
    public void PlayPistolShootSound()
    {
        clipNumber = Random.Range(0, pistolShootSFX.Length);
        shootSounds.clip = pistolShootSFX[clipNumber];
        shootSounds.Play();
    }

    public void PlayRifleShootSound()
    {
        shootSounds.clip = rifleShootSFX;
        shootSounds.Play();
    }

    public void PlayFootstepSound()
    {
        clipNumber = Random.Range(0, footStepsSFX.Length);
        movingSounds.clip = footStepsSFX[clipNumber];
        movingSounds.Play();
    }

    public void PlayJumpStartSound()
    {
        movingSounds.clip = jumpStartSound;
        movingSounds.Play();
    }

    public void PlayJumpEndSound()
    {
        movingSounds.clip = jumpEndSound;
        movingSounds.Play();
    }

    public void PlaySyringePickingUpSounds()
    {
        voiceSounds.clip = niceSound;
        voiceSounds.Play();
        itemSounds.clip = syringeSound;
        itemSounds.Play();
    }

    public void PlayAmmoPickingUpSound()
    {
        itemSounds.clip = ammoSound;
        itemSounds.Play();
    }

    public void PlayPistolReloadSound()
    {
        if (reloadingSounds.isPlaying == false)
        {
            reloadingSounds.clip = pistolReloadSound;
            reloadingSounds.Play();
        }
    }

    public void PlayRifleReloadSound()
    {
        if (reloadingSounds.isPlaying == false)
        {
            reloadingSounds.clip = rifleReloadSound;
            reloadingSounds.Play();
        }
    }
    
    public void PlayPistolEmptyClipSound()
    {
        emptyClipSounds.clip = pistolEmptyClipSound;
        emptyClipSounds.Play();
    }

    public void PlayRifleEmptyClipSound()
    {
        emptyClipSounds.clip = rifleEmptyClipSound;
        emptyClipSounds.Play();
    }

    public void PlayDeathSound()
    {
        if (voiceSounds.isPlaying == false)
        {
            voiceSounds.clip = deathSound;
            voiceSounds.Play();
        }
    }

    public void PlayWeaponPickUpSound(string weaponType)
    {
        if (weaponType == "Pistol")
        {
            itemSounds.clip = pistolPickUpSound;
            itemSounds.Play();
        }
        else if (weaponType == "Rifle")
        {
            itemSounds.clip = riflePickUpSound;
            itemSounds.Play();
        }
    }

    public void PlayHitSound()
    {
        movingSounds.clip = hitSound;
        movingSounds.Play();
    }
    #endregion
}
