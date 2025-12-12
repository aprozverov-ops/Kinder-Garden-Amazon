using System;
using System.Collections.Generic;
using UnityEngine;

public class ChildAnimationController
{
    private readonly Animator m_animator;
    private Dictionary<ChildAnimationType, int> m_animationHashes = new Dictionary<ChildAnimationType, int>();


    public ChildAnimationController(Animator animator)
    {
        m_animator = animator;
        foreach (ChildAnimationType animationType in Enum.GetValues(typeof(ChildAnimationType)))
        {
            m_animationHashes.Add(animationType, Animator.StringToHash(
                animationType.ToString()));
        }
    }

    public void SetBool(ChildAnimationType characterAnimationType, bool value)
    {
        m_animator.SetBool(m_animationHashes[characterAnimationType], value);
    }

    public void SetPlay(ChildAnimationType characterAnimationType, bool isTrigger = false, int layer = 0,
        float time = 0)
    {
        if (isTrigger)
        {
            m_animator.SetTrigger(m_animationHashes[characterAnimationType]);
        }
        else
        {
            m_animator.Play(m_animationHashes[characterAnimationType], layer, time);
        }
    }
}