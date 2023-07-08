using KimoTech.SuperEvents;
using UnityEngine;

public class TutuBullet : MonoBehaviour
{
    private Vector3 _TargetPosition;

    public float MoveVelocity = 1;
    public Vector3 ScaleVelocity = Vector3.one;
    public float AttackRange = 1;

    public SuperEvent<TutuBullet> AttackSucceededEvent { get; } = new SuperEvent<TutuBullet>();

    public void SetTarget(Transform target)
    {
        _TargetPosition = target.position;
        transform.LookAt(target);
    }

    private void Update()
    {
        if (!gameObject.activeSelf)
        {
            return;
        }

        var distance = Vector3.Distance(transform.position, _TargetPosition);
        if (distance < AttackRange)
        {
            AttackSucceededEvent?.Dispatch(this);
            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position, _TargetPosition, 
            MoveVelocity * Time.deltaTime);
        transform.localScale = new Vector3(
            transform.localScale.x + ScaleVelocity.x,
            transform.localScale.y + ScaleVelocity.y,
            transform.localScale.z + ScaleVelocity.z);
    }
}
