/// <summary>
/// Interface representing an entity's actions related to searching for and moving towards food.
/// </summary>
public interface ILookForFood {

    /// <summary>
    /// Method to search for food.
    /// </summary>
    void lookForFood();

    /// <summary>
    /// Method to move towards the location of food.
    /// </summary>
    void moveTowardFood();
}