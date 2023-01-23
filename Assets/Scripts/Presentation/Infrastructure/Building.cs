using UnityEngine;

namespace Presentation.Infrastructure
{
    public class Building : MonoBehaviour
    {
        [SerializeField] protected SpriteRenderer _spriteRenderer;

        [SerializeField] protected float _alphaWhenSelected = 0.475f;
        private int _id, _level;
        private bool _placed;
        protected Color originalColor;
        protected Color colorWithTransparency;
        private Vector3Int _area;
        public Vector3Int Area => _area;

        protected SpriteRenderer SpriteRenderer => _spriteRenderer;
        
        private void Awake()
        {
            var color = SpriteRenderer.color;
            
            originalColor = color;
            colorWithTransparency = new Color(color.r, color.g, color.b, _alphaWhenSelected);
            _area = new Vector3Int(1, 1, 1);
        }

        protected internal virtual void Initialize()
        {
            Awake();
        }
    }
}