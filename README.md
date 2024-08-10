# Transforms
Calculates 3D transformations for objects within a hierarchy.

### Dependencies
* [Simulation](https://github.com/game-simulations/simulation)

### Behaviour
Entities with an `IsTransform` component will have a `LocalToWorld` component built
using data from these components (relative to the parent entity), whenever the `TransformUpdate` event
is polled:
* `Position`
* `Rotation`
* `Scale`

### Example
```cs
eint parentObj = world.CreateEntity();
world.AddComponent(parentObj, new IsTransform());
world.AddComponent(parentObj, new Scale(2, 2, 2));

eint obj = world.CreateEntity(parentObj);
world.AddComponent(obj, new IsTransform());
world.AddComponent(obj, new Position(0, 5, 0));

world.Submit(new TransformUpdate());
world.Poll();

LocalToWorld ltw = world.GetComponent<LocalToWorld>(obj));
Vector3 worldPosition = ltw.Position;
Matrix4x4 matrix = ltw.value;
```
