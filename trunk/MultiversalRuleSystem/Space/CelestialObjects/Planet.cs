using System;
using System.Collections.Generic;
using System.Text;

namespace AvengersUtd.MultiversalRuleSystem.Space.CelestialObjects
{
    public class Planet : PrimaryBody
    {
        public Planet(string name, StellarFeatures stellarFeatures):
            base(name,stellarFeatures){}
    }
}
