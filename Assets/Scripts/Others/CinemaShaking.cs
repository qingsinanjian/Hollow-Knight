using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemaShaking : MonoBehaviour
{
    private Cinemachine.CinemachineImpulseSource _impulseSource;
    private void Start()
    {
        _impulseSource = GetComponent<Cinemachine.CinemachineImpulseSource>();
        _impulseSource.enabled = false;
    }

    public void CinemaShake()
    {
        _impulseSource.GenerateImpulse();
    }
}
