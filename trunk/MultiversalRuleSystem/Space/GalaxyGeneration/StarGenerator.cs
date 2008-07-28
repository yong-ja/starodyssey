using System;
using AvengersUtd.MultiversalRuleSystem.Space.CelestialObjects;
using AvengersUtd.Odyssey;
using AvengersUtd.Odyssey.Utils;

namespace AvengersUtd.MultiversalRuleSystem.Space.GalaxyGeneration
{
    public class StarGenerator
    {
        const int tableLength = 80;
        readonly static double[] lZamsFit = new double[] { 6.16327601e-2, 4.3936505, -0.45406535, -0.21307969, 4.98479310e-2 };
        readonly static EnhancedRandom rnd = Dice.Rnd;

        readonly GalaxyOptions galaxyOptions;
        readonly char[] typeCharArray = new char[] {'O', 'B', 'A', 'F', 'G', 'K', 'M','L','D'};

        readonly double[] bcMainSequence;
        readonly double[] temperatureTable;
        readonly double[] bolumetricMagnitudeTable;
        readonly double[] massTable;

        readonly double[,] luminosityModifiers;

        double mass;
        double magnitude;
        double temperature;
        double luminosity;
        double radius;
        double density;
        double lifespan;
        string starName;
        double age;
        double semiMajorAxis;
        double eccentricity;
        double orbitalPeriod;

        int spectralType;
        int subType;
        StarColor starColor;
        LuminosityClass luminosityClass;

        public StarGenerator(GalaxyOptions galaxyOptions)
        {
            this.galaxyOptions = galaxyOptions;

          
            bolumetricMagnitudeTable = new double[tableLength];
            bcMainSequence = new double[tableLength];
            massTable = new double[tableLength];
            temperatureTable = new double[tableLength];

            luminosityModifiers = new double[tableLength/10,10];

            GenerateStarTables();
        }

        #region Star Table generation

