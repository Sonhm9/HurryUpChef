using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ObjectDetector : MonoBehaviour
{
    public Shader highlightShader; // 강조 셰이더
    public Shader transparentShader; // 투명 셰이더
    public Transform player; // 플레이어 또는 기준점
    private Dictionary<Renderer, Shader> originalShaders = new Dictionary<Renderer, Shader>();
    private List<Collider> detectedObjects = new List<Collider>(); // 트리거 내의 오브젝트 리스트
    private Renderer[] currentHighlightedRenderers; // 현재 강조된 오브젝트와 하위 오브젝트의 렌더러 배열

    public Action<Transform> OnInteractableObject; // 강조된 오브젝트를 전달하는 콜백

    private void OnTriggerEnter(Collider other)
    {
        if (!IsValidTag(other.tag)) return;

        // 트리거 내의 오브젝트 리스트에 추가
        if (!detectedObjects.Contains(other))
        {
            detectedObjects.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!detectedObjects.Contains(other)) return;

        // 트리거 리스트에서 제거
        detectedObjects.Remove(other);

        // 나간 오브젝트가 현재 강조된 오브젝트라면 복구
        if (currentHighlightedRenderers != null && currentHighlightedRenderers.Contains(other.GetComponent<Renderer>()))
        {
            ResetAllShaders();
            currentHighlightedRenderers = null;
            OnInteractableObject?.Invoke(null); // 강조 해제 알림
        }
    }

    private void Update()
    {
        RemoveDestroyedObjects(); // 삭제된 오브젝트 제거
        HighlightClosestObject(); // 매 프레임 가장 가까운 오브젝트를 강조
    }

    private void HighlightClosestObject()
    {
        // 현재 강조 초기화 (필요할 때만)
        if (currentHighlightedRenderers != null)
        {
            Collider closestObject = detectedObjects
                .Where(obj => obj != null && obj.gameObject != null)
                .OrderBy(obj => Vector3.Distance(player.position, obj.transform.position))
                .FirstOrDefault();

            // 강조된 오브젝트가 변경되지 않았다면 갱신 중단
            if (currentHighlightedRenderers[0] != null && closestObject != null && currentHighlightedRenderers[0].gameObject == closestObject.gameObject)
            {
                return;
            }

            ResetAllShaders();
            currentHighlightedRenderers = null;
        }

        if (detectedObjects.Count == 0)
        {
            OnInteractableObject?.Invoke(null); // 강조된 오브젝트가 없음을 알림
            return;
        }

        // 가장 가까운 오브젝트 찾기
        Collider closestObjectToPlayer = detectedObjects
            .Where(obj => obj != null && obj.gameObject != null)
            .OrderBy(obj => Vector3.Distance(player.position, obj.transform.position))
            .FirstOrDefault();

        if (closestObjectToPlayer != null)
        {
            // 가장 가까운 오브젝트와 하위 오브젝트의 모든 렌더러 가져오기
            Renderer[] renderers = closestObjectToPlayer.GetComponentsInChildren<Renderer>();
            if (renderers.Length > 0)
            {
                ApplyHighlightShader(renderers);
                currentHighlightedRenderers = renderers;

                // 강조된 오브젝트의 Transform 전달
                OnInteractableObject?.Invoke(closestObjectToPlayer.transform);
            }
        }
    }

    private void ApplyHighlightShader(Renderer[] renderers)
    {
        foreach (var renderer in renderers)
        {
            if (!originalShaders.ContainsKey(renderer))
            {
                originalShaders[renderer] = renderer.material.shader;
            }

            renderer.material.shader = highlightShader;
            renderer.material.SetColor("_BaseColor", new Color(0f, 1f, 0f)); // 연두색 강조
        }
    }

    private void ResetAllShaders()
    {
        if (currentHighlightedRenderers == null) return;

        foreach (var renderer in currentHighlightedRenderers)
        {
            if (renderer == null || renderer.gameObject == null) continue;

            if (originalShaders.ContainsKey(renderer))
            {
                renderer.material.shader = originalShaders[renderer];

                //하이라이트 때 변경한 색상을 다시 복구
                if (renderer.tag == "Window")
                {
                    renderer.material.shader = transparentShader;
                }
                else if (renderer.tag == "Stain")
                {
                    renderer.material.SetColor("_BaseColor", Color.black);
                }
                else if (renderer.tag == "Tool")
                {
                    renderer.material.SetColor("_BaseColor", new Color(1, 1, 1, 125f / 255f));
                }
                else
                {
                    renderer.material.SetColor("_BaseColor", Color.white);
                }

                originalShaders.Remove(renderer);
            }
        }
    }

    private bool IsValidTag(string tag)
    {
        return tag == "InteractableObject" || tag == "Kitchentable" || tag == "ObjectDispenser" ||
               tag == "Table" || tag == "Stove" || tag == "CuttingBoard" || tag == "Sink" ||
               tag == "Trashcan" || tag == "Reception" || tag == "CleaningToolBox" || tag == "Stain";
    }

    private void RemoveDestroyedObjects()
    {
        detectedObjects = detectedObjects.Where(obj => obj != null).ToList();
    }
}