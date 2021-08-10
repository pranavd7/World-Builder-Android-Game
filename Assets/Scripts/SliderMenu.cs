using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderMenu : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void ShowHideMenu()
    {
        bool show = animator.GetBool("show");
        animator.SetBool("show", !show);
    }

    public void HideMenu()
    {
        animator.SetBool("show", false);
    }
}
