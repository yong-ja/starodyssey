#region Using directives

using System;
using System.Xml.Serialization;

#endregion

namespace AvengersUtd.MultiversalRuleSystem.Space.GalaxyGeneration
{
    public class StarChances
    {
        double[] starChances;

        #region Properties

        public double BlueDwarf
        {
            get { return this[StarColor.Blue]; }
            set { this[StarColor.Blue] = value; }
        }

        public double OrangeDwarf
        {
            get { return this[StarColor.Orange]; }
            set { this[StarColor.Orange] = value; }
        }

        public double YellowDwarf
        {
            get { return this[StarColor.Yellow]; }
            set { this[StarColor.Yellow] = value; }
        }

        public double RedDwarf
        {
            get { return this[StarColor.Red]; }
            set { this[StarColor.Red] = value; }
        }

        #endregion

        public StarChances()
        {
            starChances = new double[4];
        }

        public StarChances(
            double blueDwarf,
            double yellowDwarf,
            double orangeDwarf,
            double redDwarf
            ) : this()
        {
            this[StarColor.Blue] = blueDwarf;
            this[StarColor.Yellow] = yellowDwarf;
            this[StarColor.Orange] = orangeDwarf;
            this[StarColor.Red] = redDwarf;
        }

        public double this[StarColor type]
        {
            get { return starChances[(int) type - 1]; }
            set { starChances[(int) type - 1] = value; }
        }

        public bool Check()
        {
            double total = 0;
            for (int i = 0; i < starChances.Length; i++)
                total += starChances[i];

            if (total == 1.0)
                return true;
            else return false;
        }
    }

    public class PlanetChances
    {
        int number;
        double[] planetChances;

        #region Properties

        [XmlAttribute]
        public int Number
        {
            get { return number; }
            set { number = value; }
        }

        public double Tiny
        {
            get { return this[PlanetSize.Tiny]; }
            set { this[PlanetSize.Tiny] = value; }
        }

        public double Small
        {
            get { return this[PlanetSize.Small]; }
            set { this[PlanetSize.Small] = value; }
        }

        public double Medium
        {
            get { return this[PlanetSize.Medium]; }
            set { this[PlanetSize.Medium] = value; }
        }

        public double Large
        {
            get { return this[PlanetSize.Large]; }
            set { this[PlanetSize.Large] = value; }
        }

        public double Huge
        {
            get { return this[PlanetSize.Huge]; }
            set { this[PlanetSize.Huge] = value; }
        }

        public double Immense
        {
            get { return this[PlanetSize.Immense]; }
            set { this[PlanetSize.Immense] = value; }
        }

        #endregion

        public PlanetChances()
        {
            number = 0;
            planetChances = new double[6];
        }

        public PlanetChances(
            double tiny,
            double small,
            double medium,
            double large,
            double huge,
            double immense
            ) : this()
        {
            this[PlanetSize.Tiny] = tiny;
            this[PlanetSize.Small] = small;
            this[PlanetSize.Medium] = medium;
            this[PlanetSize.Large] = large;
            this[PlanetSize.Huge] = huge;
            this[PlanetSize.Immense] = immense;
        }

        public double this[PlanetSize size]
        {
            get { return planetChances[(int) size - 1]; }
            set { planetChances[(int) size - 1] = value; }
        }

        public bool Check()
        {
            double total = 0;
            for (int i = 0; i < planetChances.Length; i++)
                total += planetChances[i];

            if (1 - total < 1e-3f)
                return true;
            else return false;
        }
    }

    public class GasGiantChances
    {
        double[] gasGiantChances;

        #region Properties

        public double SubJovian
        {
            get { return this[PlanetSize.SubJovian]; }
            set { this[PlanetSize.SubJovian] = value; }
        }

        public double Jovian
        {
            get { return this[PlanetSize.Jovian]; }
            set { this[PlanetSize.Jovian] = value; }
        }

        public double SuperJovian
        {
            get { return this[PlanetSize.SuperJovian]; }
            set { this[PlanetSize.SuperJovian] = value; }
        }

        #endregion

        public GasGiantChances()
        {
            gasGiantChances = new double[3];
        }

        public GasGiantChances(double subJovian, double jovian, double superJovian) : this()
        {
            this[PlanetSize.SubJovian] = subJovian;
            this[PlanetSize.Jovian] = jovian;
            this[PlanetSize.SuperJovian] = superJovian;
        }

