using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using AvengersUtd.MultiversalRuleSystem.Space;
using AvengersUtd.MultiversalRuleSystem.Space.CelestialObjects;
using AvengersUtd.Odyssey.Collections;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Resources;
using AvengersUtd.Odyssey.Utils.Xml;
using SlimDX.Direct3D9;
using AvengersUtd.MultiversalRuleSystem;
using AvengersUtd.Odyssey;

namespace AvengersUtd.StarOdyssey.Resources
{
    public struct AtmosphereDescriptor
    {
        AtmosphericDensity atmosphericDensity;
        TextureDescriptor[] textureDescriptors;

        [XmlAttribute]
        public AtmosphericDensity AtmosphericDensity
        {
            get { return atmosphericDensity; }
            set { atmosphericDensity = value; }
        }

        public TextureDescriptor[] TextureDescriptors
        {
            get { return textureDescriptors; }
            set { textureDescriptors = value; }
        }

        public AtmosphereDescriptor(AtmosphericDensity atmosphericDensity, TextureDescriptor[] textureDescriptors)
        {
            this.atmosphericDensity = atmosphericDensity;
            this.textureDescriptors = textureDescriptors;
        }
    }

    public struct ClimateDescriptor
    {
        Climate climate;
        TextureDescriptor[] textureDescriptors;

        [XmlAttribute]
        public Climate Climate
        {
            get { return climate; }
            set { climate = value; }
        }

        public TextureDescriptor[] TextureDescriptors
        {
            get { return textureDescriptors; }
            set { textureDescriptors = value; }
        }

        public ClimateDescriptor(Climate climate, TextureDescriptor[] textureDescriptors)
        {
            this.climate = climate;
            this.textureDescriptors = textureDescriptors;
        }

        public EntityDescriptor ConvertToEntityDescriptor(PlanetSize size, AtmosphericDensity atmosphericDensity)
        {
            string sizeString = size.ToString();
            string label = sizeString + climate.ToString();
            string meshPath = string.Format("{0}Planets/{1}.x", Global.MeshPath, sizeString);

            switch (climate)
            {
                case Climate.Cytherean:
                case Climate.AzurianJovian:
                case Climate.CryoJovian:
                case Climate.EpistellarJovian:
                case Climate.HydroJovian:
                case Climate.HyperthermicJovian:
                case Climate.EuJovian:
                    return new EntityDescriptor(label,
                                                new MeshDescriptor(meshPath),
                                                new MaterialDescriptor(typeof (DiffuseMaterial),
                                                                       textureDescriptors));

                default:
                    if (atmosphericDensity != AtmosphericDensity.None &&
                        atmosphericDensity != AtmosphericDensity.Traces)
                    {
                        Array.Resize(ref textureDescriptors, textureDescriptors.Length + 1);
                        AtmosphereDescriptor aDesc = ResourceLocator.GetRandomAtmosphereDescriptor(atmosphericDensity);
                        textureDescriptors[3] = aDesc.TextureDescriptors[0];

                        return new EntityDescriptor(label,
                                                    new MeshDescriptor(meshPath),
                                                    new MaterialDescriptor[]
                                                        {
                                                            new MaterialDescriptor(typeof (SurfaceAtmosphereMaterial),
                                                                                   textureDescriptors),
                                                            new MaterialDescriptor(typeof (AtmosphereMaterial))
                                                        });
                    }
                    else
                    {
                        return new EntityDescriptor(label,
                                                    new MeshDescriptor(meshPath),
                                                    new MaterialDescriptor(typeof (SurfaceMaterial), textureDescriptors));
                    }
            }
        }
    }

    public static class ResourceLocator
    {
        // Terrestrial Planets
        static List<ClimateDescriptor> ammoniaClimateDescriptors;
        static List<ClimateDescriptor> areanClimateDescriptors;
        static List<ClimateDescriptor> aridClimateDescriptors;
        static List<ClimateDescriptor> cereanClimateDescriptors;
        static List<ClimateDescriptor> cythereanClimateDescriptors;
        static List<ClimateDescriptor> desertClimateDescriptors;
        static List<ClimateDescriptor> ferrinianClimateDescriptors;
        static List<ClimateDescriptor> glacialClimateDescriptors;
        static List<ClimateDescriptor> hadeanClimateDescriptors;
        static List<ClimateDescriptor> hephaestianClimateDescriptors;
        static List<ClimateDescriptor> kuiperianClimateDescriptors;
        static List<ClimateDescriptor> oceanClimateDescriptors;
        static List<ClimateDescriptor> pelagicClimateDescriptors;
        static List<ClimateDescriptor> selenianClimateDescriptors;
        static List<ClimateDescriptor> terranClimateDescriptors;
        static List<ClimateDescriptor> titanianClimateDescriptors;
        static List<ClimateDescriptor> tundraClimateDescriptors;
        static List<ClimateDescriptor> volcanicClimateDescriptors;

