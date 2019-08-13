using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spark.UI;
using TMPro;
using System.Linq;

[AddComponentMenu("")]
public class HealthAndResourceCanvas : InstantiatableCanvas
{
    [SerializeField]
    HealthAndResourceBar baseBar;

    List<EntityBarPair> pairs = new List<EntityBarPair>();

    Camera cam;

    private void Start()
    {
        cam = Camera.main;

        AddPairs(10);

        pairs.Add(new EntityBarPair { bar = baseBar, entity = null });
    }

    class EntityBarPair
    {
        public ICombatEntity entity;
        public HealthAndResourceBar bar;

        public void Update(Camera cam)
        {
            if (entity == null || !entity.Valid)
            {
                if (bar.gameObject.activeSelf) bar.gameObject.SetActive(false);
                return;
            }

            if (!bar.gameObject.activeSelf) bar.gameObject.SetActive(true);
            bar.transform.position = cam.WorldToScreenPoint(entity.transform.position + Vector3.up * 1.6f);
            bar.UpdateHealth(entity.HealthPercentage);
            bar.UpdateResource(entity.ResourcePercentage);

            //var vec = text.rectTransform.anchoredPosition;
            //var scale = canvas.transform.localScale;
            //scale = new Vector3(1f / scale.x, 1f / scale.y, 1f / scale.z);
            //vec.Scale(scale);
            //text.rectTransform.anchoredPosition = vec;
        }
    }

    void AddPairs(int amount)
    {
        for (int i = 0; i < amount; ++i)
        {
            var bar = Instantiate(baseBar, baseBar.transform.parent, true);
            pairs.Add(new EntityBarPair { bar = bar, entity = null });
        }
    }

    private void Update()
    {
        var entities = TargetProvider.Get();
        for (int i = 0; i < entities.Count; ++i)
        {
            var alreadyExistsPair = (from p in pairs where p.entity == entities[i] select p).FirstOrDefault();
            if (alreadyExistsPair != null) continue;
            else
            {
                var toAddPair = (from p in pairs where p.entity == null select p).FirstOrDefault();
                if (toAddPair == null)
                {
                    AddPairs(5);
                    toAddPair = pairs[pairs.Count - 1];
                }

                toAddPair.entity = entities[i];
            }
        }

        for (int i = 0; i < pairs.Count; ++i) pairs[i].Update(cam);
    }
}