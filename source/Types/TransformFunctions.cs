using Simulation;
using System.Numerics;
using Transforms;
using Transforms.Components;

public static class TransformFunctions
{
    public static Transform AsTransform<T>(this T entity) where T : IEntity
    {
        if (!entity.ContainsComponent<T, IsTransform>())
        {
            entity.Become<T, Transform>();
        }

        return new Transform(entity.World, entity.Value);
    }

    /// <summary>
    /// Retrieves the <see cref="Position"/> value.
    /// </summary>
    public static Vector3 GetPosition<T>(this T entity) where T : IPosition
    {
        return entity.GetComponent(new Position()).value;
    }

    /// <summary>
    /// Assigns the <see cref="Position"/> value.
    /// </summary>
    public static void SetPosition<T>(this T entity, Vector3 value) where T : IPosition
    {
        if (!entity.ContainsComponent<T, Position>())
        {
            entity.AddComponent(new Position());
        }

        entity.GetComponentRef<T, Position>().value = value;
    }

    public static void SetPosition<T>(this T entity, float x, float y, float z) where T : IPosition
    {
        SetPosition(entity, new Vector3(x, y, z));
    }

    /// <summary>
    /// Retrieves the <see cref="Rotation"/> value.
    /// </summary>
    public static Quaternion GetRotation<T>(this T entity) where T : IRotation
    {
        return entity.GetComponent(new Rotation()).value;
    }

    /// <summary>
    /// Assigns the <see cref="Rotation"/> value.
    /// </summary>
    public static void SetRotation<T>(this T entity, Quaternion value) where T : IRotation
    {
        if (!entity.ContainsComponent<T, Rotation>())
        {
            entity.AddComponent(new Rotation());
        }

        entity.GetComponentRef<T, Rotation>().value = value;
    }

    /// <summary>
    /// Retrieves the <see cref="Scale"/> value.
    /// </summary>
    public static Vector3 GetScale<T>(this T entity) where T : IScale
    {
        return entity.GetComponent(new Scale()).value;
    }

    /// <summary>
    /// Assigns the <see cref="Scale"/> value.
    /// </summary>
    public static void SetScale<T>(this T entity, Vector3 value) where T : IScale
    {
        if (!entity.ContainsComponent<T, Scale>())
        {
            entity.AddComponent(new Scale());
        }

        entity.GetComponentRef<T, Scale>().value = value;
    }

    public static void SetScale<T>(this T entity, float x, float y, float z) where T : IScale
    {
        entity.SetScale(new Vector3(x, y, z));
    }
}