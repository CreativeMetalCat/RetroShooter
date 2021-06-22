namespace RetroShooter.Engine
{
    /**
     * Base class for every object placeable in the world
     */
    public class BaseObject
    {
        /*
         * If this value is false this object will be removed from the game by the end of the update
         */
        protected bool pendingKill = false;
        
        /*
         * If this value is false this object will be removed from the game by the end of the update
         */
        public  bool Valid
        {
            get => !pendingKill;
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        protected string _name;

        public BaseObject(string name)
        {
            _name = name;
        }

        public  virtual void Init(){}
        
        /**
         * 
         * \p deltaTime  time since last frame
         */
        public  virtual void Update(float deltaTime){}

        public virtual void Destroy(){}
    }
}