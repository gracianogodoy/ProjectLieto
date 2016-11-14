using UnityEngine;
using Zenject;

namespace GG
{
    [CreateAssetMenu(fileName = "GolemSettingsInstaller", menuName = "Installers/GolemSettingsInstaller")]
    public class GolemSettingsInstaller : ScriptableObjectInstaller<GolemSettingsInstaller>
    {
        public Attack.Settings AttackSettings;
        public FaceDirection.Settings FaceDirectionSettings;
        public Life.Settings LifeSettings;
        public Strike.Settings StrikeSettings;
        public EnemyAttackAI.Settings EnemyAttackAISettings;

        public override void InstallBindings()
        {
            Container.BindInstance(AttackSettings);
            Container.BindInstance(FaceDirectionSettings);
            Container.BindInstance(LifeSettings);
            Container.BindInstance(StrikeSettings);
            Container.BindInstance(EnemyAttackAISettings);
        }
    }
}