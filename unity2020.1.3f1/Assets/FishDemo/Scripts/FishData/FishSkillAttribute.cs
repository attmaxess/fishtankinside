using System;
using UnityEngine;
/// <summary>
/// 
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class FishSkillAttribute : PropertyAttribute
{
    public bool editable = false;
}