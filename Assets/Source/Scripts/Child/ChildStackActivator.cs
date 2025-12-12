public class ChildStackActivator
{
    private readonly StackableItem m_stack;

    public ChildStackActivator(StackableItem stack)
    {
        m_stack = stack;
    }

    public void ActivateStackable(bool isActivate)
    {
        m_stack.IsStackableItemActivate = isActivate;
    }
}