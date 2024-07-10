# Transforms
Calculates 3D transformations for objects within a hierarchy.

### Dependencies
* [Simulation](https://github.com/game-simulations/simulation)

### Behaviour
1. All behaviour occurs when the `TransformUpdate` event is invoked
2. Entities with an `IsTransform` component will also have a `LocalToWorld` component built,
using data on the entity from these components:
* `Position`
* `Rotation`
* `EulerAngles`
* `Scale`

3. When an entity has both a `Rotation` and a `EulerAngles` component, the rotation will always
be preferred.

### Example
```cs
EntityID parentObj = world.CreateEntity();
world.AddComponent(parentObj, new IsTransform());
world.AddComponent(parentObj, new Scale(2, 2, 2));

EntityID obj = world.CreateEntity(parentObj);
world.AddComponent(obj, new IsTransform());
world.AddComponent(obj, new Position(0, 5, 0));

world.Submit(new TransformUpdate());
world.Poll();

LocalToWorld ltw = world.GetComponent<LocalToWorld>(obj));
Vector3 worldPosition = ltw.Position;
Matrix4x4 matrix = ltw.value;
```