        void GenerateStarTables()
        {

            #region Bolumetric Magnitude, Mass and Temperature tables
            // O0 - O9
            bolumetricMagnitudeTable[4] = -10.24;
            massTable[4] = 90;
            temperatureTable[4] = 48000;

            bolumetricMagnitudeTable[5] = -9.99;
            massTable[5] = 60;
            temperatureTable[5] = 44500;

            bolumetricMagnitudeTable[6] = -9.31;
            massTable[6] = 37;
            temperatureTable[6] = 41000;
            
            bolumetricMagnitudeTable[7] = -8.79;
            massTable[7] = 30;
            temperatureTable[7] = 38000;
            
            bolumetricMagnitudeTable[8] = -8.33;
            massTable[8] = 25;
            temperatureTable[8] = 35800;
            
            bolumetricMagnitudeTable[9] = -7.72;
            massTable[9] = 23.3;
            temperatureTable[9] = 33000;


            // B0 - B9
            bolumetricMagnitudeTable[10] = -7.04;
            massTable[10] = 17.5;
            temperatureTable[10] = 30000;

            bolumetricMagnitudeTable[11] = -5.76;
            massTable[11] = 14.2;
            temperatureTable[11] = 25400;

            bolumetricMagnitudeTable[12] = -4.64;
            massTable[12] = 10.9;
            temperatureTable[12] = 22000;

            bolumetricMagnitudeTable[13] = -3.45;
            massTable[13] = 7.6;
            temperatureTable[13] = 18700;

            bolumetricMagnitudeTable[14] = -3.00;
            massTable[14] = 6.9;
            temperatureTable[14] = 16900;

            bolumetricMagnitudeTable[15] = -2.55;
            massTable[15] = 5.9;
            temperatureTable[15] = 15400;

            bolumetricMagnitudeTable[16] = -2.00;
            massTable[16] = 5.2;
            temperatureTable[16] = 14000;

            bolumetricMagnitudeTable[17] = -1.50;
            massTable[17] = 4.5;
            temperatureTable[17] = 13000;

            bolumetricMagnitudeTable[18] = -0.89;
            massTable[18] = 3.8;
            temperatureTable[18] = 11900;

            bolumetricMagnitudeTable[19] = -0.19;
            massTable[19] = 3.35;
            temperatureTable[19] = 10500;

            // A0 - A9
            bolumetricMagnitudeTable[20] = 0.42;
            massTable[20] = 2.9;
            temperatureTable[20] = 9520;

            bolumetricMagnitudeTable[21] = 0.89;
            massTable[21] = 2.72;
            temperatureTable[21] = 9230;

            bolumetricMagnitudeTable[22] = 1.21;
            massTable[22] = 2.54;
            temperatureTable[22] = 8970;

            bolumetricMagnitudeTable[23] = 1.44;
            massTable[23] = 2.36;
            temperatureTable[23] = 8200;

            bolumetricMagnitudeTable[25] = 1.88;
            massTable[25] = 2;
            temperatureTable[25] = 8200;
            
            bolumetricMagnitudeTable[27] = 2.20;
            massTable[27] = 1.84;
            temperatureTable[27] = 7850;

            bolumetricMagnitudeTable[28] = 2.41;
            massTable[28] = 1.76;
            temperatureTable[28] = 7580;

            // F0 - F9
            bolumetricMagnitudeTable[30] = 2.72;
            massTable[30] = 1.6;
            temperatureTable[30] = 7200;

            bolumetricMagnitudeTable[32] = 3.17;
            massTable[32] = 1.52;
            temperatureTable[32] = 6890;

            bolumetricMagnitudeTable[35] = 3.49;
            massTable[35] = 1.4;
            temperatureTable[35] = 6400;

            bolumetricMagnitudeTable[38] = 3.94;
            massTable[38] = 1.19;
            temperatureTable[38] = 6200;

            // G0 - G9
            bolumetricMagnitudeTable[40] = 4.31;
            massTable[40] = 1.05;
            temperatureTable[40] = 6030;

            bolumetricMagnitudeTable[42] = 4.65;
            massTable[42] = 0.998;
            temperatureTable[42] = 5860;

            bolumetricMagnitudeTable[45] = 5.01;
            massTable[45] = 0.92;
            temperatureTable[45] = 5770;

            bolumetricMagnitudeTable[48] = 5.20;
            massTable[48] = 0.842;
            temperatureTable[48] = 5570;

            // K0 - K9
            bolumetricMagnitudeTable[50] = 5.69;
            massTable[50] = 0.79;
            temperatureTable[50] = 5250;

            bolumetricMagnitudeTable[51] = 5.83;
            massTable[51] = 0.766;
            temperatureTable[51] = 5080;

            bolumetricMagnitudeTable[52] = 6.09;
            massTable[52] = 0.742;
            temperatureTable[52] = 4900;

            bolumetricMagnitudeTable[53] = 6.21;
            massTable[53] = 0.718;
            temperatureTable[53] = 4730;

            bolumetricMagnitudeTable[54] = 6.55;
            massTable[54] = 0.694;
            temperatureTable[54] = 4590;

            bolumetricMagnitudeTable[55] = 6.81;
            massTable[55] = 0.670;
            temperatureTable[55] = 4350;

            bolumetricMagnitudeTable[57] = 7.25;
            massTable[57] = 0.606;
            temperatureTable[57] = 4060;

            // M0 - M9
            bolumetricMagnitudeTable[60] = 7.53;
            massTable[60] = 0.51;
            temperatureTable[60] = 3850;

            bolumetricMagnitudeTable[61] = 7.79;
            massTable[61] = 0.445;
            temperatureTable[61] = 3720;

            bolumetricMagnitudeTable[62] = 8.12;
            massTable[62] = 0.4;
            temperatureTable[62] = 3580;

            bolumetricMagnitudeTable[63] = 8.36;
            massTable[63] = 0.35;
            temperatureTable[63] = 3470;

            bolumetricMagnitudeTable[64] = 9.05;
            massTable[64] = 0.3;
            temperatureTable[64] = 3370;

            bolumetricMagnitudeTable[65] = 9.65;
            massTable[65] = 0.25;
            temperatureTable[65] = 3240;

            bolumetricMagnitudeTable[66] = 10.44;
            massTable[66] = 0.207;
            temperatureTable[66] = 3050;

            bolumetricMagnitudeTable[67] = 10.92;
            massTable[67] = 0.163;
            temperatureTable[67] = 2940;

            bolumetricMagnitudeTable[68] = 12.05;
            massTable[68] = 0.12;
            temperatureTable[68] = 2640;

            bolumetricMagnitudeTable[69] = 13.56;
            massTable[69] = 0.1;
            temperatureTable[69] = 2510;

            // E0 - E9 : Brown Dwarfs
            bolumetricMagnitudeTable[70] = 15.74;
            massTable[70] = 0.08;
            temperatureTable[70] = 1800;

            bolumetricMagnitudeTable[72] = 16.06;
            massTable[72] = 0.072;
            temperatureTable[72] = 1600;

            bolumetricMagnitudeTable[74] = 16.74;
            massTable[74] = 0.064;
            temperatureTable[74] = 1300;

            bolumetricMagnitudeTable[76] = 17.25;
            massTable[76] = 0.053;
            temperatureTable[76] = 1000;

            bolumetricMagnitudeTable[78] = 18;
            massTable[78] = 0.04;
            temperatureTable[78] = 800;

            massTable[79] = 0.01;

            #endregion

            #region Luminosity Modifiers
            // Ox-Bx 0% luminosity modifier.

            // Ax modifiers
            luminosityModifiers[2, 0] = -0.15;
            luminosityModifiers[2, 1] = -0.1;
            luminosityModifiers[2, 2] = -0.05;
            luminosityModifiers[2, 3] = -0.025;

            luminosityModifiers[2, 6] = 0.025;
            luminosityModifiers[2, 7] = 0.05;
            luminosityModifiers[2, 8] = 0.1;
            luminosityModifiers[2, 9] = 0.15;

            // Fx modifiers
            luminosityModifiers[3, 0] = -0.4;
            luminosityModifiers[3, 1] = -0.3;
            luminosityModifiers[3, 2] = -0.2;
            luminosityModifiers[3, 3] = -0.1;

            luminosityModifiers[3, 5] = 0.1;
            luminosityModifiers[3, 6] = 0.2;
            luminosityModifiers[3, 7] = 0.3;
            luminosityModifiers[3, 8] = 0.4;
            luminosityModifiers[3, 9] = 0.5;

            // Gx modifiers
            luminosityModifiers[4, 0] = -0.4;
            luminosityModifiers[4, 1] = -0.3;
            luminosityModifiers[4, 2] = -0.2;
            luminosityModifiers[4, 3] = -0.1;

            luminosityModifiers[4, 5] = 0.1;
            luminosityModifiers[4, 6] = 0.2;
            luminosityModifiers[4, 7] = 0.3;
            luminosityModifiers[4, 8] = 0.4;
            luminosityModifiers[4, 9] = 0.5;

            // Kxmodifiers
            luminosityModifiers[5, 0] = -0.15;
            luminosityModifiers[5, 1] = -0.1;
            luminosityModifiers[5, 2] = -0.05;

            luminosityModifiers[5, 8] = 0.05;
            luminosityModifiers[5, 9] = 0.1;

            // Mx modifiers
            luminosityModifiers[5, 0] = -0.1;

            luminosityModifiers[5, 9] = 0.1;
            #endregion

            
        }

      
        #endregion

