using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicEvolved
{
    class Generation
    {
        private int _genCount;
        public int GenCount { get => _genCount; }

        private List<Creature> _creatures;
        public List<Creature> Cretures { get => _creatures; }
        
        private List<Creature> _graveyard;
        public List<Creature> Creatures { get => _creatures; }

        private List<Food> _foods;
        public List<Food> Foods { get => _foods; }

        public Generation(int genCount)
        {
            this._genCount = genCount;
        }

        public Generation(List<Creature> creatures, int genCount)
        {
            this._creatures = creatures;
        }

    }
}
