using UnityEngine;
using System.Collections;

public class UnitDamageHitState : MonoBehaviour, IState<Unit> // IState<Unit> 유지
{
    private Unit unit; // _unitContext -> unit으로 변수명 변경
    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;

    [Header("Hit Effect Settings")]
    [SerializeField] private Color hitColor = Color.red;
    [SerializeField] private float hitDuration = 0.2f;
    [SerializeField] private float flashInterval = 0.05f;
    [SerializeField] private float knockbackForce = 1f;

    private Coroutine _hitEffectCoroutine;

    public void Handle(Unit context) // 파라미터 context의 타입은 Unit 유지
    {
        unit = context; // 변수명 변경
        _spriteRenderer = unit.GetComponentInChildren<SpriteRenderer>(); // 변수명 변경
        if (_spriteRenderer != null)
        {
            _originalColor = _spriteRenderer.color;
        }
        else
        {
            Debug.LogWarning($"UnitDamageHitState: No SpriteRenderer found on {unit.gameObject.name}. Hit effect will not work."); // 변수명 변경
        }
    }

    public void Enter()
    {
        if (unit == null) return; // 변수명 변경

        //unit.Animator?.SetTrigger("3_Damagehit"); // 변수명 변경

        if (_hitEffectCoroutine != null)
        {
            StopCoroutine(_hitEffectCoroutine);
        }
        _hitEffectCoroutine = StartCoroutine(HitEffectCoroutine());

        Rigidbody2D rb = unit.GetComponent<Rigidbody2D>(); // 변수명 변경
        if (rb != null && unit.Target != null) // 변수명 변경
        {
            Vector2 knockbackDirection = (Vector2)(unit.transform.position - unit.Target.position).normalized; // 변수명 변경
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
        else if (rb != null)
        {
            Debug.LogWarning($"Knockback applied without a specific target direction for {unit.gameObject.name}."); // 변수명 변경
        }
    }

    public void Action() { }

    public void Exit()
    {
        if (unit == null) return; // 변수명 변경

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

        unit.UnitStateManager?.ChangeState(UnitState.IDLE); // 변수명 변경
    }
}