        // Gas Giants
        static List<ClimateDescriptor> azurianJovianClimateDescriptors;
        static List<ClimateDescriptor> cryoJovianClimateDescriptors;
        static List<ClimateDescriptor> epistellarJovianClimateDescriptors;
        static List<ClimateDescriptor> hydroJovianClimateDescriptors;
        static List<ClimateDescriptor> hyperthermicJovianClimateDescriptors;
        static List<ClimateDescriptor> jovianClimateDescriptors;

        // Clouds
        static List<AtmosphereDescriptor> thinAtmosphereDescriptors;
        static List<AtmosphereDescriptor> standardAtmosphereDescriptors;
        static List<AtmosphereDescriptor> denseAtmosphereDescriptors;
        static List<AtmosphereDescriptor> superdenseAtmosphereDescriptors;

        static ResourceLocator()
        {
            ClimateDescriptor[] climateList = Data.DeserializeCollection<ClimateDescriptor>("ClimateDescriptors.xml");
            AtmosphereDescriptor[] atmosphereList =
                Data.DeserializeCollection<AtmosphereDescriptor>("AtmosphereDescriptors.xml");

            // Terrestrial Planets
            ammoniaClimateDescriptors = new List<ClimateDescriptor>();
            areanClimateDescriptors = new List<ClimateDescriptor>();
            aridClimateDescriptors = new List<ClimateDescriptor>();
            cereanClimateDescriptors = new List<ClimateDescriptor>();
            cythereanClimateDescriptors = new List<ClimateDescriptor>();
            desertClimateDescriptors = new List<ClimateDescriptor>();
            ferrinianClimateDescriptors = new List<ClimateDescriptor>();
            glacialClimateDescriptors = new List<ClimateDescriptor>();
            hadeanClimateDescriptors = new List<ClimateDescriptor>();
            hephaestianClimateDescriptors = new List<ClimateDescriptor>();
            kuiperianClimateDescriptors = new List<ClimateDescriptor>();
            oceanClimateDescriptors = new List<ClimateDescriptor>();
            pelagicClimateDescriptors = new List<ClimateDescriptor>();
            selenianClimateDescriptors = new List<ClimateDescriptor>();
            terranClimateDescriptors = new List<ClimateDescriptor>();
            titanianClimateDescriptors = new List<ClimateDescriptor>();
            tundraClimateDescriptors = new List<ClimateDescriptor>();
            volcanicClimateDescriptors = new List<ClimateDescriptor>();

            // Gas Giants
            azurianJovianClimateDescriptors = new List<ClimateDescriptor>();
            cryoJovianClimateDescriptors = new List<ClimateDescriptor>();
            epistellarJovianClimateDescriptors = new List<ClimateDescriptor>();
            hydroJovianClimateDescriptors = new List<ClimateDescriptor>();
            hyperthermicJovianClimateDescriptors = new List<ClimateDescriptor>();
            jovianClimateDescriptors = new List<ClimateDescriptor>();

            // Clouds

            thinAtmosphereDescriptors = new List<AtmosphereDescriptor>();
            standardAtmosphereDescriptors = new List<AtmosphereDescriptor>();
            denseAtmosphereDescriptors = new List<AtmosphereDescriptor>();
            superdenseAtmosphereDescriptors = new List<AtmosphereDescriptor>();

            #region Climate Descriptors

            foreach (ClimateDescriptor cDesc in climateList)
            {
                switch (cDesc.Climate)
                {
                        // Terrestrial Planets
                    case Climate.Ammonia:
                        ammoniaClimateDescriptors.Add(cDesc);
                        break;

                    case Climate.Arean:
                        areanClimateDescriptors.Add(cDesc);
                        break;

                    case Climate.Arid:
                        aridClimateDescriptors.Add(cDesc);
                        break;

                    case Climate.Cerean:
                        cereanClimateDescriptors.Add(cDesc);
                        break;

                    case Climate.Cytherean:
                        cythereanClimateDescriptors.Add(cDesc);
                        break;

                    case Climate.Desert:
                        desertClimateDescriptors.Add(cDesc);
                        break;

                    case Climate.Ferrinian:
                        ferrinianClimateDescriptors.Add(cDesc);
                        break;

                    case Climate.Glacial:
                        glacialClimateDescriptors.Add(cDesc);
                        break;

                    case Climate.Hadean:
                        hadeanClimateDescriptors.Add(cDesc);
                        break;

                    case Climate.Hephaestian:
                        hephaestianClimateDescriptors.Add(cDesc);
                        break;

                    case Climate.Kuiperian:
                        kuiperianClimateDescriptors.Add(cDesc);
                        break;

                    case Climate.Ocean:
                        oceanClimateDescriptors.Add(cDesc);
                        break;

                    case Climate.Pelagic:
                        pelagicClimateDescriptors.Add(cDesc);
                        break;

                    case Climate.Selenian:
                        selenianClimateDescriptors.Add(cDesc);
                        break;

                    case Climate.Terran:
                        terranClimateDescriptors.Add(cDesc);
                        break;

                    case Climate.Titanian:
                        titanianClimateDescriptors.Add(cDesc);
                        break;

                    case Climate.Tundra:
                        tundraClimateDescriptors.Add(cDesc);
                        break;

                    case Climate.Volcanic:
                        volcanicClimateDescriptors.Add(cDesc);
                        break;

                        // Gas Giants
                    case Climate.AzurianJovian:
                        azurianJovianClimateDescriptors.Add(cDesc);
                        break;

                    case Climate.CryoJovian:
                        cryoJovianClimateDescriptors.Add(cDesc);
                        break;

                    case Climate.EpistellarJovian:
                        epistellarJovianClimateDescriptors.Add(cDesc);
                        break;

                    case Climate.HydroJovian:
                        hydroJovianClimateDescriptors.Add(cDesc);
                        break;

                    case Climate.HyperthermicJovian:
                        hyperthermicJovianClimateDescriptors.Add(cDesc);
                        break;

                    case Climate.EuJovian:
                        jovianClimateDescriptors.Add(cDesc);
                        break;
                }
            }

            #endregion

            #region Atmosphere Descriptors

            foreach (AtmosphereDescriptor aDesc in atmosphereList)
            {
                switch (aDesc.AtmosphericDensity)
                {
                    case AtmosphericDensity.Thin:
                        thinAtmosphereDescriptors.Add(aDesc);
                        break;

                    case AtmosphericDensity.Standard:
                        standardAtmosphereDescriptors.Add(aDesc);
                        break;

                    case AtmosphericDensity.Dense:
                        denseAtmosphereDescriptors.Add(aDesc);
                        break;

                    case AtmosphericDensity.Superdense:
                        superdenseAtmosphereDescriptors.Add(aDesc);
                        break;
                }
            }

            #endregion
        }

