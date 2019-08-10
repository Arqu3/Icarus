public interface IActionProvider
{
    bool HasTarget { get; }
    ICombatEntity Target { get; }
    void OverrideTarget(ICombatEntity target);

    bool IsInRange { get; }

    IStatProvider CreateBaseStatProvider(BaseEntity.StatMultipliers mod, out int startHealth);

    void Update();
}