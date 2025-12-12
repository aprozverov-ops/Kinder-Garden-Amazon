using StateMachine;

public class HelperStackControllerState : State
{
    private readonly HelperStack m_helperStack;
    private Prop m_prop;
    private Child m_child;

    public Child Child => m_child;
    public Prop CurrentProp => m_prop;
    public bool IsReadyToLeave;

    public HelperStackControllerState(HelperStack helperStack)
    {
        m_helperStack = helperStack;
    }

    public void SetProp(Prop prop)
    {
        m_prop = prop;
    }

    public void SetChild(Child child)
    {
        m_child = child;
    }

    public override void OnStateEnter()
    {
        if (m_prop != null)
        {
            m_helperStack.Attach(m_prop);
        }

        if (m_child != null)
        {
            m_helperStack.Attach(m_child.gameObject.GetComponent<StackableItem>());
        }

        IsReadyToLeave = true;
    }


    public override void OnStateExit()
    {
        IsReadyToLeave = false;
    }
}