        #region Computations
        /// <summary>
        /// Computes the bolumetric luminosity using the radius and the effective temperature.
        /// </summary>
        /// <param name="radius">The radius of the star in Km</param>
        /// <param name="temperature">The effective temperature in degrees Kelvin.</param>
        /// <returns>The bolumetric luminosity of the star, in Watts.</returns>
        public static double ComputeBolumetricLuminosity(double radius, double temperature)
        {
            return 4.0 * Math.PI * Math.Pow((radius * PhysicalConstants.MetersPerKilometer), 2) *
                   PhysicalConstants.StefanBoltzmannConstant * Math.Pow(temperature, 4);
        }

        /// <summary>
        /// Computes the luminosity using the Absolute Bolumetric Magnitude.
        /// </summary>
        /// <param name="absoluteBolumetricMagnitude">The Absolute Bolumetric Magnitude of the star.</param>
        /// <returns>The luminosity of the star, in units of Solar luminosities.</returns>
        public static double ComputeLuminosity(double absoluteBolumetricMagnitude)
        {
            return Math.Pow(10.0, 0.4 * (PhysicalConstants.SolarAbsoluteBolumetricMagnitude -
                                         absoluteBolumetricMagnitude));
        }

        /// <summary>
        /// Computes the luminosity using the radius-temperature relationship..
        /// </summary>
        /// <param name="radius">The radius, in Solar radii.</param>
        /// <param name="temperature">The temperature, in degrees Kelvin.</param>
        /// <returns>The luminosity of the star, in units of Solar luminosities.</returns>
        public static double ComputeLuminosity(double radius, double temperature)
        {
            return Math.Pow(10.0, 2 * Math.Log10(radius) + 4 *
                Math.Log10(temperature / PhysicalConstants.SolarEffectiveTemperature));
        }

