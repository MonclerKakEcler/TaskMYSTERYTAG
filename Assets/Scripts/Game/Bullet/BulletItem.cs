using UnityEngine;
using DG.Tweening;

public class BulletItem : MonoBehaviour, IPoolItem
{
    [SerializeField] private int _damage;
    [SerializeField] private float _speed;
    [SerializeField] private float _distance = 10f;

    private const string kTagAsteroid = "Asteroid";

    private bool _isActive;
    private Vector2 _initialPosition;

    public bool IsActive => _isActive;
    public int Damage => _damage;

    public void StateBullet(bool state)
    {
        _isActive = state;
        gameObject.SetActive(state);
    }

    public void Activate(Vector2 position)
    {
        _initialPosition = position;
        transform.position = _initialPosition;

        StateBullet(true);

        float targetY = transform.position.y + _distance;

        transform.DOMoveY(targetY, _distance / _speed)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                Deactivate();
            });
    }

    public void Deactivate()
    {
        transform.DOKill();
        StateBullet(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out AsteroidItem asteroidItem))
        {
            asteroidItem.TakeDamage(_damage);
            Deactivate();
        }
    }
}