        public static ClimateDescriptor GetRandomClimateDescriptor(Climate climate)
        {
            int index;
            switch (climate)
            {
                    // Terrestrial Planets
                case Climate.Ammonia:
                    index = Dice.Roll1D(ammoniaClimateDescriptors.Count) - 1;
                    return ammoniaClimateDescriptors[index];

                case Climate.Arean:
                    index = Dice.Roll1D(areanClimateDescriptors.Count) - 1;
                    return areanClimateDescriptors[index];

                case Climate.Arid:
                    index = Dice.Roll1D(aridClimateDescriptors.Count) - 1;
                    return aridClimateDescriptors[index];

                case Climate.Cerean:
                    index = Dice.Roll1D(cereanClimateDescriptors.Count) - 1;
                    return cereanClimateDescriptors[index];

                case Climate.Cytherean:
                    index = Dice.Roll1D(cythereanClimateDescriptors.Count) - 1;
                    return cythereanClimateDescriptors[index];

                case Climate.Desert:
                    index = Dice.Roll1D(desertClimateDescriptors.Count) - 1;
                    return desertClimateDescriptors[index];

                case Climate.Ferrinian:
                    index = Dice.Roll1D(ferrinianClimateDescriptors.Count) - 1;
                    return ferrinianClimateDescriptors[index];

                case Climate.Glacial:
                    index = Dice.Roll1D(glacialClimateDescriptors.Count) - 1;
                    return glacialClimateDescriptors[index];

                case Climate.Hadean:
                    index = Dice.Roll1D(hadeanClimateDescriptors.Count) - 1;
                    return hadeanClimateDescriptors[index];

                case Climate.Hephaestian:

                    index = Dice.Roll1D(hephaestianClimateDescriptors.Count) - 1;
                    return hephaestianClimateDescriptors[index];

                case Climate.Kuiperian:
                    index = Dice.Roll1D(kuiperianClimateDescriptors.Count) - 1;
                    return kuiperianClimateDescriptors[index];

                case Climate.Ocean:
                    index = Dice.Roll1D(oceanClimateDescriptors.Count) - 1;
                    return oceanClimateDescriptors[index];

                case Climate.Pelagic:
                    index = Dice.Roll1D(pelagicClimateDescriptors.Count) - 1;
                    return pelagicClimateDescriptors[index];

                case Climate.Selenian:
                    index = Dice.Roll1D(selenianClimateDescriptors.Count) - 1;
                    return selenianClimateDescriptors[index];

                case Climate.Terran:
                default:
                    index = Dice.Roll1D(terranClimateDescriptors.Count) - 1;
                    return terranClimateDescriptors[index];

                case Climate.Titanian:
                    index = Dice.Roll1D(titanianClimateDescriptors.Count) - 1;
                    return titanianClimateDescriptors[index];

                case Climate.Tundra:
                    index = Dice.Roll1D(tundraClimateDescriptors.Count) - 1;
                    return tundraClimateDescriptors[index];

                case Climate.Volcanic:
                    index = Dice.Roll1D(volcanicClimateDescriptors.Count) - 1;
                    return volcanicClimateDescriptors[index];

                    // Gas Giants
                case Climate.AzurianJovian:
                    index = Dice.Roll1D(azurianJovianClimateDescriptors.Count) - 1;
                    return azurianJovianClimateDescriptors[index];

                case Climate.CryoJovian:
                    index = Dice.Roll1D(cryoJovianClimateDescriptors.Count) - 1;
                    return cryoJovianClimateDescriptors[index];

                case Climate.EpistellarJovian:
                    index = Dice.Roll1D(epistellarJovianClimateDescriptors.Count) - 1;
                    return epistellarJovianClimateDescriptors[index];

                case Climate.HydroJovian:
                    index = Dice.Roll1D(hydroJovianClimateDescriptors.Count) - 1;
                    return hydroJovianClimateDescriptors[index];

                case Climate.HyperthermicJovian:
                    index = Dice.Roll1D(hyperthermicJovianClimateDescriptors.Count) - 1;
                    return hyperthermicJovianClimateDescriptors[index];

                case Climate.EuJovian:
                    index = Dice.Roll1D(jovianClimateDescriptors.Count) - 1;
                    return jovianClimateDescriptors[index];
            }
        }

