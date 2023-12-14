using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreCompany.Cosmetics
{
    public class CosmeticApplication : MonoBehaviour
    {
        public Transform head;
        public Transform hip;
        public Transform lowerArmRight;
        public Transform shinLeft;
        public Transform shinRight;
        public Transform chest;
        public List<CosmeticInstance> spawnedCosmetics = new List<CosmeticInstance>();

        public void Awake()
        {
            head = FindDeepChild(transform, "spine/root.x/spine_01.x/spine_02.x/spine_03.x/neck.x/head.x");
            if (head == null)
            {
                head = FindDeepChild(transform, "root.x/spine_01.x/spine_02.x/spine_03.x/neck.x/head.x");
            }
            if (head == null)
            {
                head = FindDeepChild(transform, "spine/spine.001/spine.002/spine.003/spine.004");
            }
            chest = transform.Find("spine").Find("spine.001").Find("spine.002").Find("spine.003");
            lowerArmRight = transform.Find("spine").Find("spine.001").Find("spine.002").Find("spine.003").Find("shoulder.R").Find("arm.R_upper").Find("arm.R_lower");
            hip = transform.Find("spine");
            shinLeft = transform.Find("spine").Find("thigh.L").Find("shin.L");
            shinRight = transform.Find("spine").Find("thigh.R").Find("shin.R");

            RefreshAllCosmeticPositions();
        }

        private void OnDisable()
        {
            foreach (var spawnedCosmetic in spawnedCosmetics)
            {
                if (spawnedCosmetic != null) { 
                spawnedCosmetic.gameObject.SetActive(false);
            }
            }
        }

        private void OnEnable()
        {
            foreach (var spawnedCosmetic in spawnedCosmetics)
            {
                if (spawnedCosmetic != null)
                {

                    spawnedCosmetic.gameObject.SetActive(true);
                }
                }
        }

        public void ClearCosmetics()
        {
            foreach (var spawnedCosmetic in spawnedCosmetics)
            {
                if (spawnedCosmetic != null)
                {

                    GameObject.Destroy(spawnedCosmetic.gameObject);
                }
            }
            spawnedCosmetics.Clear();
        }
        
        public void ApplyCosmetic(string cosmeticId, bool startEnabled)
        {
            if (CosmeticRegistry.cosmeticInstances.ContainsKey(cosmeticId))
            {
                CosmeticInstance cosmeticInstance = CosmeticRegistry.cosmeticInstances[cosmeticId];
                GameObject cosmeticInstanceGameObject = GameObject.Instantiate(cosmeticInstance.gameObject);
                cosmeticInstanceGameObject.SetActive(startEnabled);
                CosmeticInstance cosmeticInstanceBehavior = cosmeticInstanceGameObject.GetComponent<CosmeticInstance>();
                spawnedCosmetics.Add(cosmeticInstanceBehavior);
                if (startEnabled)
                {
                    ParentCosmetic(cosmeticInstanceBehavior);
                }
            }
        }
        
        public void RefreshAllCosmeticPositions()
        {
            foreach (var spawnedCosmetic in spawnedCosmetics)
            { 
                if (spawnedCosmetic != null) { 
                ParentCosmetic(spawnedCosmetic);
                }
            }
        }

        private void ParentCosmetic(CosmeticInstance cosmeticInstance)
        {
            Transform targetTransform = null;
            switch (cosmeticInstance.cosmeticType)
            {
                case CosmeticType.HAT:
                    targetTransform = head;
                    break;
                case CosmeticType.R_LOWER_ARM:
                    targetTransform = lowerArmRight;
                    break;
                case CosmeticType.HIP:
                    targetTransform = hip;
                    break;
                case CosmeticType.L_SHIN:
                    targetTransform = shinLeft;
                    break;
                case CosmeticType.R_SHIN:
                    targetTransform = shinRight;
                    break;
                case CosmeticType.CHEST:
                    targetTransform = chest;
                    break;
            }
            
            cosmeticInstance.transform.position = targetTransform.position;
            cosmeticInstance.transform.rotation = targetTransform.rotation;
            cosmeticInstance.transform.parent = targetTransform;
        }
        Transform FindDeepChild(Transform parent, string path)
        {
            Transform current = parent;
            foreach (var name in path.Split('/'))
            {
                if (current == null) return null;
                current = current.Find(name);
            }
            return current;
        }
    }

}