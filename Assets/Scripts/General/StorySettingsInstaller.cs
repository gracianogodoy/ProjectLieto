using UnityEngine;
using Zenject;

namespace GG
{
    [CreateAssetMenu(fileName = "StorySettingsInstaller", menuName = "Installers/StorySettingsInstaller")]
    public class StorySettingsInstaller : ScriptableObjectInstaller<StorySettingsInstaller>
    {
        public StoryFlow.Settings StoryFlowSettings;

        public override void InstallBindings()
        {
            Container.BindInstance(StoryFlowSettings);
        }
    }
}