using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    bool _dead;

    [SerializeField] int _maxHealth;
    [SerializeField] int _currentHealth;

    [Space]

    public UnityEvent OnDeath;

	void Start()
	{
        _currentHealth = _maxHealth;
	}

	public void TakeDamage(int damage)
    {
        if (_dead) return;

        damage = Mathf.Abs(damage);

        _currentHealth -= damage;
        if (_currentHealth <= 0) Die();
    }

	public void Heal(int health)
    {
        if (_dead) return;

        health = Mathf.Abs(health);

        _currentHealth += health;
        if (_currentHealth > _maxHealth) _currentHealth = _maxHealth;
    }

    public void Die()
    {
        _dead = true;
        OnDeath?.Invoke();
    }
}
