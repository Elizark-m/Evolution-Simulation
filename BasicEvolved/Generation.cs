using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolution_Simulation
{
    class Generation
    {   
        // generation number
        private int _genCount;
        public int GenCount { get => _genCount; }
        
        // cretures including in this generation
        private List<Creature> _creatures;
        public List<Creature> Cretures { get => _creatures; }
        
        // creatures dead in this generation
        private List<Creature> _graveyard;
        public List<Creature> Creatures { get => _creatures; }
        
        // foods spawned in this generation
        private List<Food> _foods;
        public List<Food> Foods { get => _foods; }
        
        // this simple construct
        public Generation(int genCount)
        {
            this._genCount = genCount;
        }
        
        // this with creatures prebuild construct
        public Generation(List<Creature> creatures, int genCount)
        {
            this._creatures = creatures;
        }

    }
}