        /// <summary>
        /// Computes the radius of the star using the luminosity / temperature relationship.
        /// </summary>
        /// <param name="luminosity">The luminosity of the star, in units of Solar luminosities.</param>
        /// <param name="temperature">The effective temperature of the star, in degrees Kelvin.</param>
        /// <returns>The radius of the star, in units of Solar radii.</returns>
        public static double ComputeRadius(double luminosity, double temperature)
        {
            return Math.Sqrt(luminosity) * Math.Pow(PhysicalConstants.SolarEffectiveTemperature / temperature, 2.0);
        }

        /// <summary>
        /// Computes the temperature using the luminosity-radius relationship.
        /// </summary>
        /// <param name="luminosity">The luminosity of the star, in units of Solar luminosities.</param>
        /// <param name="radius">The radius of the star, in units of Solar radii.</param>
        /// <returns></returns>
        public static double ComputeTemperature(double luminosity, double radius)
        {
            return Math.Pow(10.0, (Math.Log10(luminosity) - 
                2 * (Math.Log10(radius))) / 4) * PhysicalConstants.EarthEffectiveTemperature;
        }

        /// <summary>
        /// Computes the Absolute Bolumetric Magnitude.
        /// </summary>
        /// <param name="luminosity">The luminosity, in units of Solar luminosities.</param>
        /// <returns>The Absolute Bolumetric Magnitude.</returns>
        public static double ComputeAbsoluteBolumetricMagnitude(double luminosity)
        {
            return PhysicalConstants.SolarAbsoluteBolumetricMagnitude -
                   2.5 * Math.Log10(luminosity);
        }

        /// <summary>
        /// Calculates average density of body, given mass and radius.
        /// </summary>
        /// <param name="mass">Mass in Solar masses.</param>
        /// <param name="equatorialRadius">Mean equatorial radius in Solar radii.</param>
        /// <returns>Average density in units of Kg/m3.</returns>
        public static double ComputeDensity(double mass, double equatorialRadius)
        {
           
            mass *= PhysicalConstants.SolarMassInKilograms;
            equatorialRadius *= PhysicalConstants.SolarRadiusInKilometers * PhysicalConstants.MetersPerKilometer;
            double volume = (4.0 * Math.PI * Math.Pow(equatorialRadius,3)) / 3.0;
            return (mass / volume);
        }

        /// <summary>
        /// Empirically determines the luminosity, using the mass-luminosity relationship.
        /// </summary>
        /// <param name="mass">The mass, in units of solar masses.</param>
        /// <returns>The luminosity, in units of Solar luminosities.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Solar Masses</term>
        ///         <description>Luminosity</description>
        ///     </listheader>
        ///     <item>
        ///         <term>M* lower than 0.43 Msol</term>
        ///         <description>L = 0.23*M^2.3</description>
        ///     </item>
        ///     <item>
        ///         <term>M* lower than 2.0 Msol</term>
        ///         <description>L = 0.75*M^4.8</description>
        ///     </item>
        ///     <item>
        ///         <term>M* lower than 20.0 Msol</term>
        ///         <description>L = 1.75*M^3.5</description>
        ///     </item>
        ///     <item>
        ///         <term>M* greater than 20.0 Msol</term>
        ///         <description>L = 81*M^2.14</description>
        ///     </item>
        /// </list> 
        /// </returns>
        /// <remarks>This formula is known to be inaccurate.</remarks>
        public static double EstimateEmpiricalLuminosity(double mass)
        {
            if (mass < 0.43)
                return 0.23*Math.Pow(mass, 2.3);
            else if (mass <= 2)
                return 0.75*Math.Pow(mass, 4.8);
            else if (mass <= 20)
                return 1.75*Math.Pow(mass, 3.5);
            else
                return 81*Math.Pow(mass, 2.14);
        }

        /// <summary>
        /// Empirically determines the main sequence lifespan of a star, 
        /// given its mass and its luminosity.
        /// </summary>
        /// <param name="mass">The mass of the star, in units of Solar masses.</param>
        /// <param name="luminosity">The luminosity, in units of Solar luminosities.</param>
        /// <returns>The lifespan of the star on the main sequence, in thousands of years.</returns>
        public static double EstimateMainSequenceLifespan(double mass, double luminosity)
        {
            return (mass / luminosity) * 10;
        }