        public double this[PlanetSize size]
        {
            get
            {
                if (size < PlanetSize.SubJovian)
                    throw new ArgumentOutOfRangeException("You must choose a Gas Giant size");

                return gasGiantChances[(int) size - 7];
            }

            set
            {
                if (size < PlanetSize.SubJovian)
                    throw new ArgumentOutOfRangeException("You must choose a Gas Giant size");

                gasGiantChances[(int) size - 7] = value;
            }
        }

        public bool Check()
        {
            double total = 0;
            for (int i = 0; i < gasGiantChances.Length; i++)
                total += gasGiantChances[i];

            if (total == 1.0)
                return true;
            else return false;
        }
    }

    public class WorldTypeChances
    {
        Zone zone;
        double[] worldTypeChances;

        #region Properties

        [XmlAttribute]
        public Zone Type
        {
            get { return zone; }
            set { zone = value; }
        }

        public double IcyCore
        {
            get { return this[WorldType.IceWorld]; }
            set { this[WorldType.IceWorld] = value; }
        }

        public double IronCore
        {
            get { return this[WorldType.Planet]; }
            set { this[WorldType.Planet] = value; }
        }

        public double GasGiant
        {
            get { return this[WorldType.GasGiant]; }
            set { this[WorldType.GasGiant] = value; }
        }

        public double AsteroidBelt
        {
            get { return this[WorldType.AsteroidBelt]; }
            set { this[WorldType.AsteroidBelt] = value; }
        }

        #endregion

        public WorldTypeChances()
        {
            worldTypeChances = new double[4];
            zone = Zone.Unknown;
        }

        public WorldTypeChances(double icyCore, double ironCore, double gasGiant, double asteroidBelt) : this()
        {
            this[WorldType.IceWorld] = icyCore;
            this[WorldType.Planet] = ironCore;
            this[WorldType.GasGiant] = gasGiant;
            this[WorldType.AsteroidBelt] = asteroidBelt;
        }

        public double this[WorldType worldType]
        {
            get { return worldTypeChances[(int) worldType]; }
            set { worldTypeChances[(int) worldType] = value; }
        }

        public bool Check()
        {
            double total = 0;
            for (int i = 0; i < worldTypeChances.Length; i++)
                total += worldTypeChances[i];

            if (total == 1.0)
                return true;
            else return false;
        }
    }

    public class EmptyOrbitChances
    {
        StarColor type;
        double[] emptyOrbitChances;

        #region Properties

        [XmlAttribute]
        public StarColor Type
        {
            get { return type; }
            set { type = value; }
        }

        public double Planetary
        {
            get { return this[Zone.Planetary]; }
            set { this[Zone.Planetary] = value; }
        }

        public double GasGiant
        {
            get { return this[Zone.GasGiant]; }
            set { this[Zone.GasGiant] = value; }
        }

        public double Frozen
        {
            get { return this[Zone.Frozen]; }
            set { this[Zone.Frozen] = value; }
        }

        #endregion

        public EmptyOrbitChances()
        {
            emptyOrbitChances = new double[3];
            type = StarColor.None;
        }

        public EmptyOrbitChances(double planetary, double gasGiant, double frozenZone) : this()
        {
            this[Zone.Planetary] = planetary;
            this[Zone.GasGiant] = gasGiant;
            this[Zone.Frozen] = frozenZone;
        }

        public double this[Zone zone]
        {
            get { return emptyOrbitChances[(int) zone]; }
            set { emptyOrbitChances[(int) zone] = value; }
        }
    }


    [XmlRoot(Namespace = "http://www.avengersutd.com")]
    public class GalaxyOptions
    {
        string id;
        int sectorWidth;
        int sectorHeight;
        int starCount;
        GalaxyAge age;
        GalaxyShape shape;
        GalaxySize size;
        GalaxyPlanetDensity planetDensity;

        StarChances starChances;
        //EmptyOrbitChances[] emptyOrbitChances;
        //WorldTypeChances[] worldTypeChances;
        //PlanetChances[] planetChances;
        //GasGiantChances gasGiantChance;

        #region Properties

        [XmlAttribute]
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        #region Galaxy Features

        public GalaxyAge Age
        {
            get { return age; }
            set { age = value; }
        }

        public GalaxyShape Shape
        {
            get { return shape; }
            set { shape = value; }
        }

