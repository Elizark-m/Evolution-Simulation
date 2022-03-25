using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace BasicEvolved
{

    class Creature : LiveObject
    {
        private Random r;
        private double _genSpeed = 1;

        private int _oldCount = 0;
        public int OldCount { get => _oldCount; }

        private double _energy;
        public double Energy { get => _energy; }

        private double _reqEnergy;

        private List<double> _evoSize;
        private List<double> _evoSpeed;
        private List<double> _evoSense;

        private double _size;
        public double Size { get => this._size; }
        private double _speed;
        public double Speed { get => this._speed; }
        private double _sense;
        public double Sense { get => this._sense; }

        private double[] _termination;
        public double[] Termination { get => _termination; }

        private int _aliveData = -1;
        public int AliveData { get => this._aliveData; }

        private int _deathData = -1;
        public int DeathData { get => this._deathData; }

        private string _deathMark;
        public string DeathMark { get => this._deathMark; }

        private List<Creature> _victims = new List<Creature>();
        public List<Creature> Victims { get => this._victims; }

        private Creature _slayer;
        public Creature Slayer { get => this._slayer; }

        public Creature()
        {
            this._aliveData = 1;
            this._size = 1;
            this._speed = 1;
            this._sense = 1;
        }

        private Creature(Creature parent, int gen)
        {
            this._aliveData = gen;

            if (parent != null)
            {
                _evoSize.Add(MutateGen());
                this._size = parent._size + _evoSize.Last();

                _evoSpeed.Add(MutateGen());
                this._speed = parent._speed + _evoSpeed.Last();

                _evoSense.Add(MutateGen());
                this._sense = parent._sense + _evoSense.Last();

                this._reqEnergy = this._size * this._size * this._size * this._speed * this._speed + this._sense;
            }
        }

        private double MutateGen()
        {
            if (r.NextDouble() > 0.7)
                return -_genSpeed;
            else if (r.NextDouble() > 0.5)
                return _genSpeed;
            return 0;
        }

        public bool Starve(int gen)
        {
            if (_energy < _reqEnergy)
            {
                Death("Starved", gen);
                return true;
            }
            return false;
        }

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

        private bool Hunt(Creature victim)
        {
            double slayerThrow = (this._position[0] - victim._position[0]) * (this._position[0] - victim._position[0]) + (this._position[1] - victim._position[1]) * (this._position[1] - victim._position[1]);

            if (slayerThrow <= (this._speed * this._speed))
            {
                Move(victim._position);
                Absorption(victim);
                Kill(victim);

                return true;
            }
            else
            {
                MoveTo(victim._position);
            }

            return false;
        }

        private bool Absorption <TLiveObject>(TLiveObject meal) where TLiveObject: LiveObject
        {
            if (meal != null)
            {
                this._energy += meal.RequireEnergy;
                return true;
            }
            return false;
        }

        private bool MoveTo(double[] position)
        {
            double dx = position[0] - this._position[0];
            double dy = position[1] - this._position[1];

            double r = Math.Sqrt(dx * dx + dy * dy);

            double[] res = { this._position[0] + this._speed * dx, this._position[1] + this._speed * dy };

            this._position = res;
            return true;
        }

        private bool Move(double[] position)
        {
            if (position != null)
            {
                this._position = position;
                return true;
            }
            return false;
        }

        public bool Step(List<Food> foods, List<Creature> creatures, int gen)
        {
            if (foods != null)
            {
                Absorption(FindClose<Food>(foods));
                return true;
            }

            if (creatures != null)
            {
                
                Hunt(FindClose<Creature>(creatures));
            }

            return false;
        }

        private TLiveObject FindClose<TLiveObject>(List<TLiveObject> objects) where TLiveObject: LiveObject
        {
            if (objects != null)
            {
                TLiveObject res = objects[0];
                double min = CheckRadius(res.Position);

                foreach (TLiveObject object_ in objects)
                {
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

        private double CheckRadius(double[] position)
        {
            double dx = position[0] - this._position[0];
            double dy = position[1] - this._position[1];

            double r = Math.Sqrt(dx * dx + dy * dy);

            return r;
        }
        
        



    }
}