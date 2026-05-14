using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class Entity : MonoBehaviour
{
    [SerializeField] private List<Parameter> _parameters;

    public event Action<Parameter> ParameterValueChange;

    public bool AddToParameter(ParameterType type, int value)
    {
        var parameter = _parameters.FirstOrDefault(p => p.Type.Equals(type));
        if (parameter == null)
            return false;

        //if (parameter.Value >= parameter.Max)
        //    return false;
       
        parameter.Value += value;
        parameter.Value = Mathf.Clamp(parameter.Value, parameter.Min, parameter.Max);
        ParameterValueChange?.Invoke(parameter);
        return true;
    }

    public int GetParameterValue(ParameterType type)
    {
        var parameter = _parameters.FirstOrDefault(parameter => parameter.Type.Equals(type));
        if (parameter == null) 
            return 0;
        else 
            return parameter.Value;
    }

    public Parameter GetParameter(ParameterType type)
    {
        var parameter = _parameters.FirstOrDefault(parameter => parameter.Type.Equals(type));
        return parameter;
    }
}

[Serializable]
public class Parameter
{
    public ParameterType Type;
    public int Value;
    public int Min;
    public int Max;
}

public enum ParameterType
{
    None = 0,
    Health = 1,
    Gold = 2    
}