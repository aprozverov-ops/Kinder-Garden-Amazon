using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Kuhpik;
using UnityEngine;
using UnityEngine.AI;

public class Mother : MonoPooled
{
    [SerializeField] private MotherConfiguration m_configuration;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform childPosition;
    [SerializeField] private NavMeshAgent navMeshAgent;

    public bool IsPlaceFree => child == null;
    private Vector3 m_startPos;
    private Vector3 m_movePosition;
    private Child child;

    public Child Child => child;
    public bool IsSecond;
    private bool IsFirstMother;
    private bool isTutor;

    public MotherSpawnConfiguration MotherSpawner;

    public void MotherInitialize(Transform movePosition, bool isSecond)
    {
        GetComponent<NavMeshAgent>().enabled = true;
        IsSecond = isSecond;
        IsFirstMother = true;
        animator.SetBool("InHand", true);
        m_startPos = transform.position;
        m_movePosition = movePosition.position;
        var newChild = Bootstrap.Instance.GetSystem<ChildSpawner>().SpawnNewChild(isSecond);
        newChild.transform.parent = childPosition;
        newChild.transform.localPosition = Vector3.zero;
        newChild.transform.localRotation = Quaternion.identity;
        child = newChild;
    }

    public void MotherTutorial(Transform firstPos, Transform secondPos)
    {
        isTutor = true;
        IsFirstMother = false;
        animator.SetBool("InHand", false);
        m_startPos = secondPos.position;
        m_movePosition = firstPos.position;
    }

    public void MotherTakeChild(Transform movePosition, bool isSecond)
    {
        IsSecond = isSecond;
        IsFirstMother = false;
        animator.SetBool("InHand", false);
        m_startPos = transform.position;
        GetComponent<NavMeshAgent>().enabled = true;
        m_movePosition = movePosition.position;
    }

    public override void Initialize()
    {
        base.Initialize();
        m_movePosition = Vector3.zero;
    }

    public void TakeChild(Child child, bool isSecond)
    {
        IsSecond = isSecond;
        Bootstrap.Instance.GetSystem<MoneySpawnSystem>()
            .SpawnMoney(transform, child.IsSecondChild, 5, m_configuration.Money);
        this.child = child;
        child.ToEnd();
        animator.SetBool("InHand", true);
        child.transform.parent = childPosition;
        child.transform.DOLocalJump(Vector3.zero, 1, 1, 0.3f);
        child.transform.DOLocalRotateQuaternion(Quaternion.identity, 0.3f);
        m_movePosition = m_startPos;
    }

    private void Update()
    {
        if (m_movePosition != Vector3.zero)
        {
            navMeshAgent.SetDestination(m_movePosition);
        }

        if (IsFirstMother)
        {
            FirstMother();
        }
        else
        {
            SecondMother();
        }
    }

    private void SecondMother()
    {
        if (Vector3.Distance(transform.position, m_movePosition) < 2)
        {
            if (m_movePosition == m_startPos)
            {
                if (isTutor == false)
                    if (child.IsSecondChild)
                    {
                        Bootstrap.Instance.GameData.amountChildSecond--;
                        Bootstrap.Instance.GameData.amountChildSecondFake--;
                    }
                    else
                    {
                        Bootstrap.Instance.GameData.amountChildFirst--;
                        Bootstrap.Instance.GameData.amountChildFirstFake--;
                    }

                if (isTutor == false)
                {
                    child.ReturnToPool();
                    child = null;
                }

                if (isTutor)
                {
                    Destroy(gameObject);
                    if (child.IsSecondChild)
                    {
                        Bootstrap.Instance.GameData.amountChildSecondFake--;
                    }
                    else
                    {
                        Bootstrap.Instance.GameData.amountChildFirstFake--;
                    }
                }
                else
                {
                    ReturnToPool();
                }
            }
            else
            {
                animator.SetBool("Idle", true);
            }
        }
        else
        {
            animator.SetBool("Idle", false);
        }
    }

    private void FirstMother()
    {
        if (Vector3.Distance(transform.position, m_movePosition) < 2)
        {
            if (m_movePosition != m_startPos)
            {
                if (MotherSpawner.IsReadyToUnlockChild == false)
                {
                    animator.SetBool("Idle", true);
                    return;
                }

                animator.SetBool("Idle", false);
                animator.SetBool("InHand", false);
                child.Initialize();
                child.transform.parent = null;
                child.transform.DOJump(m_movePosition, 1f, 1, 0.4f);
                child.transform.DORotate(Vector3.zero, 0.4f);
                m_movePosition = m_startPos;
                return;
            }

            if (m_movePosition == m_startPos)
            {
                child = null;
                if (isTutor)
                {
                    Destroy(gameObject);
                    if (child.IsSecondChild)
                    {
                        Bootstrap.Instance.GameData.amountChildSecond--;
                        Bootstrap.Instance.GameData.amountChildSecondFake--;
                    }
                    else
                    {
                        Bootstrap.Instance.GameData.amountChildFirst--;
                        Bootstrap.Instance.GameData.amountChildFirstFake--;
                    }
                }
                else
                {
                    ReturnToPool();
                }
            }
        }
    }
}