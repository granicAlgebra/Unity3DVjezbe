using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _handPositiontime = 1;
    [SerializeField] private float _animationTime = 2f;
    private IkController _ikController;

    internal void Interact(IkController ikController)
    {
        _ikController = ikController;
        StartCoroutine(InteractCoroutine());
    }

    private IEnumerator InteractCoroutine()
    {
        _ikController.PlaceRightHand(_target, _handPositiontime);
        yield return new WaitForSeconds(_handPositiontime);
        _animator.SetTrigger("Play");
        yield return new WaitForSeconds(_animationTime);
        _ikController.ReturnRightHand(_handPositiontime);
    }
}