        public static AtmosphereDescriptor GetRandomAtmosphereDescriptor(AtmosphericDensity density)
        {
            int index;
            switch (density)
            {
                case AtmosphericDensity.Thin:
                    index = Dice.Roll1D(thinAtmosphereDescriptors.Count) - 1;
                    return thinAtmosphereDescriptors[index];

                case AtmosphericDensity.Standard:
                    index = Dice.Roll1D(standardAtmosphereDescriptors.Count) - 1;
                    return standardAtmosphereDescriptors[index];

                case AtmosphericDensity.Dense:
                    index = Dice.Roll1D(denseAtmosphereDescriptors.Count) - 1;
                    return denseAtmosphereDescriptors[index];

                case AtmosphericDensity.Superdense:
                    index = Dice.Roll1D(superdenseAtmosphereDescriptors.Count) - 1;
                    return superdenseAtmosphereDescriptors[index];

                default:
                    return new AtmosphereDescriptor();
            }
        }

        public static EntityDescriptor GetRandomEntityDescriptor(PlanetaryFeatures planetaryFeatures )
        {
            ClimateDescriptor cDesc = GetRandomClimateDescriptor(planetaryFeatures.Climate);
            return cDesc.ConvertToEntityDescriptor(planetaryFeatures.Size, planetaryFeatures.AtmosphericDensity);
        }
    }
}