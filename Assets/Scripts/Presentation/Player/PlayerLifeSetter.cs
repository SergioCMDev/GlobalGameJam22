using Application_.Models;
using UnityEngine;

namespace Presentation.Player
{
    public class PlayerLifeSetter : LifeSetter
    {
        private IPlayerModel _playerModel;
        // [Header("Events")] [SerializeField] private ChangeLifeUIEvent changeLifeUIEvent;
        // [SerializeField] private PlayerIsDeadEvent _playerIsDeadEvent;
        // [SerializeField] private UIPauseGameEvent uiPauseGameEvent;

        public void Init(IPlayerModel playerModel)
        {
            // _characterFacade = characterFacade;
            _playerModel = playerModel;
        }


        public override void ReceiveDamage(GameObject itemWhichHit, float receivedDamage)
        {
            RemoveLife(receivedDamage);
        }

        // //Used By Event
        // public void PlayerHasReceivedDamage(PlayerHasReceivedDamageEvent playerHasReceivedDamageEvent)
        // {
        //     ReceiveDamage(playerHasReceivedDamageEvent.damageReceived);
        // }
        //
        // //Used By Event
        // public void PlayerHasReceivedCuration(PlayerHasReceivedCurationEvent playerHasReceivedCurationEvent)
        // {
        //     AddLife(playerHasReceivedCurationEvent.curationReceived);
        // }


        public override void ReceiveDamage(float receivedDamage)
        {
            RemoveLife(receivedDamage);
        }

        public override void AddLife(float lifeToAdd)
        {
            _playerModel.CurrentLife += lifeToAdd;
            Mathf.Clamp(_playerModel.CurrentLife, 0, _playerModel.MaxLife);
            UpdatePlayerLife();
        }

        private void RemoveLife(float receivedDamage)
        {
            _playerModel.CurrentLife -= receivedDamage;
            Mathf.Clamp(_playerModel.CurrentLife, 0, _playerModel.MaxLife);

            UpdatePlayerLife();
            if (_playerModel.CurrentLife <= 0)
            {
                PlayerIsDead();
            }
        }

        private void PlayerIsDead()
        {
            Debug.Log("Player Death");
            // _playerIsDeadEvent.Fire();
            // uiPauseGameEvent.Fire();
        }




        private void UpdatePlayerLife()
        {
            // changeLifeUIEvent.newLifeValue = _playerModel.CurrentLife;
            // changeLifeUIEvent.Fire();
        }
    }
}