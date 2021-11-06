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
    }

    internal void Init(GameManager gameManager)
    {
        
    }

    public void Play_Effect(ParticleTypes particleType, Vector3 pos ,  bool isLoop = false, int player_index = 0)
    {
        GameObject clip = getParticleSystemRef(particleType);

       //  = isLoop;

        Play_Effect(clip, pos);

    }

    private void Play_Effect(GameObject particle, Vector3 position)
    {
        GameObject clone = Instantiate(particle, transform);
        clone.transform.position = position;
        clone.GetComponent<ParticleSystem>().Play();
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
