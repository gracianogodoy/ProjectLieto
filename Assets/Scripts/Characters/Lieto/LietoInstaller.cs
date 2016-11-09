using Zenject;

namespace GG
{
    public class LietoInstaller : MonoInstaller<LietoInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<InputHandler>().AsSingle();
            Container.BindAllInterfaces<InputHandler>().To<InputHandler>().AsSingle();

            Container.Bind<Move>().AsSingle();
            Container.Bind<Jump>().AsSingle();
            Container.BindAllInterfaces<Jump>().To<Jump>().AsSingle();

            bindCharacterMotor();

            bindInputCommands();
            bindJumpCommands();
        }

        private void bindJumpCommands()
        {
            Container.BindCommand<Jump.JumpStartCommand>().ToNothing();/*To<Move>(m => () => { m.SetEnable(false); }).AsSingle();*/

            Container.BindCommand<Jump.JumpEndCommand>().ToNothing();/*To<Move>(m => () => { m.SetEnable(true); }).AsSingle();*/
        }

        private void bindInputCommands()
        {
            Container.BindCommand<InputCommands.MoveCommand, int>().To<Move>(m => m.OnMove).AsSingle();
            Container.BindCommand<InputCommands.JumpCommand>().To<Jump>(j => j.StartJump).AsSingle();
            Container.BindCommand<InputCommands.StopJumpCommand>().To<Jump>(j => j.StopJump).AsSingle();
            Container.BindCommand<InputCommands.AttackCommand>().ToNothing();
            Container.BindCommand<InputCommands.SwitchCommand>().ToNothing();
        }

        private void bindCharacterMotor()
        {
            var characterController2d = GetComponent<Prime31.CharacterController2D>();
            Container.BindInstance(characterController2d);
            Container.Bind<CharacterMotor>().AsSingle();
            Container.BindAllInterfaces<CharacterMotor>().To<CharacterMotor>().AsSingle();
        }
    }
}