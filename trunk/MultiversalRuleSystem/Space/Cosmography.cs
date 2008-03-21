namespace AvengersUtd.MultiversalRuleSystem.Space
{

    #region Enums

    public enum PlanetClass
    {
        Planet,
        GasGiant,
        Other
    }


    public enum OrbitContent : int
    {
        Empty = 0,
        Planet = 1,
        GasGiant = 2,
        AsteroidBelt = 3,
    }

    public enum GalaxyOption
    {
        Age,
        PlanetDensity,
        Shape,
        Size,
        Generation
    }

    public enum PlanetType
    {
        Mercury,
        Venus,
        Terra,
        Mars,
        Luna,
        Jupiter
    }

    public enum PrimaryStats
    {
        Class,
        Habitability,
        Faction,
        Population
    }

    public enum SecondaryStats : int
    {
        Climate,
        Size,
        Gravity,
        Density,
        Temperature,
        AtmosphericDensity,
        Composition,
        Atmosphere
    }

    public enum PlanetSize : int
    {
        Asteroid = 0,
        Tiny = 1,
        Small = 2,
        Medium = 3,
        Large = 4,
        Huge = 5,
        Immense = 6,
        SubJovian = 7,
        Jovian = 8,
        SuperJovian = 9
    }

    public enum Gravity : int
    {
        None = 0,
        UltraLow = 1,
        VeryLow = 2,
        Low = 3,
        BelowAverage = 4,
        Average = 5,
        AboveAverage = 6,
        High = 7,
        VeryHigh = 8,
        UltraHigh = 9
    }

    public enum Density : int
    {
        GasGiant = 0,
        IcyCore = 1,
        LargeIronCore = 2
    }

    public enum Temperature : int
    {
        ZeroKelvin = 0,
        Frozen = 1,
        Icy = 2,
        VeryCold = 3,
        Cold = 4,
        Temperate = 5,
        Tropical = 6,
        VeryHot = 7,
        Extreme = 8,
        Hellish = 9,
    }

    public enum AtmosphericDensity : int
    {
        None = 0,
        Traces = 1,
        VeryThin = 2,
        Thin = 3,
        Tenuous = 4,
        Standard = 5,
        Thick = 6,
        Dense = 7,
        VeryDense = 8,
        Superdense = 9
    }

    public enum Composition : int
    {
        Vacuum,
        NitrogenOxygen,
        CarbonDioxide,
        Helium,
        Hydrogen,
    }

    public enum Climate : int
    {
        Unknown,
        Hadean,
        Ice,
        Glacial,
        Ammonia,
        Volcanic,
        Barren,
        Tundra,
        Terran,
        Ocean,
        Arid,
        Desert,
        Cataclismic,
        Radiated,
        GasGiant
    }

    public enum StarType : int
    {
        Unknown = 0,
        BlueDwarf = 1,
        YellowDwarf = 2,
        OrangeDwarf = 3,
        RedDwarf = 4,
        BrownDwarf = 5,
        RedGiant = 6,
        WhiteDwarf = 7,
        NeutronStar = 8,
        BlackHole = 9,
    }

    #endregion
}