        /// <summary>
        /// Estimates the <b>Zero Age Main Sequence</b> luminosity, using polynomial
        /// curve fitting.
        /// </summary>
        /// <param name="mass">The mass.</param>
        /// <returns></returns>
        public static double EstimateZamsLuminosity(double mass)
        {
            return Math.Pow(10.0, AvengersUtd.Odyssey.MathHelper.PolynomialFit(Math.Log10(mass), lZamsFit));
        }

        /// <summary>
        /// Estimates the mass loss due to solar wind.
        /// </summary>
        /// <param name="luminosity">The luminosity of the star, in units of solar luminosities.</param>
        /// <returns>The amoun of solar masses loss per year due to solar wind.</returns>
        public static double EstimateMassLoss(double luminosity)
        {
            return Math.Pow(luminosity, 1.7) * 1E-14;
        }

        public static double EstimateEmpiricalBolumetricCorrection(double temperature)
        {
            double logT = Math.Log10(temperature) - 4;
            double term1 = -8.499 * Math.Pow(logT, 4.0);
            double term2 = 13.421 * Math.Pow(logT, 3.0);
            double term3 = -8.131 * Math.Pow(logT, 2.0);
            double term4 = -3.901 * (logT) - 0.438;

            return term1 + term2 + term3 + term4;
        }

        /// <summary>
        /// Computes the SemiMajor Axis of the star whose mass is M.
        /// </summary>
        /// <param name="M">The mass of the central body.</param>
        /// <param name="m">The mass of the orbiting body.</param>
        /// <param name="d">The mean separation, in AUs.</param>
        /// <returns></returns>
        public static double ComputeSemiMajorAxis(double M, double m, double d)
        {
            return (m / (M + m)) * d;
        }

        /// <summary>
        /// Computes the orbital period of the star, in years.
        /// </summary>
        /// <param name="M">The mass of the central body.</param>
        /// <param name="m">The mass of the orbiting body.</param>
        /// <param name="a">The length of the semimajor axis, in AUs.</param>
        /// <returns>The length of the orbital period, in years.</returns>
        public static double ComputeOrbitalPeriod(double M, double m, double a)
        {
            return Math.Sqrt(Math.Pow(a, 3) / (M + m));
        }
#endregion

        static double GenerateSeparation()
        {
            int diceRoll = Dice.Roll1D(10);
            double meanSeparation = 0;

            if (diceRoll <= 3)
            {
                meanSeparation = Dice.Roll1D(10) * 0.05;
            }
            else if (diceRoll <= 6)
            {
                meanSeparation = Dice.Roll1D(10) * 0.5;
            }
            else if (diceRoll <= 8)
            {
                meanSeparation = Dice.Roll1D(10) * 3.0;
            }
            else if (diceRoll == 9)
            {
                meanSeparation = Dice.Roll1D(10) * 20.0;
            }
            else
            {
                meanSeparation = Dice.Roll1D(10) * 200.0;
            }

            return rnd.About(meanSeparation, 0.1);
        }

        static double GenerateEccentricity()
        {
            int diceRoll = Dice.Roll1D(10);
            double orbitEccentricity = 0;

            if (diceRoll <= 2)
            {
                orbitEccentricity = Dice.Roll1D(10) * 0.01;
            }
            else if (diceRoll <= 4)
            {
                orbitEccentricity = 0.1 + Dice.Roll1D(10) * 0.01;
            }
            else if (diceRoll <= 6)
            {
                orbitEccentricity = 0.2+ Dice.Roll1D(10) * 0.01;
            }
            else if (diceRoll <= 8)
            {
                orbitEccentricity = 0.3 + Dice.Roll1D(10) * 0.01;
            }
            else if (diceRoll == 9)
            {
                orbitEccentricity = 0.4 + Dice.Roll1D(10) * 0.01;
            }
            else
            {
                orbitEccentricity = 0.5 + Dice.Roll1D(10) * 0.04;
            }

            return rnd.About(orbitEccentricity, 0.05);
        }

