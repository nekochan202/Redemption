using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour {
    private Animator animator;
    private const string IS_RUN = "isRun";
    private const string RELOAD_TRIGGER = "Reload";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Player.Instance.OnReloadStarted += HandleReload;
    }
    private void OnDestroy()
    {
        Player.Instance.OnReloadStarted -= HandleReload;
    }

    private void Update()
    {
        animator.SetBool(IS_RUN, Player.Instance.IsRun());
    }

    private void HandleReload()
    {
        animator.SetTrigger(RELOAD_TRIGGER);
    }
}
