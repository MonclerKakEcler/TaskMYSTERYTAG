using UnityEngine;
using Zenject;

public class GameEntryPoint : MonoBehaviour
{
    [SerializeField] private LevelManagerView _levelManagerView;
    [SerializeField] private ShipView _shipView;
    [SerializeField] private HeaderView _headerView;

    private LevelManagerController _levelManagerController;
    private ShipController _shipController;
    private HeaderController _headerController;
    private ILevelService _levelService;

    [Inject]
    public void Construct(LevelManagerController levelManagerController, 
                          ShipController shipController,
                          HeaderController headerController,
                          ILevelService levelService)
    {
        _levelManagerController = levelManagerController;
        _shipController = shipController;
        _headerController = headerController;
        _levelService = levelService;
    }

    private void Start()
    {
        InitializeControllers();
    }

    public async void InitializeControllers()
    {
        await _levelService.Init();
        _levelManagerController.Init(_levelManagerView);
        _headerController.Init(_headerView);
        _shipController.Init(_shipView);
    }
}