        /// <summary>
        /// Generates a primary star for a solar system.
        /// </summary>
        /// <param name="starSystemName">Name of the star system.</param>
        /// <param name="starMass">The mass of the star.</param>
        /// <param name="evolutionResult">The result of the stellar evolution.</param>
        /// <param name="mainSequenceFeatures">A <see cref="StellarFeatures"/> object detailing
        /// the features of the star, as if it were a Main Sequence star. If the star is old enough,
        /// it will evolve off the main sequence and become either a Sub Giant, a Giant, a White Dwarf,
        /// a Neutron Star or a Black Hole. This <b>out</b> parameter will then contain the features
        /// of the star before it evolved.</param>
        /// <returns>A <see cref="StellarFeatures"/> object detaiing the features of the star, 
        /// after the eventual evolution.</returns>
        /// <remarks>If the star doesn't leave the main sequence, the two objects won't be equal, 
        /// because as a main sequence star ages, its luminosity increases. Therefore the <b>out</b>
        /// parameter can be thought as the <i>average</i> parameters a star of that type will have.</remarks>
        public Star GenerateSingleStarSystem(string starSystemName, double starMass, 
            out EvolutionResult evolutionResult, out StellarFeatures mainSequenceFeatures)
        {
            return GenerateStarData(starSystemName, -1, starMass, GetRandomAge(galaxyOptions.Age),
                out evolutionResult, out mainSequenceFeatures);
        }

        public Star[] GenerateMultipleStarSystem(string starSystemName, double[] starMasses, 
            out EvolutionResult[] evolutionResult, out StellarFeatures[] mainSequenceFeatures)
        {
            int length = starMasses.Length;
            evolutionResult = new EvolutionResult[length];
            mainSequenceFeatures = new StellarFeatures[length];
            Star[] starSystem = new Star[length];

            double starSystemAge = GetRandomAge(galaxyOptions.Age);
            double starMassesSum=0;

            // Standing to current observations, multiple star
            // system tend to "couple" stars. They are likely to 
            // be found in pairs.
            for (int i=0; i<starMasses.Length; i=i+2)
            {
                // Generate stars by pairs
                if ((starMasses.Length - i) >= 2)
                {
                    // Generate mean separation between the two stars
                    double separation = GenerateSeparation();

                    // Inits the star mass value:
                    starMassesSum += starMasses[i];

                    // generate random semimajor axis and eccentricity for the first star
                    semiMajorAxis = ComputeSemiMajorAxis(starMassesSum, starMasses[i + 1], separation);
                    eccentricity = GenerateEccentricity();
                    orbitalPeriod = ComputeOrbitalPeriod(starMassesSum, starMasses[i + 1], semiMajorAxis);
                    starSystem[i] = GenerateStarData(starSystemName, i, starMasses[i], starSystemAge,
                                                     out evolutionResult[i], out mainSequenceFeatures[i]);
                    
                    // generate random semimajor axis and eccentricity for the next star
                    semiMajorAxis = ComputeSemiMajorAxis(starMasses[i+1], starMassesSum, separation);
                    eccentricity = GenerateEccentricity();
                    orbitalPeriod = ComputeOrbitalPeriod(starMassesSum, starMasses[i + 1], semiMajorAxis);
                    starSystem[i + 1] = GenerateStarData(starSystemName, i + 1, starMasses[i + 1], starSystemAge,
                                                         out evolutionResult[i + 1], out mainSequenceFeatures[i + 1]);
                    starMassesSum += starMasses[i + 1];
                }
                else
                {
                    // Generate mean separation between the binary pair and the lone companion
                    double separation = GenerateSeparation();
                    semiMajorAxis = ComputeSemiMajorAxis(starMassesSum, starMasses[i], separation);
                    eccentricity = GenerateEccentricity();
                    orbitalPeriod = ComputeOrbitalPeriod(starMassesSum, starMasses[i], semiMajorAxis);
                    starSystem[i] = GenerateStarData(starSystemName, i, starMasses[i], starSystemAge,
                                                     out evolutionResult[i], out mainSequenceFeatures[i]);

                }
            }
            return starSystem;
            
        }



