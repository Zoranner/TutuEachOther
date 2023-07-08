using System.Collections.Generic;
using UnityEngine;

public class TutuScene : MonoBehaviour
{
    private readonly List<TutuExecutor> _Executors;

    public void Register(TutuExecutor executor)
    {
        _Executors.Add(executor);
    }
}
