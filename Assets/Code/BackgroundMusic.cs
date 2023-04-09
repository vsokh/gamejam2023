using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic _obj;

    private void Awake()
    {
        if (_obj == null)
        {
            _obj = this;
            DontDestroyOnLoad(_obj);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
