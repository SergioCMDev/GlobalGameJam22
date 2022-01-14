using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    // [SerializeField] private UpdateCoinsInformationOnUIEvent _updateCoinsInformationOnUIEvent;
    // [SerializeField] private PlaySFXEvent _playSfxEvent;
    // private IPlayerModel _playerModel;
    //
    // private void Start()
    // {
    //     _playerModel = ServiceLocator.Instance.GetModel<IPlayerModel>();
    // }
    //
    // public void PlayerGotCollectible(PlayerGotCollectibleEvent playerGotCollectibleEvent)
    // {
    //     if (playerGotCollectibleEvent.collectible)
    //         Destroy(playerGotCollectibleEvent.collectible);
    //
    //     _playerModel.CurrentCoins++;
    //     _updateCoinsInformationOnUIEvent.numberOfCoins = _playerModel.CurrentCoins;
    //     _updateCoinsInformationOnUIEvent.Fire();
    //     _playSfxEvent.soundName = SfxSoundName.PlayerPickUpCoin;
    //     _playSfxEvent.Fire();
    // }
}