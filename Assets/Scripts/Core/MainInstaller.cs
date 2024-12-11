using Zenject;

public class MainInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        PoolBullets();
        BindLevelData();
        BindStatistic();

        Container.Bind<ShipController>().To<ShipController>().AsSingle();
        Container.Bind<LevelManagerController>().To<LevelManagerController>().AsSingle();
        Container.Bind<HeaderController>().To<HeaderController>().AsSingle();
    }

    private void PoolBullets()
    {
        Container.Bind<IObjectPool<BulletItem>>().To<ObjectPool<BulletItem>>().AsSingle();
    }

    private void BindLevelData()
    {
        Container.Bind<ILevelService>().To<LevelService>().AsSingle();
        Container.Bind<ILevelProvider>().To<LevelProvider>().AsSingle();
    }

    private void BindStatistic()
    {
        Container.Bind<StatisticScreenController>().To<StatisticScreenController>().AsSingle();
        Container.Bind<IStatisticService>().To<StatisticService>().AsSingle();
        Container.Bind<IStatisticProvider>().To<StatisticProvider>().AsSingle();
    }
}
