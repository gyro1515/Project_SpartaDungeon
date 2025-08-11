using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [Header("점프대 세팅")]
     [SerializeField] private Vector3 jumpForce = new Vector3(0, 10f, 0); // 점프 힘 설정

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody playerRigidbody = other.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                playerRigidbody.AddForce(jumpForce, ForceMode.Impulse);
            }
        }
    }
}
