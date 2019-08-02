public interface IEntityResourceProvider
{
    float GetCurrent();
    float GetMax();
    float GetPercentage();

    void Give(float amount);
    void GivePercentage(float percentage);
    bool Spend(float amount);
    bool SpendPercentage(float percentage);
    void Update();
}