/// <summary>
/// Interface representing an entity's actions related to reducing vital statistics and checking vital signs.
/// </summary>
public interface IReduceVitals {

    /// <summary>
    /// Method to reduce vital statistics, such as health or thirst.
    /// </summary>
    void reduceVitals();

    /// <summary>
    /// Method to check vital signs.
    /// </summary>
    void checkVitals();
}