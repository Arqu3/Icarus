public interface IEntityHealthProvider
{
    int GetCurrent();
    int GetMax();
    float GetPercentage();

    void Give(int amount);
    void GivePercentage(float percentage);
    DamageResult Remove(int amount, DamageType type);
    DamageResult RemovePercentage(float percentage, DamageType type);

    int GetRegenAmount();
    float GetRegenInterval();
}