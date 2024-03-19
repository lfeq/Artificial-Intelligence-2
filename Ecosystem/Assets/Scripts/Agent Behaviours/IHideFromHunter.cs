/// <summary>
/// Interface representing an entity's ability to hide from a hunter and detect proximity to the hunter.
/// </summary>
public interface IHideFromHunter {

    /// <summary>
    /// Method to hide from the hunter.
    /// </summary>
    void hideFromHunter();

    /// <summary>
    /// Method to determine if the entity is close to the hunter.
    /// </summary>
    /// <returns>True if the entity is close to the hunter; otherwise, false.</returns>
    bool isCloseToHunter();
}