namespace GG
{
    public class GolemResurrect : BaseResurrect
    {
        private EnemyAttackAI _attackAi;

        public GolemResurrect(LietoResurrectSignal resurrectSignal, CharacterMotor motor, Life life, EnemyAttackAI ai) : base(resurrectSignal, motor, life)
        {
            _attackAi = ai;
        }

        protected override void onResurrect()
        {
            _attackAi.SetEnable(true);
        }
    }
}