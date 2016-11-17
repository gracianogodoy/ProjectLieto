namespace GG
{
    public class GhoulResurrect : BaseResurrect
    {
        private GhoulAI _ghoulAi;

        public GhoulResurrect(GhoulAI ai, LietoResurrectSignal resurrectSignal, CharacterMotor motor, Life life) : base(resurrectSignal, motor, life)
        {
            _ghoulAi = ai;
        }

        protected override void onResurrect()
        {
            _ghoulAi.SetState(GhoulAI.State.Reseting);
        }
    }
}