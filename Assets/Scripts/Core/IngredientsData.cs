using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Core
{
    [Serializable]
    public class OrganicData : IngredientData<OrganicType>
    {
    }
        
    [Serializable]
    public class NonOrganicData : IngredientData<NonOrganicType>
    {
    }
        
    [Serializable]
    public class BaseData : IngredientData<BaseType>
    {
    }
        
    [Serializable]
    public class PotionData : IngredientData<PotionType>
    {
    }
        
    [Serializable]
    public class DistilledData : IngredientData<DistilledType>
    {
    }
        
    [Serializable]
    public class CutData : IngredientData<CutType>
    {
    }
        
    [Serializable]
    public class DustData : IngredientData<DustType>
    {
    }
        
    [Serializable]
    public class ProcessorData : IngredientData<ProcessorType>
    {
        [field: SerializeField] public string Text { get; set; }
    }

    [Serializable]
    public class IngredientData<TType> : IIngredientData where TType : Enum
    {
        [field: SerializeField] public Sprite Icon { get; set; }
        [field: SerializeField] public TType Type { get; set; }
        [field: SerializeField] public GameObject Model { get; set; }
            
        public string Name => Regex.Replace(Type.ToString(), "(?<!^)([A-Z])", " $1");
    }

    public interface IIngredientData
    {
        Sprite Icon { get; }
        string Name { get; }
    }
}