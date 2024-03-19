# Ecosystem Simulation Project

## Introduction
This project simulates an ecosystem inhabited by four different animals: rabbits, foxes, deer, and bears. These animals interact with each other, mimicking real-life behaviors, such as foxes hunting rabbits and rabbits hiding from predators. Each animal has variables like hunger, thirst, age, etc., which influence their decision-making.

## World Generation
The world is procedurally generated, meaning it's different each time. It consists of three types of terrain: water, grass, and land, each with distinct colors and functionalities. For example, animals cannot walk on water tiles but can drink from them.

## Specifications

### Scenario
The scenario is created using a 2D cellular automaton. A random map is generated with tiles marked true or false. After several iterations following a specific rule, the final scenario is formed where true tiles are walkable, and false tiles represent water.
```csharp
private void GenerateRandomMap() {
    m_map1 = new bool[gridWidth, gridHeight];
    for (int y = 0; y < gridHeight; y++) {
        for (int x = 0; x < gridWidth; x++) {
            m_map1[y, x] = Random.value < cubeProbability; // true = grass, false = water
        }
    }
}
```

### Agents

#### Base Agent
The `BaseAgent` class stores characteristics like maximum speed, vision range, gender, etc., used to control agent behavior.

#### Rabbit
Controlled by `RabbitAgent`, rabbits search for bushes within their vision range to eat when hungry.

#### Fox
Controlled by `FoxAgent`, foxes hunt for rabbits when hungry, moving towards them if detected within their vision range.

#### Deer
Controlled by `DeerAgent`, deer search for bushes to eat when hungry, similar to rabbits.

#### Bear
Controlled by `BearAgent`, bears hunt for rabbits or deer when hungry, moving towards them if detected within their vision range.

### Genetic Algorithm
When two animals mate, the offspring inherits characteristics from its parents, managed by `GeneticsManager`. Traits are randomly selected from either parent, with a chance of mutation. This can lead to species evolution based on environmental pressures.

```csharp
// Set son maxSpeed
sonAgent.slowingRadius = Random.value < 0.5f ? t_father.slowingRadius : t_mother.slowingRadius;
if (Random.value < mutationProbability) {
    sonAgent.slowingRadius = Random.value < 0.5f ? t_father.slowingRadius * Random.Range(0.1f, 2f) : t_mother.slowingRadius * Random.Range(0.1f, 2f);
}
// Set son eyeRadius
sonAgent.eyeRadius = Random.value < 0.5f ? t_father.eyeRadius : t_mother.eyeRadius;
if (Random.value < mutationProbability) {
    sonAgent.eyeRadius = Random.value < 0.5f ? t_father.eyeRadius * Random.Range(0.1f, 2f) : t_mother.eyeRadius * Random.Range(0.1f, 2f);
}
```

## Future Improvements
This project was made for my Artificial Intelligence class, so I'm not sure whether I'm coming back to improve it, but here are some changes I could think of to improve the experience:

- Wander steering behavior could be improved with a small pause after moving for some time and a small cooldown so it doesn't get calculated every frame.
- Use Unity DOTS for performance improvements.
- Add sounds and particle effects for actions like eating and drinking.
- Show more animal `BaseAgent` stats in the UI.
- Save an average of the animals `BaseAgent` stats and at the end show a graphic of how they changed.

## Credits
Assets used in the project

- https://assetstore.unity.com/packages/3d/low-poly-tree-62946
- https://assetstore.unity.com/packages/3d/characters/animals/5-animated-voxel-animals-145754
- https://free-game-assets.itch.io/free-wild-animal-3d-low-poly-models
- https://kenney.nl/assets/ui-pack
