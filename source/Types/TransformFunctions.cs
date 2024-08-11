using Simulation;
using System;
using System.Numerics;
using Transforms;
using Transforms.Components;

public static class TransformFunctions
{
    public static Transform BecomeTransform<T>(this T entity) where T : IEntity
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

    public static ref Vector3 GetPositionRef<T>(this T entity) where T : IPosition
    {
        ref Position position = ref entity.TryGetComponentRef<T, Position>(out bool contains);
        if (!contains)
        {
            position = ref entity.AddComponentRef<T, Position>();
        }

        return ref position.value;
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

    public static ref Quaternion GetRotationRef<T>(this T entity) where T : IRotation
    {
        ref Rotation rotation = ref entity.TryGetComponentRef<T, Rotation>(out bool contains);
        if (!contains)
        {
            rotation = ref entity.AddComponentRef<T, Rotation>();
            rotation = Rotation.Default;
        }

        return ref rotation.value;
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

    public static void SetEulerRotation<T>(this T entity, float x, float y, float z) where T : IRotation
    {
        float cy = (float)Math.Cos(z * 0.5);
        float sy = (float)Math.Sin(z * 0.5);
        float cp = (float)Math.Cos(y * 0.5);
        float sp = (float)Math.Sin(y * 0.5);
        float cr = (float)Math.Cos(x * 0.5);
        float sr = (float)Math.Sin(x * 0.5);
        Quaternion rotation = new Quaternion
        {
            W = cr * cp * cy + sr * sp * sy,
            X = sr * cp * cy - cr * sp * sy,
            Y = cr * sp * cy + sr * cp * sy,
            Z = cr * cp * sy - sr * sp * cy
        };

        SetRotation(entity, rotation);
    }

    public static void SetEulerRotation<T>(this T entity, Vector3 eulerAngles) where T : IRotation
    {
        SetEulerRotation(entity, eulerAngles.X, eulerAngles.Y, eulerAngles.Z);
    }

    /// <summary>
    /// Retrieves the <see cref="Scale"/> value.
    /// </summary>
    public static Vector3 GetScale<T>(this T entity) where T : IScale
    {
        return entity.GetComponent(new Scale()).value;
    }

    public static ref Vector3 GetScaleRef<T>(this T entity) where T : IScale
    {
        ref Scale scale = ref entity.TryGetComponentRef<T, Scale>(out bool contains);
        if (!contains)
        {
            scale = ref entity.AddComponentRef<T, Scale>();
            scale = Scale.Default;
        }

        return ref scale.value;
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