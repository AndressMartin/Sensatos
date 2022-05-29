using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalaSeguranca : MonoBehaviour
{
    [SerializeField] Enemy inimigo;
    GeneralManagerScript generalManagerScript;

    bool camerasLigadas;
    bool camerasLigadasRespawn;
    void Start()
    {
        camerasLigadas = true;
        generalManagerScript = FindObjectOfType<GeneralManagerScript>();

        SetRespaw();
    }

    void Update()
    {
        if (!camerasLigadas)
        {
            return;
        }
        if(inimigo.IsMorto())
        {
            LigarDeslgiarCameras(false);
            camerasLigadas = false;
        }
    }
    public void SetRespaw()
    {
        camerasLigadasRespawn = camerasLigadas;
    }
    public void Respawn()
    {
        camerasLigadas = camerasLigadasRespawn;
        LigarDeslgiarCameras(camerasLigadas);
    }
    public void LigarDeslgiarCameras(bool valor)
    {
        foreach (CameraDeSeguranca cameraDeSeguranca in generalManagerScript.ObjectManager.ListaDeCamerasLockdown)
        {
            cameraDeSeguranca.ReceberDesativarAtivarCamera(valor);
        }
    }
}
