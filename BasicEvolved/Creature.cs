using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Evolution_Simulation
{

    class Creature : LiveObject
    {
        private Random r;
        // 
        private double _genSpeed = 1;

        private int _oldCount = 0;
        public int OldCount { get => _oldCount; }
        
        //current creature energy 
        private double _energy;
        public double Energy { get => _energy; }
        
        //require energy per generation
        private double _reqEnergy;
        
        // mutation bonus size
        private List<double> _evoSize;
        // mutation bonus speed
        private List<double> _evoSpeed;
        // mutation bonus sense
        private List<double> _evoSense;
        
        // creture size base
        private double _size;
        public double Size { get => this._size; }
        // creture speed base
        private double _speed;
        public double Speed { get => this._speed; }
        //creature sense base, radius of vision
        private double _sense;
        public double Sense { get => this._sense; }
        
        // number of slayed death generation
        private double[] _termination;
        public double[] Termination { get => _termination; }
        // number of born generation
        private int _aliveData = -1;
        public int AliveData { get => this._aliveData; }
        // number of starve death generation 
        private int _deathData = -1;
        public int DeathData { get => this._deathData; }
        
        // name of death reason
        private string _deathMark;
        public string DeathMark { get => this._deathMark; }
        
        // creture, killed by this
        private List<Creature> _victims = new List<Creature>();
        public List<Creature> Victims { get => this._victims; }
        
        // creture killed this
        private Creature _slayer;
        public Creature Slayer { get => this._slayer; }
        
        // simple construct this
        public Creature()
        {
            this._aliveData = 1;
            this._size = 1;
            this._speed = 1;
            this._sense = 1;
        }
        
        // linked construction
        private Creature(Creature parent, int gen)
        {
            this._aliveData = gen;
            
            // check parent-null exception
            if (parent != null)
            {   
                // forming mutation and value for size
                _evoSize.Add(MutateGen());
                this._size = parent._size + _evoSize.Last();
                
                // forming mutation and value for speed
                _evoSpeed.Add(MutateGen());
                this._speed = parent._speed + _evoSpeed.Last();
                
                // forming mutation and value for sense
                _evoSense.Add(MutateGen());
                this._sense = parent._sense + _evoSense.Last();
                
                // forming energy require for creature live
                this._reqEnergy = this._size * this._size * this._size * this._speed * this._speed + this._sense;
            }
        }
        
        //forming creature value change
        private double MutateGen()
        {   
            // 70% debuff creature value
            if (r.NextDouble() > 0.7)
                return -_genSpeed;
            // 50% buff creature value
            else if (r.NextDouble() > 0.5)
                return _genSpeed;
            return 0;
        }
        
        // check creture starve death
        public bool Starve(int gen)
        {
            if (_energy < _reqEnergy)
            {
                Death("Starved", gen);
                return true;
            }
            return false;
        }
        
        // mark death reason and date
        public bool Death(String mark, int gen)
        {
            if (mark != null)
            {
                this._deathMark = mark;
                this._deathData = gen;

                return true;
            }
            return false;
        }
        
        // mark death, then creture was kiiled by slayer
        public bool Death(Creature slayer, String mark, int gen)
        {
            if (mark != null && slayer != null)
            {
                this._deathMark = mark;
                this._deathData = gen;
                this._slayer = slayer;

                return true;
            }
            return false;
        }
        
        // creature kill someone
        public bool Kill(Creature victim)
        {
            if (victim != null)
            {
                string mark = "Killed by Creature";
                if (victim.Death(this, mark, victim.AliveData + victim.OldCount))
                    return true;
            }
            return false;
        }
        
        // move creture to victim and check possibillity to kill
        private bool Hunt(Creature victim)
        {
            // range creature to victim
            double slayerThrow = (this._position[0] - victim._position[0]) * (this._position[0] - victim._position[0]) + (this._position[1] - victim._position[1]) * (this._position[1] - victim._position[1]);
            
            // check speed > range t ovictiom
            if (slayerThrow <= (this._speed * this._speed))
            {
                Move(victim._position);
                // take energy from victiom to creature
                Absorption(victim);
                // kill victim
                Kill(victim);

                return true;
            }
            else
            {
                MoveTo(victim._position);
            }

            return false;
        }
        
        // take energy from victim to creature
        private bool Absorption <TLiveObject>(TLiveObject meal) where TLiveObject: LiveObject
        {   
            // check victim is not null
            if (meal != null)
            {   
                // sum meal require energy to creature energy
                this._energy += meal.RequireEnergy;
                return true;
            }
            return false;
        }
        
        // move creature to custom position
        private bool MoveTo(double[] position)
        {
            double dx = position[0] - this._position[0];
            double dy = position[1] - this._position[1];
            
            // find range from creature to position 
            double r = Math.Sqrt(dx * dx + dy * dy);
            
            // find result position with speed accounting
            double[] res = { this._position[0] + this._speed * dx, this._position[1] + this._speed * dy };

            this._position = res;
            return true;
        }
        
        // simple transfer cruature to position
        private bool Move(double[] position)
        {
            if (position != null)
            {
                this._position = position;
                return true;
            }
            return false;
        }
        
        // Active actions of creature by 1 step of generation 
        public bool Step(List<Food> foods, List<Creature> creatures, int gen)
        {   
            // find food around and try to absoption
            if (foods != null)
            {
                Absorption(FindClose<Food>(foods));
                return true;
            }
            
            // find creature around and hunt it
            if (creatures != null)
            {
                
                Hunt(FindClose<Creature>(creatures));
            }

            return false;
        }
        
        // find some type of objects around creture into creature sense range with min radius
        private TLiveObject FindClose<TLiveObject>(List<TLiveObject> objects) where TLiveObject: LiveObject
        {
            if (objects != null)
            {
                TLiveObject res = objects[0];
                double min = CheckRadius(res.Position);

                foreach (TLiveObject object_ in objects)
                {
                    // cheack object radius
                    double radius = CheckRadius(object_.Position);
                    if (min > radius)
                    {
                        min = radius;
                        res = object_;
                    }
                }
                return res;
            }
            return null;
        }
        
        // find radius from position to position
        private double CheckRadius(double[] position)
        {
            double dx = position[0] - this._position[0];
            double dy = position[1] - this._position[1];

            double r = Math.Sqrt(dx * dx + dy * dy);

            return r;
        }
    }
}
