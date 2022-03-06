using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private static readonly int openAnimID = Animator.StringToHash("Open");
    private static readonly int playerInWayAnimID = Animator.StringToHash("PlayerInWay");

    [SerializeField] private Animator doorAnimator;
    [SerializeField] private float autoCloseTime = 5f;

    //field controlled by animation
    public bool Closed;

    public PlayerInWayEvent PlayerInWay;

    private Coroutine autoCloseCoroutine;

    public void SetPlayerInWay(bool value)
    {
        doorAnimator.SetBool(playerInWayAnimID, value);
        PlayerInWay?.Invoke(value);
    }

    public void SetOpen(bool value)
    {
        if (autoCloseCoroutine != null)
        {
            StopCoroutine(autoCloseCoroutine);
        }

        if (value)
        {
            autoCloseCoroutine = StartCoroutine(AutoCloseCoroutine(autoCloseTime));
        }

        doorAnimator.SetBool(openAnimID, value);
    }

    private IEnumerator AutoCloseCoroutine(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        doorAnimator.SetBool(openAnimID, false);
    }
}
