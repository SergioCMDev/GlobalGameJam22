using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    // [SerializeField] private GameObject _actualWeaponPrefab;
    [SerializeField] private LayerMask _layerMaskToAttack;
    private IPlayerModel _playerModel;
    private Vector3 _attackDirection;
    private bool _selectAtFirstGun, _selectAtFirstkatana;

    private float currentWeaponId = 0;
    private Rigidbody _rigidBody;
    private Quaternion _rotation;

    // private GameObject _currentWeaponInstance;

    // public Weapon Weapon => weapon;

    public void Init(IPlayerModel playerModel)
    {
        _playerModel = playerModel;
    }


    public void AttackLeft(Vector3 vectorOfMovement)
    {
        if (!CanAttack()) return;
        if (vectorOfMovement.magnitude == 0)
        {
            vectorOfMovement = transform.forward;
        }


        // Quaternion rotation = Quaternion.AngleAxis(VectorUtils.GetRotationOfDirection(transform, vectorOfMovement),
        //     Vector3.forward);
        _attackDirection = vectorOfMovement;
    }

    private bool CanAttack()
    {
        return true;
    }


    public void AttackRight(Vector3 vectorToShowDirectionOfMovement)
    {
        Debug.Log("AttackRight");
    }

    private void OnDrawGizmos()
    {
    }
}