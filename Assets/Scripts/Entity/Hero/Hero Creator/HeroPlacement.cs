using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class HeroPlacement : MonoBehaviour
{
    [SerializeField]
    Material outlineSelectMaterial;

    [SerializeField]
    Transform firstSpawn;
    [SerializeField]
    Transform secondSpawn;

    Camera cam;
    Renderer currentSelected;

    void Start()
    {
        cam = Camera.main;
        Time.timeScale = 0f;

        for (int i = 0; i < HeroCollection.Instance.GetSelected().Count; i++)
        {
            var h = HeroCollection.Instance.GetSelected()[i];
            HeroCreator.CreateLiveFrom(h, Vector3.Lerp(firstSpawn.position, secondSpawn.position, (float)i / (HeroCollection.Instance.GetSelected().Count - 1)));
        }

        var ui = new PreCombatUI();
        ui.OnStart.AddListener(() =>
        {
            Time.timeScale = 1f;
            ClearMaterial();
            enabled = false;
            ui.Hide();
        });
        ui.Show();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                if (hit.collider.GetComponent<HeroEntity>())
                {
                    ClearMaterial();
                    SetMaterial(hit.collider.GetComponent<Renderer>());
                }
                else if (currentSelected)
                {
                    currentSelected.GetComponent<NavMeshAgent>().Warp(hit.point + Vector3.up);
                    Physics.SyncTransforms();
                }
            }
        }
        else if (Input.GetMouseButtonDown(1)) ClearMaterial();
    }

    void ClearMaterial()
    {
        if (currentSelected)
        {
            var materials = currentSelected.sharedMaterials.ToList();
            materials.Remove(outlineSelectMaterial);
            currentSelected.sharedMaterials = materials.ToArray();
            currentSelected = null;
        }
    }

    void SetMaterial(Renderer rend)
    {
        currentSelected = rend;
        if (currentSelected)
        {
            var materials = currentSelected.sharedMaterials.ToList();
            if (!materials.Contains(outlineSelectMaterial)) materials.Add(outlineSelectMaterial);
            currentSelected.sharedMaterials = materials.ToArray();
        }
    }
}
