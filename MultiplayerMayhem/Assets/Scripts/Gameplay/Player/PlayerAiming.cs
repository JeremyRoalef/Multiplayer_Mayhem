using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

//Network behavior
public class PlayerAiming : NetworkBehaviour
{
    [SerializeField]
    InputReaderSO inputReaderSO;

    [SerializeField]
    Transform turretTransform;

    //Late update due to game setup
    private void LateUpdate()
    {
        if (!IsOwner) { return; }

        Vector2 aimScreenPosition = inputReaderSO.AimPosition;
        Vector2 aimWorldPosition = Camera.main.ScreenToWorldPoint(aimScreenPosition);

        turretTransform.up = new Vector2(
            aimWorldPosition.x-turretTransform.position.x, 
            aimWorldPosition.y-turretTransform.position.y
            );


    }
}
