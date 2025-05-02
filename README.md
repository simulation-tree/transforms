# Transforms

For representing a hierarchy of objects in 3D space.

### Behaviour

Entities with the `IsTransform` tag will have a `LocalToWorld` component calculated
using data from these components, relative to the parent:
* `Position`
* `Rotation`
* `Scale`
* `Anchor`
* `Pivot`

### Anchoring to corners

The `Anchor` component is able to describe both relative and absolute values. The example
below makes the child transform have a 10 pixel border inside the parent:
```cs
Transform parent = new(world);
parent.LocalScale = new(100, 100, 0);
parent.LocalPosition = new(50, 50, 0);

Transform child = new(world);
child.Anchor = new(10, 10, 0, 10, 10, 0, Anchor.Relativeness.X | Anchor.Relativeness.Y);

//after simulating, child will be at position (60, 60) with size (80, 80)
```

### Reading world state

Accessing world position, rotation or scale is can be done through properties of a transform
entity:
```cs
Transform a = ...
Vector3 position = a.WorldPosition;
Quaternion rotation = a.WorldRotation;
Vector3 scale = a.WorldScale;
```

And optionally, can be accessed directly through components. However, accessing
the world rotation is not possible through the `LocalToWorld` component and must
be done through `WorldRotation`:
```cs
uint a = ...
LocalToWorld ltw = world.GetComponent<LocalToWorld>(a);
Vector3 position = ltw.Position;
Vector3 scale = ltw.Scale;
Quaternion rotation = world.GetComponent<WorldRotation>(a).value;
```