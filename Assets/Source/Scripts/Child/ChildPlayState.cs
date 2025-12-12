using StateMachine;
using UnityEngine;

public class ChildPlayState : State
{
    private readonly ChildAnimationController m_childAnimationController;
    private ChildPlayConfiguration m_childPlayConfiguration;

    public float PlayTime;
    public bool IsSwap;

    public ChildPlayState(ChildAnimationController childAnimationController)
    {
        m_childAnimationController = childAnimationController;
    }

    public void SetPlayGame(ChildPlayConfiguration childPlayConfiguration)
    {
        IsSwap = Random.Range(0, 2) == 1;
        PlayTime = Random.Range(childPlayConfiguration.MINPlayTime, childPlayConfiguration.MAXPlayTime);
        
        m_childPlayConfiguration = childPlayConfiguration;
    }

    public override void OnStateEnter()
    {
        foreach (var playItem in m_childPlayConfiguration.PlayItem)
        {
            playItem.SetActive(false);
        }

        m_childAnimationController.SetBool(m_childPlayConfiguration.ChildAnimationType, true);
    }

    public override void OnStateExit()
    {
        IsSwap = false;
        foreach (var playItem in m_childPlayConfiguration.PlayItem)
        {
            playItem.SetActive(true);
        }

        m_childPlayConfiguration.IsActivate = true;
        m_childAnimationController.SetBool(m_childPlayConfiguration.ChildAnimationType, false);
    }
}