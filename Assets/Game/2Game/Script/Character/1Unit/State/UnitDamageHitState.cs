using UnityEngine;
using System.Collections;

public class UnitDamageHitState : MonoBehaviour, IState<Unit> // IState<Unit> ����
{
    private Unit unit; // _unitContext -> unit���� ������ ����
    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;

    [Header("Hit Effect Settings")]
    [SerializeField] private Color hitColor = Color.red;
    [SerializeField] private float hitDuration = 0.2f;
    [SerializeField] private float flashInterval = 0.05f;
    [SerializeField] private float knockbackForce = 1f;

    private Coroutine _hitEffectCoroutine;

    public void Handle(Unit context) // �Ķ���� context�� Ÿ���� Unit ����
    {
        unit = context; // ������ ����
        _spriteRenderer = unit.GetComponentInChildren<SpriteRenderer>(); // ������ ����
        if (_spriteRenderer != null)
        {
            _originalColor = _spriteRenderer.color;
        }
        else
        {
            Debug.LogWarning($"UnitDamageHitState: No SpriteRenderer found on {unit.gameObject.name}. Hit effect will not work."); // ������ ����
        }
    }

    public void Enter()
    {
        if (unit == null) return; // ������ ����

        //unit.Animator?.SetTrigger("3_Damagehit"); // ������ ����

        if (_hitEffectCoroutine != null)
        {
            StopCoroutine(_hitEffectCoroutine);
        }
        _hitEffectCoroutine = StartCoroutine(HitEffectCoroutine());

        Rigidbody2D rb = unit.GetComponent<Rigidbody2D>(); // ������ ����
        if (rb != null && unit.Target != null) // ������ ����
        {
            Vector2 knockbackDirection = (Vector2)(unit.transform.position - unit.Target.position).normalized; // ������ ����
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
        else if (rb != null)
        {
            Debug.LogWarning($"Knockback applied without a specific target direction for {unit.gameObject.name}."); // ������ ����
        }
    }

    public void Action() { }

    public void Exit()
    {
        if (unit == null) return; // ������ ����

        if (_hitEffectCoroutine != null)
        {
            StopCoroutine(_hitEffectCoroutine);
        }
        if (_spriteRenderer != null)
        {
            _spriteRenderer.color = _originalColor;
        }
    }

    private IEnumerator HitEffectCoroutine()
    {
        float timer = 0f;
        while (timer < hitDuration)
        {
            if (_spriteRenderer != null)
            {
                _spriteRenderer.color = hitColor;
            }
            yield return Utils.WaitForSecond(flashInterval);

            if (_spriteRenderer != null)
            {
                _spriteRenderer.color = _originalColor;
            }
            yield return Utils.WaitForSecond(flashInterval);

            timer += (flashInterval * 2);
        }

        if (_spriteRenderer != null)
        {
            _spriteRenderer.color = _originalColor;
        }

        unit.UnitStateManager?.ChangeState(UnitState.IDLE); // ������ ����
    }
}