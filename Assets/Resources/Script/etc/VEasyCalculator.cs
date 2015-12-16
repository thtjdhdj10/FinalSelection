using UnityEngine;
using System.Collections;

public class VEasyCalculator {

    public static bool CheckMyRect(Unit.LogicalPosition me, Unit.LogicalPosition target, float r)
    {
        if (r < 0f)
            return false;

        if(target.x > me.x + r ||
            target.x < me.x - r ||
            
            target.y > me.y + r ||
            target.y < me.y - r)
        {
            return false;
        }

        return true;
    }

    public static bool CheckMyCircle(Unit.LogicalPosition me, Unit.LogicalPosition target, float r)
    {
        if (r < 0f)
            return false;
        
        float deltaDistanceSquare = CalcDistanceSquare2D(me, target);
        
        if (deltaDistanceSquare > r*r)
        {
            return false;
        }

        return true;
    }

    public static float CalcDistanceSquare2D(Unit.LogicalPosition me, Unit.LogicalPosition target)
    {
        float deltaX = target.x - me.x;
        float deltaY = target.y - me.y;

        return deltaX * deltaX + deltaY * deltaY;
    }

    public static float CalcDistance2D(Unit.LogicalPosition me, Unit.LogicalPosition target)
    {
        float deltaX = target.x - me.x;
        float deltaY = target.y - me.y;

        float deltaSquare = deltaX * deltaX + deltaY * deltaY;

        return Mathf.Sqrt(deltaSquare);
    }

    public static T CopyComponent<T>(T original, GameObject destination)
        where T : Component
    {
        System.Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy as T;
    }

    public static Unit CopyComponent(Unit original)
    {
        System.Type type = original.GetType();
        Unit copy = new Unit();
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy;
    }
}