        /// <summary>
        /// Generates the star data for companion stars.
        /// </summary>
        /// <param name="starSystemName">Name of the star system.</param>
        /// <param name="starNumber">Stars are ordered depending on their mass. In a multiple star
        /// system, the most massive star has the <b>A</b> suffix. The next most massive is the
        /// <b>B</b> one, and so on. This parameter therefore describes the order of the star in the system.</param>
        /// <param name="starMass">The mass of the star.</param>
        /// <param name="systemAge">The age of the star system, in GigaYears (1E9). All of the stars in a
        /// multiple star system have the same age.</param>
        /// <param name="evolutionResult">The result of the stellar evolution.</param>
        /// <param name="mainSequenceFeatures">A <see cref="StellarFeatures"/> object detailing
        /// the features of the star, as if it were a Main Sequence star. If the star is old enough,
        /// it will evolve off the main sequence and become either a Sub Giant, a Giant, a White Dwarf,
        /// a Neutron Star or a Black Hole. This <b>out</b> parameter will then contain the features
        /// of the star before it evolved.</param>
        /// <returns>A <see cref="StellarFeatures"/> object detaiing the features of the star, 
        /// after the eventual evolution.</returns>
        /// <remarks>If the star doesn't leave the main sequence, the two objects won't be equal, 
        /// because as a main sequence star ages, its luminosity increases. Therefore the <b>out</b>
        /// parameter can be thought as the <i>average</i> parameters a star of that type will have.</remarks>
        public Star GenerateStarData(string starSystemName, int starNumber, double starMass, double systemAge,
            out EvolutionResult evolutionResult, out StellarFeatures mainSequenceFeatures)
        {
            mass = starMass;
            int[] iValues = MathHelper.FindInterpolationInterval(mass, massTable);
            spectralType = iValues[1] / 10;
            subType = iValues[1] % 10;
            luminosityClass = LuminosityClass.V;
            
            starName = starNumber == -1 ? starSystemName : GetStarName(starSystemName, starNumber);
            magnitude = rnd.About(MathHelper.CubicInterpolation(bolumetricMagnitudeTable, iValues), 0.005);
            temperature = rnd.About(MathHelper.CubicInterpolation(temperatureTable, iValues), 0.01);
            luminosity = ComputeLuminosity(magnitude);
            radius = ComputeRadius(luminosity, temperature);
            lifespan = EstimateMainSequenceLifespan(mass, luminosity);
            density = ComputeDensity(mass, radius);

            age = systemAge;

            mainSequenceFeatures = new StellarFeatures(
                magnitude, mass, luminosity, temperature,
                radius, semiMajorAxis, eccentricity, orbitalPeriod,
                density, age,
                new SpectralClass((SpectralType)spectralType, subType, luminosityClass));

            return Evolve(out evolutionResult);
        }

        

   
        static double GetRandomAge(GalaxyAge galaxyAge)
        {
            double chance = rnd.NextDouble();
            double age;

            if (chance <= 0.05)
            {
                age = 0.1;
            }
            else if (chance <= 0.3)
            {
                age = 0.1 + 0.15*Dice.Roll1D(10) + 0.025*Dice.Roll1D(10);
            }
            else if (chance <= 0.6)
            {
                age = 2.0 + 0.3*Dice.Roll1D(10) + 0.05*Dice.Roll1D(10);
            }
            else if (chance <= 0.8)
            {
                age = 5 + 0.3*Dice.Roll1D(10) + 0.05*Dice.Roll1D(10);
            }
            else if (chance <= 0.95)
            {
                age = 8.0 + 0.3*Dice.Roll1D(10) + 0.05*Dice.Roll1D(10);
            }
            else
            {
                age = 10.0 + 0.3*Dice.Roll1D(10) + 0.05*Dice.Roll1D(10);
            }

            return rnd.About(age,0.1);
        }

        Star Evolve(out EvolutionResult evolutionResult)
        {
            double subGiantLifespan = rnd.About(lifespan * 0.15, 0.05);
            double giantLifespan = rnd.About(lifespan * 0.2, 0.05);

            if (age < lifespan)
            {
                // The star is in the main sequence
                EvolveOnMainSequence();
                evolutionResult = EvolutionResult.MainSequence;
            }
            else if (age < lifespan + subGiantLifespan)
            {
                // The star has become a subgiant
                EvolveAsSubGiant(subGiantLifespan);
                evolutionResult = EvolutionResult.SubGiant;
            }
            else if (age < lifespan + giantLifespan)
            {
                // The star has become a giant
                EvolveAsGiant();
                evolutionResult = EvolutionResult.Giant;
            }
            else
            {
                // The star is now dead
                if (mass <= 1.4)
                {
                    EvolveAsWhiteDwarf(giantLifespan);
                    evolutionResult = EvolutionResult.WhiteDwarf;
                }
                else if (mass <= 8)
                {
                    EvolveAsNeutronStar();
                    evolutionResult = EvolutionResult.Neutron;
                }
                else
                {
                    EvolveAsBlackHole();
                    evolutionResult = EvolutionResult.BlackHole;
                }
            }

            StellarFeatures evolvedFeatures = new StellarFeatures(magnitude, mass, luminosity, temperature, radius, semiMajorAxis, eccentricity, orbitalPeriod, density, age,
                new SpectralClass((SpectralType)spectralType, subType, luminosityClass));

            return new Star(starName, evolvedFeatures);
            
        }

