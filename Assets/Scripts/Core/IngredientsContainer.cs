using System.Collections.Generic;

namespace Core
{
    public static class IngredientsContainer
    {
        public static List<OrganicType> AvailableOrganicTypes = new();
        public static List<NonOrganicType> AvailableNonOrganicTypes = new();
        public static List<BaseType> AvailableBaseTypes = new();
        public static List<PotionType> AvailablePotionTypes = new();
        
        private static readonly List<OrganicType> DefaultAvailableOrganicTypes = new()
        {
            OrganicType.Bone,
            OrganicType.Bone,
            OrganicType.Bone,
            OrganicType.Bone,
            OrganicType.Bone,
            OrganicType.Eyeball,
            OrganicType.Eyeball,
            OrganicType.Eyeball,
            OrganicType.Eyeball,
            OrganicType.Eyeball,
            OrganicType.Mandragora,
            OrganicType.Mandragora,
            OrganicType.Mandragora,
            OrganicType.Mandragora,
            OrganicType.Mandragora
        };

        private static readonly List<NonOrganicType> DefaultAvailableNonOrganicTypes = new()
        {
            NonOrganicType.Diamond,
            NonOrganicType.Diamond,
            NonOrganicType.Diamond,
            NonOrganicType.Diamond,
            NonOrganicType.Diamond,
            NonOrganicType.Gold,
            NonOrganicType.Gold,
            NonOrganicType.Gold,
            NonOrganicType.Gold,
            NonOrganicType.Gold,
            NonOrganicType.Ruby,
            NonOrganicType.Ruby,
            NonOrganicType.Ruby,
            NonOrganicType.Ruby,
            NonOrganicType.Ruby,
        };

        private static readonly List<BaseType> DefaultAvailableBaseTypes = new()
        {
            BaseType.Alcohol,
            BaseType.Alcohol,
            BaseType.Alcohol,
            BaseType.Alcohol,
            BaseType.Alcohol,
            BaseType.Oil,
            BaseType.Oil,
            BaseType.Oil,
            BaseType.Oil,
            BaseType.Oil,
            BaseType.Water,
            BaseType.Water,
            BaseType.Water,
            BaseType.Water,
            BaseType.Water
        };
        
        private static readonly List<PotionType> DefaultAvailablePotionTypes = new();

        public static void Apply()
        {
            AvailablePotionTypes = new List<PotionType>(DefaultAvailablePotionTypes);
            AvailableBaseTypes = new List<BaseType>(DefaultAvailableBaseTypes);
            AvailableOrganicTypes = new List<OrganicType>(DefaultAvailableOrganicTypes);
            AvailableNonOrganicTypes = new List<NonOrganicType>(DefaultAvailableNonOrganicTypes);
        }
    }
}