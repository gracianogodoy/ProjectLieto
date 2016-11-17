using Zenject;

namespace GG
{
    public class SquireResurrect : BaseResurrect, IInitializable
    {
        private SquireAI _ai;
        private Life _life;

        public SquireResurrect(SquireAI ai, LietoResurrectSignal resurrectSignal, CharacterMotor motor, Life life) : base(resurrectSignal, motor)
        {
            _ai = ai;
            _life = life;
        }

        protected override void onResurrect()
        {
            _ai.SetState(SquireAI.State.Idle);
            _life.Ressurect();
        }
    }
}