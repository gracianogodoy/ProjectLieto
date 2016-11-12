using Zenject;

namespace GG
{
    public class LietoInstaller : MonoInstaller<LietoInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInstance(gameObject).WithId(InjectId.Owner).AsSingle();

            Container.BindAllInterfacesAndSelf<WireEvents>().To<WireEvents>().AsSingle();

            Container.BindAllInterfacesAndSelf<InputHandler>().To<InputHandler>().AsSingle();

            Container.Bind<Move>().AsSingle();

            Container.BindAllInterfacesAndSelf<Jump>().To<Jump>().AsSingle();
            Container.BindAllInterfacesAndSelf<FaceDirection>().To<FaceDirection>().AsSingle();
            Container.BindAllInterfacesAndSelf<Attack>().To<Attack>().AsSingle();
            Container.BindAllInterfacesAndSelf<Strike>().To<Strike>().AsSingle();


            Container.BindAllInterfacesAndSelf<LietoDeath>().To<LietoDeath>().AsSingle();

            LifeInstaller.Install(Container, gameObject);
            CharacterMotorInstaller.Install(Container, gameObject);
        }

        public class WireEvents : IInitializable
        {
            [Inject]
            private Move _move;
            [Inject]
            private Jump _jump;
            [Inject]
            private Attack _attack;
            [Inject]
            private FaceDirection _faceDirection;
            [Inject]
            private InputHandler _input;

            public void Initialize()
            {
                wireInputs();
                wireJump();
            }

            private void wireInputs()
            {
                _input.OnMove += _move.OnMove;
                _input.OnMove += _faceDirection.SetDirection;
                _input.OnJump += _jump.StartJump;
                _input.OnAttack += _attack.OnAttack;
                _input.OnStopJump += _jump.StopJump;
            }

            private void wireJump()
            {
                _jump.OnJump += () => _attack.SetEnable(false);
                _jump.OnStopJump += () => _attack.SetEnable(true);
            }
        }
    }
}