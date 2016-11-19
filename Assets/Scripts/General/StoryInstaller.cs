using UnityEngine;
using Zenject;

namespace GG
{
    public class StoryInstaller : MonoInstaller<StoryInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindAllInterfacesAndSelf<StoryFlow>().To<StoryFlow>().AsSingle();
            Container.BindAllInterfacesAndSelf<CountDeadSquires>().To<CountDeadSquires>().AsSingle();
        }
    } 
}