        public GalaxySize Size
        {
            get { return size; }
            set { size = value; }
        }

        public int SectorWidth
        {
            get { return sectorWidth; }
            set { sectorWidth = value; }
        }

        public int SectorHeight
        {
            get { return sectorHeight; }
            set { sectorHeight = value; }
        }

        public int StarCount
        {
            get { return starCount; }
            set { starCount = value; }
        }

        public GalaxyPlanetDensity PlanetDensity
        {
            get { return planetDensity; }
            set { planetDensity = value; }
        }

        #endregion

        public StarChances StarChances
        {
            get { return starChances; }
            set { starChances = value; }
        }

        /*
        [XmlArray(ElementName="EmptyOrbitChances")]
        [XmlArrayItem(ElementName="ChancesPerStar")]
        public EmptyOrbitChances[] EmptyOrbitChances
        {
            get { return emptyOrbitChances; }
            set
            {
                emptyOrbitChances = value;
                for (int i = 0; i < emptyOrbitChances.Length; i++)
                    emptyOrbitChances[i].Type = (StarColor) (i + 1);
            }
        }

        [XmlArrayItem(ElementName = "ChancesPerZone")]
        public WorldTypeChances[] WorldTypeChances
        {
            get { return worldTypeChances; }
            set
            {
                worldTypeChances = value;
                for (int i = 0; i < worldTypeChances.Length; i++)
                    worldTypeChances[i].Type = (Zone) i;
            }
        }

        [XmlArray(ElementName="PlanetarySizesChances")]
        [XmlArrayItem(ElementName = "ChancesPerOrbit")]
        public PlanetChances[] PlanetChances
        {
            get { return planetChances; }
            set
            {
                planetChances = value;

                for (int i = 0; i < planetChances.Length; i++)
                    planetChances[i].Number = i + 1;
            }
        }

        public GasGiantChances GasGiantChance
        {
            get { return gasGiantChance; }
            set { gasGiantChance = value; }
        }
         * */

        #endregion

        public GalaxyOptions()
        {
            starChances = new StarChances();
            //emptyOrbitChances = new EmptyOrbitChances[8];
            //worldTypeChances = new WorldTypeChances[3];
            //planetChances = new PlanetChances[12];
            //gasGiantChance = new GasGiantChances();
        }


        public GalaxyOptions(string id, int sectorWidth, int sectorHeight, int starCount,
                             GalaxyAge age, GalaxyShape shape, GalaxySize size, GalaxyPlanetDensity planetDensity,
                             StarChances starChances
                             /*EmptyOrbitChances[] emptyOrbitChances,
                             WorldTypeChances[] zoneChances,
                             PlanetChances[] planetChances,
                             GasGiantChances gasGiantChance*/)
        {
            this.id = id;
            //this.emptyOrbitChances = emptyOrbitChances;
            //for (int i = 0; i < this.emptyOrbitChances.Length; i++)
            //    this.emptyOrbitChances[i].Type = (StarColor) (i + 1);

            //this.planetChances = planetChances;
            //for (int i = 0; i < this.planetChances.Length; i++)
            //    this.planetChances[i].Number = i + 1;

            //worldTypeChances = zoneChances;
            //for (int i = 0; i < worldTypeChances.Length; i++)
            //    worldTypeChances[i].Type = (Zone) i;

            this.sectorWidth = sectorWidth;
            this.sectorHeight = sectorHeight;
            this.starCount = starCount;
            this.age = age;
            this.shape = shape;
            this.size = size;
            this.planetDensity = planetDensity;
            this.starChances = starChances;

           // this.gasGiantChance = gasGiantChance;
        }

        public GalaxyOptions(string id,
                             int starCount, GalaxyAge age, GalaxyShape shape, GalaxySize size,
                             GalaxyPlanetDensity planetDensity,
                             StarChances starChances)
                             //EmptyOrbitChances[] emptyOrbitChances,
                             //WorldTypeChances[] zoneChances,
                             //PlanetChances[] planetChances,
                             //GasGiantChances gasGiantChance)
            : this(
                id, 0, 0, starCount, age, shape, size, planetDensity, starChances
            /*emptyOrbitChances, zoneChances,
                planetChances, gasGiantChance*/)
        {
            switch (size)
            {
                default:
                case GalaxySize.Normal:
                    sectorWidth = 2;
                    sectorHeight = 2;
                    break;
            }
        }
    }
}