using UnityEngine;

namespace App
{
    public class TileEntity : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        public bool Walkable { get; set; }
    
        protected Color GetOriginalColor()
        {
            return _spriteRenderer.color;
        }

        protected void HideSprite()
        {
            _spriteRenderer.gameObject.SetActive(false);
        }
    
        protected void ShowSprite()
        {
            _spriteRenderer.gameObject.SetActive(true);
        }
        protected void ChangeColour(Color color)
        {
            color.a = 1;
            _spriteRenderer.color = color;
        }
    }
}