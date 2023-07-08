using KimoTech.SuperCoroutines;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class TutuExecutor : MonoBehaviour
{
    private ObjectPool<TutuBullet> _BulletPool;

    public Transform Executor;
    public TutuBullet Bullet;
    public float TutuInterval = 300;
    public TutuExecutor[] Targets;

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
        while (this)
        {
            foreach(var target in Targets)
            {
                var bullet = _BulletPool.Get();
                bullet.SetTarget(target.Executor);
                bullet.AttackSucceededEvent.AddOnce(BulletAttackSucceeded);
            }

            yield return TimeTunnel.WaitForSeconds(TutuInterval / 1000);
        }
    }

    private void BulletAttackSucceeded(TutuBullet bullet)
    {
        _BulletPool.Release(bullet);
    }
}
