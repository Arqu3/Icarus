public interface IEntityResourceProvider
{
    float Current { get; }
    float Max { get; }
    float Percentage { get; }

    void Give(float amount);
    void GivePercentage(float percentage);
    bool Spend(float amount);
    bool SpendPercentage(float percentage);
    void Update();
}