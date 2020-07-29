using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class AnimationLogic : MonoBehaviour
{
    public Animator animatorController;

    private void Start()
    {
        animatorController = GetComponent<Animator>();
    }
}
