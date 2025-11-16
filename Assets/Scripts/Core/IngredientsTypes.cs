namespace Core
{
    public enum IngredientType
    {
        None,
        Organic,
        Base,
        NonOrganic,
        Potion,
        Distilled,
        Cut,
        Dust
    }

    public enum OrganicType
    {
        Root,
        Eye,
        Bone
    }

    public enum BaseType
    {
        Alcohol,
        Water,
        Oil
    }

    public enum NonOrganicType
    {
        Gold,
        Diamond,
        Ruby
    }

    public enum PotionType
    {
        Speed,
        Haste,
        Love,
        Happiness,
        Goo
    }

    public enum DustType
    {
        Moon,
        Crest,
        Noble,
        Ash
    }

    public enum CutType
    {
        OrganicMix,
        GemMix,
        UniversalMix,
        Mush
    }

    public enum DistilledType
    {
        Water,
        Alcohol,
        Oil
    }

    public enum ProcessorType
    {
        None,
        Distiller,
        CuttingBoard,
        Mortar,
        Cauldron,
        DistillerOutput,
        CuttingOutput,
        MortarOutput,
        CauldronOutput
    }
}