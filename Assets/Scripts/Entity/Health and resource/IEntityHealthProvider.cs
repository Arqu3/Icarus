public interface IEntityHealthProvider
{
    int GetCurrent();
    int GetMax();
    float GetPercentage();

    void Give(int amount);
    void GivePercentage(float percentage);
    void Remove(int amount);
    void RemovePercentage(float percentage);
}