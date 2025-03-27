using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private Animator animator;

    private const string IS_RUN = "isRun";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        animator.SetBool(IS_RUN, Player.Instance.IsRun());
    }
}
