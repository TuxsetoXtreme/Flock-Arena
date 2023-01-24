using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class GunGunProjectile : Projectile
{
    [SerializeField] SingleShotGun gungun;
    [SerializeField] GameObject gunCamera;
    [SerializeField] float initialBoost;
    [SerializeField] float boostTime;
    private bool boostDone = false;
    [SerializeField] float vSens;
    [SerializeField] float hSens;
    Transform playerCamTransform;
    Camera playerCamera;

    private void Start()
    {
        boostTime += Time.time;
        AddSpeed(initialBoost);
        if (playerController != null)
        {
            playerCamTransform = playerController.camTransform;
            playerCamera = playerController.playerCamera;
            playerController.PlayerCamerasActive(false);
            gungun.playerController = playerController;
            playerController.camTransform = gunCamera.transform;
            playerController.playerCamera = gunCamera.GetComponent<Camera>();
            gungun.UpdateAmmo();
            playerController.canMove = false;
        }
        else
        {
            Destroy(gunCamera);
        }
    }
    private void FixedUpdate()
    {
        rb.velocity = transform.forward * speed;
    }

    private void Update()
    {
        if (playerController != null)
        {
            GunGunLook();
        }

        if(!boostDone && Time.time > boostTime)
        {
            boostDone = true;
            AddSpeed(-initialBoost);
        }
    }

    public void GunGunLook()
    {
        if (playerController.isPaused)
            return;
        if (Input.GetKey(KeyCode.S))
        {
            transform.Rotate(vSens, 0,0);
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.Rotate(-vSens, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, hSens, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, -hSens, 0);
        }
        if (Input.GetMouseButtonDown(1))
        {
            Explosion(exploDamage);
        }
        if(transform.rotation.z != 0)
        {
            gameObject.transform.eulerAngles = new Vector3(
                gameObject.transform.eulerAngles.x,
                gameObject.transform.eulerAngles.y,
                0);
        }
    }


    private void OnDestroy()
    {
        if (playerController != null)
        {
            playerController.SetAmmoText("0", "1", false);
            gunCamera.SetActive(false);
            playerController.camTransform = playerCamTransform;
            playerController.playerCamera = playerCamera;
            gungun.playerController.PlayerCamerasActive(true);
            playerController.canMove = true;
        }
    }
}
