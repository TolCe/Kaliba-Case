using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Driver : DynamicLevelItem
{
    [SerializeField] private DriverAnimation _driverAnimation;

    public async Task Move(List<TileElement> targetTileList)
    {
        _driverAnimation.ToggleRun(true);
        await MoveItem(targetTileList);
        _driverAnimation.ToggleRun(false);
    }

    public void SelectDriver()
    {
        HighlightItem();
        _driverAnimation.ToggleSelected(true);
    }

    public void DeselectDriver()
    {
        _driverAnimation.ToggleSelected(false);
        RemoveHighlight();
    }

    public void EnterVehicle()
    {
        _driverAnimation.VehicleEnter();
    }
}
