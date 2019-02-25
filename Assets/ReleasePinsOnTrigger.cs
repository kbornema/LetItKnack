using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReleasePinsOnTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.PinLockLine.UnlockAll();
    }
}
