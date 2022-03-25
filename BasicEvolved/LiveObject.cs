using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicEvolved
{
    public class LiveObject
    {
        protected double[] _position;
        public double[] Position { get => _position; }

        protected double _requireEnergy;
        public double RequireEnergy { get => _requireEnergy; }

    }
}
