/// <summary>
/// Interface representing an entity's actions related to searching for a mate and interacting with potential mates.
/// </summary>
public interface ILookForMate {

    /// <summary>
    /// Method to search for a mate.
    /// </summary>
    void lookForMate();

    /// <summary>
    /// Method to move towards the location of a potential mate.
    /// </summary>
    void moveTorwardsMate();

    /// <summary>
    /// Method to wait for a mate to arrive or respond.
    /// </summary>
    void waitForMate();
}