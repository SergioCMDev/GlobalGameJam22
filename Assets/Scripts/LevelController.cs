using Application_;
using Presentation;
using Presentation.Menus;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private CanvasPresenter _canvasPresenter;

    [SerializeField] private BuyController _buyController;
    [SerializeField] private GridBuildingManager _gridBuildingManager;
    // Start is called before the first frame update
    void Start()
    {
        _canvasPresenter.OnPlayerWantsToSetBuildingInGrid += PlayerWantsToSetBuildingInGrid;
        _gridBuildingManager.OnPlayerHasSetBuildingOnGrid += PlayerHasSetBuildingInGrid;
        _gridBuildingManager.OnPlayerHasCanceledSetBuildingOnGrid += PlayerHasCanceledSetBuildingInGrid;
    }

    private void PlayerHasCanceledSetBuildingInGrid()
    {
        _canvasPresenter.SetBuildingSelectableStatus(true);
        _buyController.BuyHasBeenCanceled();
    }

    private void PlayerHasSetBuildingInGrid()
    {
        _canvasPresenter.SetBuildingSelectableStatus(true);
        _buyController.EndBuy();
    }

    private void PlayerWantsToSetBuildingInGrid(BuildingType buildingType)
    {
        _buyController.PlayerWantsToBuyBuilding(buildingType);
    }

}
