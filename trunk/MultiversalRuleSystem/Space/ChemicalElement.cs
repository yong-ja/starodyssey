#region Disclaimer

/* 
 * Gas
 *
 * Created on 03 settembre 2007
 * Author: Adalberto L. Simeone (Taranto, Italy)
 * Website: http://www.avengersutd.com
 *
 * Part of the Multiversal Rule System Library
 *
 * This source code is Intellectual Property of the Author
 * and is released under the Creative Commons Attribution 
 * NonCommercial License, available at:
 * http://creativecommons.org/licenses/by-nc/3.0/ 
 * You can alter and use this source code as you wish, 
 * provided that you do not use the results in commercial
 * projects, without the express and written consent of
 * the Author.
 *
 */

#endregion

namespace AvengersUtd.MultiversalRuleSystem.Space
{
    #region Using Directives

    using System;

    #endregion

    public enum ChemicalSymbol
    {
        Ar,
        CH4,
        Cl,
        CO2,
        F,
        Kr,
        H,
        H2O,
        He,
        O,
        O3,
        N,
        Ne,
        NH3,
        Xe
    }

    public struct ChemicalElement
    {
        public const int GasCount = 16;
        #region Private fields
        
        readonly int atomicNumber;
        readonly double atomicWeight;
        readonly double meltingPointInCelsius;
        readonly double boilingPointInCelsius;
        readonly double density;
        readonly double abundanceEarth;
        readonly double abundanceSun;
        readonly double reactivity;

        readonly string name;
        readonly ChemicalSymbol symbol;

        #endregion       

        #region Properties

        public int AtomicNumber
        {
            get { return atomicNumber; }
        }

        public double AtomicWeight
        {
            get { return atomicWeight; }
        }

        public double MeltingPointInCelsius
        {
            get { return meltingPointInCelsius; }
        }

        public double BoilingPointInCelsius
        {
            get { return boilingPointInCelsius; }
        }

        public double Density
        {
            get { return density; }
        }

        public double AbundanceEarth
        {
            get { return abundanceEarth; }
        }

        public double AbundanceSun
        {
            get { return abundanceSun; }
        }

        public double Reactivity
        {
            get { return reactivity; }
        }

        public string Name
        {
            get { return name; }
        }

        public ChemicalSymbol Symbol
        {
            get { return symbol; }
        }

        #endregion

        #region Constructors

        public ChemicalElement(string name, ChemicalSymbol symbol, int atomicNumber, double atomicWeight, double meltingPointInCelsius, double boilingPointInCelsius, double density, double abundanceEarth, double abundanceSun, double reactivity)
        {
            this.name = name;
            this.symbol = symbol;
            this.atomicNumber = atomicNumber;
            this.atomicWeight = atomicWeight;
            this.meltingPointInCelsius = meltingPointInCelsius;
            this.boilingPointInCelsius = boilingPointInCelsius;
            this.density = density;
            this.abundanceEarth = abundanceEarth;
            this.abundanceSun = abundanceSun;
            this.reactivity = reactivity;
        }

        #endregion

        public static ChemicalElement[] CreateGasTable()
        {
            ChemicalElement[] chemicalElementArray = new ChemicalElement[GasCount];
            chemicalElementArray[0] = new ChemicalElement("Hydrogen", ChemicalSymbol.H, 1, 1.0079, 14.06, 20.4, 8.99E-05, 0.00125893, 27925.4, 1);
            chemicalElementArray[1] = new ChemicalElement("Helium", ChemicalSymbol.He, 2, 4.0026, 3.46, 4.2, 0.0001787, 7.94328E-09,2722.7,0);
            chemicalElementArray[2] = new ChemicalElement("Nitrogen", ChemicalSymbol.N, 7, 14.0067, 63.34, 77.4, 0.0012506,1.99526E-05, 3.13329, 0);
            chemicalElementArray[3] = new ChemicalElement("Oxygen", ChemicalSymbol.O, 8, 15.9994, 54.8, 90.2, 0.001429, 0.501187, 23.8232,10);
            chemicalElementArray[4] = new ChemicalElement("Fluorine", ChemicalSymbol.F, 9, 18.9984, 53.58, 85.1, 0.001696, 0.000630957,0.000843335, 50);
            chemicalElementArray[5] = new ChemicalElement("Neon", ChemicalSymbol.Ne, 10, 20.1700, 24.53, 27.10, 0.0009, 5.01187E-09,3.4435E-5, 0);
            chemicalElementArray[6] = new ChemicalElement("Chlorine", ChemicalSymbol.Cl, 17, 35.5430, 172.22, 239.2, 0.003215, 0.000125893, 0.005236, 40);
            chemicalElementArray[7] = new ChemicalElement("Argon",ChemicalSymbol.Ar, 18, 39.948, 84, 87.3, 0.0017824, 3.16228E-06,0.100925, 0);
            chemicalElementArray[8] = new ChemicalElement("Krypton", ChemicalSymbol.Kr, 36, 83.8, 116.6, 119.7, 0.003708, 1E-10, 4.4978E-05, 0);
            chemicalElementArray[9] = new ChemicalElement("Xenon", ChemicalSymbol.Xe, 54, 131.3, 161.3, 165, 0.00588, 3.16228E-11, 4.69894E-06, 0);
            chemicalElementArray[10] = new ChemicalElement("Ammonia", ChemicalSymbol.NH3, 900, 17, 195.46, 239.66, 0.001, 0.002, 0.0001, 1);
            chemicalElementArray[12] = new ChemicalElement("Water", ChemicalSymbol.H2O, 901, 18, 273.16, 373.16, 1.00, 0.03, 0.001, 0);
            chemicalElementArray[13] = new ChemicalElement("CarbonDioxide", ChemicalSymbol.CO2, 902, 44, 194.66, 194.166, 0.001, 0.01, 0.0005, 0);
            chemicalElementArray[14] = new ChemicalElement("Ozone", ChemicalSymbol.O3, 903, 48, 80.16, 161.16, 0.001, 0.001, 0.000001, 2);
            chemicalElementArray[15] = new ChemicalElement("Methane", ChemicalSymbol.CH4, 904, 16, 90.16, 109.16, 0.010, 0.005, 0.001, 1);

            return chemicalElementArray;
        }
    }

    public struct Gas
    {
        int atomicNumber;
        ChemicalSymbol symbol;
        double pressure;


        public Gas(int atomicNumber, ChemicalSymbol symbol, double pressure)
        {
            this.atomicNumber = atomicNumber;
            this.symbol = symbol;
            this.pressure = pressure;
        }

        #region Properties
        public int AtomicNumber
        {
            get { return atomicNumber; }
        }

        public ChemicalSymbol Symbol
        {
            get { return symbol; }
        }

        public double Pressure
        {
            get { return pressure; }
        } 
        #endregion
    }
}