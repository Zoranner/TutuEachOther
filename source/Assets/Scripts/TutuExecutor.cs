using KimoTech.SuperCoroutines;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public class TutuExecutor : MonoBehaviour
{
    private ObjectPool<TutuBullet> _BulletPool;

    public Transform Executor;
    public TutuBullet Bullet;

    private bool _Trigger = false;
    public float Delay = 5;
    public float Interval = 1;
    public float Duration = 10;
    public TutuExecutor[] Targets;

    public UnityEvent Executed;

    private TutuScene _Scene;
    public TutuScene Scene
    {
        get
        {
            if (_Scene == null)
            {
                _Scene = GetComponentInParent<TutuScene>();
            }

            return _Scene;
        }
    }

    private void Awake()
    {
        _BulletPool = new ObjectPool<TutuBullet>(
            PoolObjectCreated,
            PoolObjectGeted,
            PoolObjectReleased,
            PoolObjectDestroyed, true, 50, 1000);
    }

    private void Start()
    {
        TimeTunnel.RunCoroutineSingleton(TutuEachOtherCoroutine(), Segment.Update, gameObject, "TutuEachOtherCoroutine", SingletonBehavior.Abort);
    }

    public void Gogogo()
    {
        Debug.Log($"{name}");
        _Trigger = true;
    }

    private TutuBullet PoolObjectCreated()
    {
        return Instantiate(Bullet, transform);
    }

    private void PoolObjectGeted(TutuBullet bullet)
    {
        bullet.transform.position = Executor.position;
        bullet.transform.localScale = Vector3.one;
        bullet.gameObject.SetActive(true);
    }

    private void PoolObjectReleased(TutuBullet bullet)
    {
        bullet.gameObject.SetActive(false);
        bullet.transform.position = Executor.position;
        bullet.transform.localScale = Vector3.one;
    }

    private void PoolObjectDestroyed(TutuBullet bullet)
    {
        Destroy(bullet);
    }

    private IEnumerator<float> TutuEachOtherCoroutine()
    {
        if (Delay < 0)
        {
            while (!_Trigger)
            {
                yield return TimeTunnel.WaitForOneFrame;
            }
        }
        else
        {
            yield return TimeTunnel.WaitForSeconds(Delay);
        }

        var startTime = Time.time;
        while (Time.time - startTime < Duration)
        {
            foreach (var target in Targets)
            {
                if (!target.gameObject.activeSelf)
                {
                    continue;
                }

                var bullet = _BulletPool.Get();
                bullet.SetTarget(target.Executor);
                bullet.AttackSucceededEvent.AddOnce(BulletAttackSucceeded);
            }

            yield return TimeTunnel.WaitForSeconds(Interval);
        }

        Executed?.Invoke();
    }

    private void BulletAttackSucceeded(TutuBullet bullet)
    {
        _BulletPool.Release(bullet);
    }
}
