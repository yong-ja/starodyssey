using System;
using AvengersUtd.MultiversalRuleSystem.Properties;
using AvengersUtd.MultiversalRuleSystem.Space;

namespace AvengersUtd.MultiversalRuleSystem
{
    public static class CustomTranslator
    {
        const string primaryStatPrefix = "PS_";
        const string secondaryStatPrefix = "SS_";
        const string sizePrefix = "SZ_";

        public static string GetSize(PlanetSize size)
        {
            switch (size)
            {
                case PlanetSize.Asteroid:
                    return Stringtable.SS_SZ_Asteroid;

                case PlanetSize.Tiny:
                    return Stringtable.SS_SZ_Tiny;

                case PlanetSize.Small:
                    return Stringtable.SS_SZ_Small;

                case PlanetSize.Medium:
                    return Stringtable.SS_SZ_Medium;

                case PlanetSize.Large:
                    return Stringtable.SS_SZ_Large;

                case PlanetSize.Huge:
                    return Stringtable.SS_SZ_Huge;

                case PlanetSize.Immense:
                    return Stringtable.SS_SZ_Immense;

                case PlanetSize.Neptunian:
                    return Stringtable.SS_SZ_Neptunian;

                case PlanetSize.SubJovian:
                    return Stringtable.SS_SZ_SubJovian;

                case PlanetSize.Jovian:
                    return Stringtable.SS_SZ_Jovian;

                case PlanetSize.SuperJovian:
                    return Stringtable.SS_SZ_SuperJovian;

                case PlanetSize.MacroJovian:
                    return Stringtable.SS_SZ_MacroJovian;

                default:
                    throw new ArgumentOutOfRangeException("size");
            }
        }

        public static string GetClimate(Climate climate)
        {
            switch (climate)
            {
                case Climate.Hadean:
                    return Stringtable.SS_CL_Hadean;
                case Climate.Cerean:
                    return Stringtable.SS_CL_Cerean;

                case Climate.Kuiperian:
                    return Stringtable.SS_CL_Kuiperian;
                case Climate.Glacial:
                    return Stringtable.SS_CL_Glacial;
                case Climate.Ammonia:
                    return Stringtable.SS_CL_Ammonia;
                case Climate.Volcanic:
                    return Stringtable.SS_CL_Volcanic;
                case Climate.Ferrinian:
                    return Stringtable.SS_CL_Ferrinian;

                case Climate.Arean:
                    return Stringtable.SS_CL_Arean;
                case Climate.Selenian:
                    return Stringtable.SS_CL_Selenian;

                case Climate.Tundra:
                    return Stringtable.SS_CL_Tundra;

                case Climate.Terran:
                    return Stringtable.SS_CL_Terran;

                case Climate.Ocean:
                    return Stringtable.SS_CL_Ocean;
                case Climate.Arid:
                    return Stringtable.SS_CL_Arid;

                case Climate.Desert:
                    return Stringtable.SS_CL_Desert;

                case Climate.Hephaestian:
                    return Stringtable.SS_CL_Hephaestian;
                case Climate.Cataclismic:
                    return Stringtable.SS_CL_Cataclismic;
                case Climate.Pelagic:
                    return Stringtable.SS_CL_Pelagic;
                case Climate.Cytherean:
                    return Stringtable.SS_CL_Cytherean;
                case Climate.HyperthermicJovian:
                    return Stringtable.SS_CL_HyperthermicJovian;
                case Climate.EpistellarJovian:
                    return Stringtable.SS_CL_EpistellarJovian;
                case Climate.AzurianJovian:
                    return Stringtable.SS_CL_AzurianJovian;
                case Climate.HydroJovian:
                    return Stringtable.SS_CL_HydroJovian;

                case Climate.EuJovian:
                    return Stringtable.SS_CL_EuJovian;
                case Climate.CryoJovian:
                    return Stringtable.SS_CL_CryoJovian;
                case Climate.Titanian:
                    return Stringtable.SS_CL_Titanian;
                default:
                    throw new ArgumentOutOfRangeException("climate");
            }
        }

        public static string GetTemperature(Temperature temperature)
        {
            switch (temperature)
            {
                case Temperature.Frozen:
                    return Stringtable.SS_TE_Frozen;
                case Temperature.Icy:
                    return Stringtable.SS_TE_Icy;
                case Temperature.Cold:
                    return Stringtable.SS_TE_Cold;
                case Temperature.Temperate:
                    return Stringtable.SS_TE_Temperate;
                case Temperature.VeryHot:
                    return Stringtable.SS_TE_VeryHot;
                case Temperature.Extreme:
                    return Stringtable.SS_TE_Extreme;
                default:
                    throw new ArgumentOutOfRangeException("temperature");
            }
        }

        public static string GetAtmosphericDensity(AtmosphericDensity density)
        {
            switch (density)
            {
                case AtmosphericDensity.None:
                    return Stringtable.SS_AD_None;
                case AtmosphericDensity.Traces:
                    return Stringtable.SS_AD_Traces;
                case AtmosphericDensity.Thin:
                    return Stringtable.SS_AD_Thin;
                case AtmosphericDensity.Standard:
                    return Stringtable.SS_AD_Standard;
                case AtmosphericDensity.Dense:
                    return Stringtable.SS_AD_Dense;
                case AtmosphericDensity.Superdense:
                    return Stringtable.SS_AD_Superdense;
                default:
                    throw new ArgumentOutOfRangeException("density");
            }
        }

        public static string GetComposition(Composition composition)
        {
            switch (composition)
            {
                case Composition.Vacuum:
                    return Stringtable.SS_CO_Vacuum;
                case Composition.Oxygen:
                    return Stringtable.SS_CO_Oxygen;
                case Composition.CarbonDioxide:
                    return Stringtable.SS_CO_CarbonDioxide;
                case Composition.Helium:
                    return Stringtable.SS_CO_Helium;
                case Composition.Hydrogen:
                    return Stringtable.SS_CO_Hydrogen;
                case Composition.Ammonia:
                    return Stringtable.SS_CO_Ammonia;
                case Composition.Nitrogen:
                    return Stringtable.SS_CO_Nitrogen;
                case Composition.Methane:
                    return Stringtable.SS_CO_Methane;
                default:
                    throw new ArgumentOutOfRangeException("composition");
            }
        }

        public static string GetGravity(Gravity gravity)
        {
            switch (gravity)
            {

                case Gravity.VeryLow:
                    return Stringtable.SS_GR_VeryLow;
                case Gravity.Low:
                    return Stringtable.SS_GR_Low;
                case Gravity.Average:
                    return Stringtable.SS_GR_Average;
                case Gravity.High:
                    return Stringtable.SS_GR_High;
                case Gravity.VeryHigh:
                    return Stringtable.SS_GR_VeryHigh;

                default:
                    throw new ArgumentOutOfRangeException("gravity");
            }
        }

        public static string GetDensity(Density density)
        {
            switch (density)
            {
                case Density.GasGiant:
                    return Stringtable.SS_DE_GasGiant;
                case Density.IcyCore:
                    return Stringtable.SS_DE_IcyCore;
                case Density.SmallIronCore:
                    return Stringtable.SS_DE_SmallIronCore;
                case Density.LargeIronCore:
                    return Stringtable.SS_DE_LargeIronCore;
                default:
                    throw new ArgumentOutOfRangeException("density");
            }
        }
    }
}