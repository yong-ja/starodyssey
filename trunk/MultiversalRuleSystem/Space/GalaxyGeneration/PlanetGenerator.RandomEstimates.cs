#region Disclaimer

/* 
 * PlanetGenerator.RandomEstimates
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

#region Using Directives

    using System;
    using System.Diagnostics;

#endregion

namespace AvengersUtd.MultiversalRuleSystem.Space.GalaxyGeneration
{

    public partial class PlanetGenerator
    {
        /// <summary>
        /// Estimates planetary inclination (axial tilt).  Calibrated to Earth.
        /// </summary>
        /// <param name="orbitalRadius">Semi-major axis of orbit in AU.</param>
        /// <returns>Tilt in degrees</returns>
        public static int GenerateInclination(double orbitalRadius)
        {
            int temp;

            temp = (int)(Math.Pow(orbitalRadius, 0.2) * rnd.About(PhysicalConstants.EarthAxialTilt, 0.4));
            return (temp % 360);
        }


        /// <summary>
        /// Generates the unitless 'volatile gas inventory'.
        /// This implements Fogg's eq.17.
        /// </summary>
        /// <param name="mass">Planetary mass in Earth masses.</param>
        /// <param name="escapeVelocity">Escape velocity in kilometers/second.</param>
        /// <param name="rmsVelocity">Average velocity at molecules at top of atmosphere.</param>
        /// <param name="stellarMass">Stellar mass in Solar masses.</param>
        /// <param name="zone">Orbital 'zone', between 1 and 3.</param>
        /// <param name="greenhouseEffect">if set to<c>true</c> the planet suffers from the greenhouse effect;
        /// <param name="accretedGas">if set to <c>true</c> the planet accreted gas.</param>
        /// <returns>
        /// A unitless 'inventory' calibrated to Earth=1000.
        /// </returns>
        public static double GenerateVolatileGasInventory(double mass, double escapeVelocity, double rmsVelocity,
                                                  double stellarMass, OrbitalZone zone, bool greenhouseEffect, bool accretedGas)
        {
            double velocityRatio, proportionConst = 1.0, temp1, temp2;

            if (rmsVelocity == 0.0) 
                return 0.0;
            velocityRatio = escapeVelocity / rmsVelocity;
            if (velocityRatio >= PhysicalConstants.GasRetentionThreshold)
            {
                switch (zone)
                {
                    case OrbitalZone.Snowline:
                        proportionConst = 140000.0;
                        break;
                    case OrbitalZone.Outer:
                        proportionConst = 75000.0;
                        break;
                    case OrbitalZone.External:
                        proportionConst = 250.0;
                        break;
                    default:
                        Debug.WriteLine("Error: orbital zone not initialized correctly!");
                        break;
                }
                temp1 = (proportionConst * mass) / stellarMass;
                temp2 = rnd.About(temp1, 0.2);
                if (greenhouseEffect)
                    return (temp2);
                else
                    return (temp2 / 140.0); 
            }
            else return (0.0);
        }
    }
}