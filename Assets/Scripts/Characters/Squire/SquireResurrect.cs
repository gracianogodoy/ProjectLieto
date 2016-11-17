using Zenject;

namespace GG
{
    public class SquireResurrect : BaseResurrect, IInitializable
    {
        private SquireAI _ai;

        public SquireResurrect(SquireAI ai, LietoResurrectSignal resurrectSignal, CharacterMotor motor, Life life) : base(resurrectSignal, motor, life)
        {
            _ai = ai;
        }

        protected override void onResurrect()
        {
            _ai.SetState(SquireAI.State.Idle);
        }
    }
}