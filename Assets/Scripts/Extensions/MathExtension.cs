using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathExtension
{
    /// <summary>
    /// Check if value is between bound1 and bound2 (bound1 and bound2 can be unordered)
    /// </summary>
    public static bool IsBetween(this float value, float bound1, float bound2)
    {
        float min = Mathf.Min(bound1, bound2);
        float max = Mathf.Max(bound1, bound2);
        return min <= value && value <= max;
    }
}
