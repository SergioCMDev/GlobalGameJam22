using UnityEditor.Tilemaps;
using UnityEngine;

namespace IA
{
    public class Enemy:MonoBehaviour
    {
        [SerializeField] private int mSpeed = 0;
        

        public void MoveTo(Vector3 position)
        {
            transform.position = Vector3.MoveTowards(transform.position, position, mSpeed * Time.deltaTime);
        }

        public void InitialPosition(Vector3 initial)
        {
            transform.position = initial;
        }
        
    }
}