using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private static readonly int openAnimID = Animator.StringToHash("Open");
    private static readonly int playerInWayAnimID = Animator.StringToHash("PlayerInWay");

    [SerializeField] private Animator doorAnimator;
    [SerializeField] private float autoCloseTime = 5f;
    [SerializeField] private AudioSource audioSource;

    //field controlled by animation
    public bool Closed;

    public PlayerInWayEvent PlayerInWay;

    public void SetPlayerInWay(bool value)
    {
        doorAnimator.SetBool(playerInWayAnimID, value);
        PlayerInWay?.Invoke(value);
    }

    public void SetOpen(bool value)
    {
       StopAllCoroutines();

        if (value)
        {
            StartCoroutine(AutoCloseCoroutine(autoCloseTime));
        }

        doorAnimator.SetBool(openAnimID, value);
    }

    public void PlayDoorSound()
    {
        audioSource.Play();
    }

    private IEnumerator AutoCloseCoroutine(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        doorAnimator.SetBool(openAnimID, false);
    }
}
