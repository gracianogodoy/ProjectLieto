using UnityEngine;
using Zenject;

namespace GG
{
    [CreateAssetMenu(fileName = "BossSettingsInstaller", menuName = "Installers/BossSettingsInstaller")]
    public class BossSettingsInstaller : ScriptableObjectInstaller<BossSettingsInstaller>
    {
        public BossAI.Settings BossAISettings;
        public Life.Settings LifeSettings;

        public override void InstallBindings()
        {
            Container.BindInstance(BossAISettings);
            Container.BindInstance(LifeSettings);
        }
    }
}