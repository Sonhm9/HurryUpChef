using UnityEngine;
using DG.Tweening;
using System;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Vector3 chefCameraPos;
    [SerializeField]
    private Vector3 waiterCameraPos;
    [SerializeField]
    private Transform[] wallContainers;

    private Renderer[] wallRenderers;

    public void SwitchCamera(bool isFocusedChef, Action onCallback)
    {
        SetToOpacityWalls();
        int index = Convert.ToByte(isFocusedChef);
        wallRenderers = wallContainers[index].GetComponentsInChildren<Renderer>();
        SetToTranslucentWalls();

        if (isFocusedChef)
        {
            Camera.main.transform.DOMove(chefCameraPos, 1f).OnComplete(() => onCallback?.Invoke());
            return;
        }

        Camera.main.transform.DOMove(waiterCameraPos, 1f).OnComplete(() => onCallback?.Invoke());
    }

    private void SetToTranslucentWalls()
    {
        if (wallRenderers == null || wallRenderers.Length <= 0)
        {
            return;
        }

        foreach (Renderer wall in wallRenderers)
        {
            Color color = wall.material.color;
            color.a = 0.2f;
            wall.material.color = color;
        }
    }

    private void SetToOpacityWalls()
    {
        if (wallRenderers == null || wallRenderers.Length <= 0)
        {
            return;
        }

        foreach (Renderer wall in wallRenderers)
        {
            wall.material.color = Color.white;
            wallRenderers = null;
        }
    }
}
