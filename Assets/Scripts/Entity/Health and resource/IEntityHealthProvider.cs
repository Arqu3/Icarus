public interface IEntityHealthProvider
{
    int Current { get; }
    int Max { get; }
    float Percentage { get; }

    void Give(int amount);
    void Remove(int amount);
    void RemovePercentage(float percentage);
}