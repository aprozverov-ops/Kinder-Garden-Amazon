using Kuhpik;
using StateMachine;
using UnityEngine;

public class CharacterMoveState : State
{
    private readonly Rigidbody m_rigidbody;
    private readonly CharacterConfigurations m_characterConfigurations;
    private readonly Joystick m_joystick;
    private readonly CharacterAnimationController m_characterAnimationController;

    public CharacterMoveState(Rigidbody rigidbody, CharacterConfigurations characterConfigurations, Joystick joystick,
        CharacterAnimationController
            characterAnimationController)
    {
        m_rigidbody = rigidbody;
        m_characterConfigurations = characterConfigurations;
        m_joystick = joystick;
        m_characterAnimationController = characterAnimationController;
    }

    public override void FixedTick()
    {
        if (m_joystick.Direction != Vector2.zero)
        {
            var delta = Time.deltaTime;
            Rotate(m_joystick.Direction, delta);
            Move(m_joystick.Direction, delta);
        }

        m_characterAnimationController.SetFloat(CharacterAnimationType.Speed,
            GetSpeed(Bootstrap.Instance.PlayerData.UpgadeLevel[UpgradeType.Speed]));
    }

    private float GetSpeed(int lvl)
    {
        switch (lvl)
        {
            case 1:
                return 0;
                break;
            case 2:
                return 0.3f;
                break;
            case 3:
                return 0.8f;
                break;
            case 4:
                return 0.9f;
                break;
        }

        return 1;
    }

    public override void OnStateEnter()
    {
        m_characterAnimationController.SetBool(CharacterAnimationType.Walk, true);
    }

    public override void OnStateExit()
    {
        m_characterAnimationController.SetBool(CharacterAnimationType.Walk, false);
    }

    private void Rotate(Vector2 joystickDirection, float deltaTime)
    {
        var rb = m_rigidbody;
        var charForward = rb.transform.forward;
        var direction = new Vector3(joystickDirection.x, 0, joystickDirection.y).normalized;
        var speed = m_characterConfigurations.SpeedRotate * deltaTime;

        var newDirection = Vector3.RotateTowards(charForward, direction, speed, 0.0f);
        rb.transform.rotation =
            Quaternion.Lerp(m_rigidbody.transform.rotation, Quaternion.LookRotation(newDirection), 1);
    }

    private void Move(Vector2 joystickDirection, float deltaTime)
    {
        var rb = m_rigidbody;
        var charForward = rb.transform.forward;
        var speed = m_characterConfigurations.Speed + (Bootstrap.Instance.PlayerData.UpgadeLevel[UpgradeType.Speed] *
                                                       m_characterConfigurations
                                                           .AddSpeedPerLevel);
        speed *= joystickDirection.magnitude;
        var velocity = charForward * (speed * deltaTime);
        rb.velocity = velocity;
    }
}