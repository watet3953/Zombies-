using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePool : ObjectPool<ZombieAI>
{
    protected override void OnGet(ZombieAI obj)
    {
        obj.player = GameManager.Instance.Player.Body;
        obj.enabled = true;
        obj.gameObject.SetActive(true);
        obj.ResetFSM();
    }

    protected override void OnReturn(ZombieAI obj)
    {
        obj.enabled = false;
        obj.gameObject.SetActive(false);

    }
}
