public interface IActionProvider
{
    bool HasTarget { get; }
    ICombatEntity Target { get; }
    void OverrideTarget(ICombatEntity target);

    bool IsInRange { get; }

    BaseStatProvider CreateBaseStatProvider();

    void Update();
}