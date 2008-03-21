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
        //ZeroKelvin = 0,
        Frozen = 1,
        //Icy = 2,
        //VeryCold = 3,
        Cold = 4,
        Temperate = 5,
        //Tropical = 6,
        VeryHot = 7,
        Extreme = 8,
        //Hellish = 9,
    }

    public enum AtmosphericDensity : int
    {
        None = 0,
        Traces = 1,
        //VeryThin = 2,
        Thin = 3,
        //Tenuous = 4,
        Standard = 5,
        //Thick = 6,
        Dense = 7,
        //VeryDense = 8,
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
        Other = 0,
        HyperGiant,
        SuperGiant,
        Giant,
        SubGiant,
        Dwarf,
        SubDwarf
    }

    public enum SpectralType : int
    {
        O = 0,
        B = 1,
        A = 2,
        F = 3,
        G = 4,
        K = 5,
        M = 6,
        L = 7,
        D = 8,
        NS= 9,
        BH = 10
    }

    public enum StarColor : int
    {
        None,
        Blue,
        Yellow,
        Orange,
        Red,
        Brown,
        White,
        Black

    }

    public enum LuminosityClass : int
    {
        None =0,
        O = 1,
        Ia =2,
        Ib =3,
        II = 4,
        III =5,
        IV = 6,
        V = 7,
        VI = 8,
        VII = 9
    }

    public struct SpectralClass
    {
        SpectralType spectralType;
        int subType;
        LuminosityClass luminosityClass;

        StarColor starColor;
        StarType starType;

        static StarColor GetStarColor(int type)
        {
            StarColor starColor = StarColor.None;

            switch (type)
            {
                // O,B,A
                case 0:
                case 1:
                case 2:
                    starColor = StarColor.Blue;
                    break;

                // F, G
                case 3:
                case 4:
                    starColor = StarColor.Yellow;
                    break;

                // K
                case 5:
                    starColor = StarColor.Orange;
                    break;

                // M
                case 6:
                    starColor = StarColor.Red;
                    break;

                // L
                case 7:
                    starColor = StarColor.Brown;
                    break;

                // D
                case 8:
                    starColor = StarColor.White;
                    break;

                // NS & BH:
                default:
                    starColor = StarColor.None;
                    break;
            }

            return starColor;
        }
        static StarType GetStarType(LuminosityClass luminosityClass)
        {
            switch (luminosityClass)
            {
                case LuminosityClass.O:
                    return StarType.HyperGiant;
                case LuminosityClass.Ia:
                case LuminosityClass.Ib:
                    return StarType.SuperGiant;
                case LuminosityClass.II:
                case LuminosityClass.III:
                    return StarType.Giant;
                case LuminosityClass.IV:
                    return StarType.SubGiant;
                case LuminosityClass.V:
                case LuminosityClass.VI:
                case LuminosityClass.VII:
                    return StarType.Dwarf;

                default:
                    return StarType.Other;
            }


        }

        public string CommonType
        {
            get
            {
                if (starType != StarType.Other)
                    return string.Format("{0} {1}", starColor.ToString(), starType.ToString());
                else if (spectralType == SpectralType.NS)
                    return "Neutron Star";
                else
                    return "Black Hole";
            }
        }

        public SpectralClass(SpectralType spectralType, int subType, LuminosityClass luminosityClass)
        {
            this.spectralType = spectralType;
            this.subType = subType;
            this.luminosityClass = luminosityClass;

            starColor = GetStarColor((int)spectralType);
            starType = GetStarType(luminosityClass);
        }

        public override string ToString()
        {
            if ((int)spectralType <= 8)
                return string.Format("{0}{1} {2}", spectralType.ToString(), subType, luminosityClass.ToString());
            else
                return spectralType.ToString();
        }

    }

    #endregion
}