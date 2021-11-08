using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesManager : MonoBehaviour
{
    [Serializable]
    public class ParticleType_And_Ref
    {
        public ParticleTypes ParticleType;
        public GameObject particleRef;
    }

    [SerializeField]
    private List<ParticleType_And_Ref> ParticleType_And_Ref_List = new List<ParticleType_And_Ref>();


    public enum ParticleTypes
    {
        None = 0,

        Boom = 1,
        Splash = 2,
        Snow = 3,
        DashRocket = 4
    }

    internal void Init(GameManager gameManager)
    {
        
    }

    public GameObject Play_Effect(ParticleTypes particleType, Vector3 pos , Transform parent = null)
    {
        GameObject particle = getParticleSystemRef(particleType);

       //  = isLoop;

        return Play_Effect(particle, pos, parent);

    }

    private GameObject Play_Effect(GameObject particle, Vector3 position, Transform parent)
    {
        if (!parent)
        {
            parent = transform;
        }
        GameObject clone = Instantiate(particle, parent);
        clone.transform.localPosition = position;
        clone.GetComponent<ParticleSystem>().Play();
        return clone;
    }

    private GameObject getParticleSystemRef(ParticleTypes particleType)
    {
        for (int i = 0; i < ParticleType_And_Ref_List.Count; i++)
        {
            if (ParticleType_And_Ref_List[i].ParticleType == particleType)
                return ParticleType_And_Ref_List[i].particleRef;
        }

        return null;
    }
}
