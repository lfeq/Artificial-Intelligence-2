/// <summary>
/// Interface representing an entity's actions related to searching for and moving towards water.
/// </summary>
public interface ILookForWater {

    /// <summary>
    /// Method to search for water.
    /// </summary>
    void lookForWater();

    /// <summary>
    /// Method to move towards the location of water.
    /// </summary>
    void moveTowardsWater();
}