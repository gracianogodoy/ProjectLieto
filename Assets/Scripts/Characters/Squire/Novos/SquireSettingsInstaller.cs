using UnityEngine;
using Zenject;

namespace GG
{
    [CreateAssetMenu(fileName = "SquireSettingsInstaller", menuName = "Installers/SquireSettingsInstaller")]
    public class SquireSettingsInstaller : ScriptableObjectInstaller<SquireSettingsInstaller>
    {
        public CharacterMotor.Settings CharacterMotorSettings;
        public Move.Settings MoveSettings;
        public FaceDirection.Settings FaceDirectionSettings;
        public Life.Settings LifeSettings;
        public SquireAI.Settings AISettings;
        public Pushback.Settings PushbackSettings;

        public override void InstallBindings()
        {
            Container.BindInstance(FaceDirectionSettings);
            Container.BindInstance(MoveSettings);
            Container.BindInstance(CharacterMotorSettings);
            Container.BindInstance(AISettings);
            Container.BindInstance(LifeSettings);
            Container.BindInstance(PushbackSettings);
        }
    }
}