        void EvolveOnMainSequence()
        {
            int index = (int)Math.Round((age / lifespan) * 9);
            double luminosityModifier = rnd.About(luminosityModifiers[spectralType, index],0.1);
            if (luminosityModifier != 0.0)
            {
                luminosity = luminosity * (1 + luminosityModifier);
                magnitude = ComputeAbsoluteBolumetricMagnitude(luminosity);
            }

            luminosityClass = LuminosityClass.V;
        }

        void EvolveAsSubGiant(double subGiantLifespan)
        {
            double luminosityModifier = rnd.About(luminosityModifiers[spectralType, 9], 0.1);
            luminosity = luminosity * (1 + luminosityModifier);
            magnitude = ComputeAbsoluteBolumetricMagnitude(luminosity);
            temperature = temperature - (((age - lifespan) / subGiantLifespan) * (temperature - 4800));
            radius = ComputeRadius(luminosity, temperature);
            

            int[] iValues =MathHelper.FindInterpolationInterval(temperature, temperatureTable);
            spectralType = iValues[1] / 10;
            subType = iValues[1] % 10;

            luminosityClass = LuminosityClass.IV;
        }

        void EvolveAsGiant()
        {
            double luminosityModifier = rnd.About(luminosityModifiers[spectralType, 9], 0.1);
            luminosity = 25 * luminosity * (1 + luminosityModifier);
            magnitude = ComputeAbsoluteBolumetricMagnitude(luminosity);

            temperature = rnd.NextDouble(3000, 5000);
            radius = ComputeRadius(luminosity, temperature);

            int[] iValues = MathHelper.FindInterpolationInterval(temperature,temperatureTable);
            spectralType = iValues[1] / 10;
            subType = iValues[1] % 10;

            if (luminosity > 1000000)
                luminosityClass = LuminosityClass.O;
            else if (luminosity > 100000)
                luminosityClass = LuminosityClass.Ia;
            else if (luminosity > 10000)
                luminosityClass = LuminosityClass.Ib;
            else if (luminosity > 1000)
                luminosityClass = LuminosityClass.II;
            else
                luminosityClass = LuminosityClass.IV;

        }

        void EvolveAsWhiteDwarf(double giantLifespan)
        {

            luminosity =Math.Max(Math.Pow(mass * 0.12, 2.0) - (age / 1000),0);
            double temp = (mass * 0.12 * PhysicalConstants.SolarMassInKilograms) / 1E9;
            radius = (Math.Pow(temp / ((4.0 / 3.0) * Math.PI), 1.0 / 3.0) / 1000) /
                     PhysicalConstants.SolarRadiusInKilometers;
            mass = rnd.NextDouble(0.35, mass);
            temperature = Math.Sqrt(1.0 / radius) * Math.Pow(luminosity, 0.25) *
                          PhysicalConstants.SolarEffectiveTemperature;

            spectralType = 8;
            subType = (int)(50400 / temperature);
            luminosityClass = LuminosityClass.VII;
        }

        void EvolveAsNeutronStar()
        {
            temperature = rnd.About(1E9, 0.001);
            mass = rnd.NextDouble(1.35, 3.0);
            radius = Math.Pow(mass, -1.0 / 3.0) / 4E-2;
            luminosity = rnd.NextDouble(0, 0.001);
            // 9 for Neutron Stars
            spectralType = 9;
            luminosityClass = LuminosityClass.None;
        }

        void EvolveAsBlackHole()
        {
            luminosity = radius = temperature = 0;
            // 10 for Black Holes
            spectralType = 10;
            luminosityClass = LuminosityClass.None;
        }

        static string GetStarName(string systemName, int starNumber)
        {
            return string.Format("{0} {1}", systemName, Char.ConvertFromUtf32(65 + starNumber));
        }
    }
}