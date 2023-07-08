using KimoTech.SuperEvents;
using UnityEngine;

public class TutuBullet : MonoBehaviour
{
    private Transform _Target;

    public float MoveVelocity = 1;
    public Vector3 ScaleVelocity = Vector3.one;
    public float AttackRange = 1;

    public SuperEvent<TutuBullet> AttackSucceededEvent { get; } = new SuperEvent<TutuBullet>();

    public void SetTarget(Transform target)
    {
        _Target = target;
    }

    private void Update()
    {
        if (!_Target)
        {
            return;
        }

        var distance = Vector3.Distance(transform.position, _Target.position);
        if (distance < AttackRange)
        {
            AttackSucceededEvent?.Dispatch(this);
            return;
        }

        var progress = MoveVelocity * Time.deltaTime / distance;
        transform.position = Vector3.Lerp(transform.position, _Target.position, progress);
        transform.localScale = new Vector3(
            transform.localScale.x + ScaleVelocity.x,
            transform.localScale.y + ScaleVelocity.y,
            transform.localScale.z + ScaleVelocity.z);
    }
}
