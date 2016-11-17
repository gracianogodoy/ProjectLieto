using UnityEngine;
using Zenject;

namespace GG
{
    [CreateAssetMenu(fileName = "GhoulSettingsInstaller", menuName = "Installers/GhoulSettingsInstaller")]
    public class GhoulSettingsInstaller : ScriptableObjectInstaller<GhoulSettingsInstaller>
    {
        public Attack.Settings AttackSettings;
        public CharacterMotor.Settings CharacterMotorSettings;
        public FaceDirection.Settings FaceDirectionSettings;
        public Life.Settings LifeSettings;
        public Strike.Settings StrikeSettings;
        public EnemyAttackAI.Settings EnemyAttackAISettings;
        public Pushback.Settings PushbackSettings;
        public GhoulAI.Settings GhoulAISettings;
        public GhoulDeath.Settings GhoulDeathSettings;

        public override void InstallBindings()
        {
            Container.BindInstance(FaceDirectionSettings);
            Container.BindInstance(CharacterMotorSettings);
            Container.BindInstance(LifeSettings);
            Container.BindInstance(PushbackSettings);
            Container.BindInstance(AttackSettings);
            Container.BindInstance(StrikeSettings);
            Container.BindInstance(EnemyAttackAISettings);
            Container.BindInstance(GhoulAISettings);
            Container.BindInstance(GhoulDeathSettings);
        }
    } 
}