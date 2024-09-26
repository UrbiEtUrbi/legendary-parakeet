using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundInstance : PoolObject
{
    [SerializeField]
    AudioSource m_AudioSource;

    Transform m_Target;
    bool followTarget;
    bool m_SpatialBlend;
    float m_Volume;


    public float Volume => m_Volume;

    string m_SoundName;
    public string SoundName => m_SoundName;
    public float Length{

        get
        {
            if (m_AudioSource.pitch == 0)
            {
                return m_AudioSource.clip.length;
            }
            else
            {
                return m_AudioSource.clip.length / Mathf.Abs(m_AudioSource.pitch);
            }
        }
    }


    public void SetLoops(bool loop)
    {
        m_AudioSource.loop = loop;
    }


    public void UpdateVolume(bool mute)
    {
        if (m_AudioSource == null)
        {
            return;
        }
        if (mute)
        {

            m_AudioSource.volume = 0;
        }
        else
        {
            m_AudioSource.volume = m_Volume;
        }
    }

    public void Set(SoundItem item, Sound sound, Transform target, float minDistance, float maxDistance, bool spatialBlend)
    {
        m_SoundName = sound.SoundName;

        m_AudioSource.clip = item.AudioClip;
        m_AudioSource.pitch = 1+sound.PitchShift;
        m_AudioSource.pitch += Random.value * sound.RandomPitch;

        
     

        if (sound.UseCustomMinMax)
        {

            m_AudioSource.minDistance = sound.MinDistance;
            m_AudioSource.maxDistance = Mathf.Max(sound.MaxDistance, m_AudioSource.minDistance + 0.1f);
        }
        else
        {
            m_AudioSource.minDistance = minDistance;
            m_AudioSource.maxDistance = Mathf.Max(maxDistance, m_AudioSource.minDistance + 0.1f);
        }

       

        m_AudioSource.rolloffMode = AudioRolloffMode.Custom;
       
        m_AudioSource.dopplerLevel = sound.DopplerLevel;





        followTarget = target != null;
        m_Target = target;

        var blend = 1f;
        if (followTarget) {
           
            blend = m_AudioSource.GetCustomCurve(AudioSourceCurveType.CustomRolloff).Evaluate(GetDistanceFromPlayer());
           }

        m_AudioSource.volume = (0.5f + sound.Volume);
        m_Volume = (0.5f + sound.Volume);

        if (sound.OverrideCustomSpatialBlend)
        {
            m_SpatialBlend = sound.UseCustomSpatialBlend;
            m_AudioSource.volume *= sound.UseCustomSpatialBlend ? blend : 1f;
        }
        else
        {
            m_SpatialBlend =spatialBlend;
            m_AudioSource.volume *= spatialBlend ? blend : 1f;
        }
        
       



    }


    private float GetDistanceFromPlayer()
    {
        //TODO change this to some kind of transform


        return 0/m_AudioSource.maxDistance;

    }

    private void Update()
    {

        if (m_Target == null)
        {
            followTarget = false;
        }
        if (!followTarget)
        {
            return;
        }
        if (m_AudioSource.isPlaying)
        {
            transform.position = m_Target.position;
            if (followTarget && m_SpatialBlend)
            {

                var blend = m_AudioSource.GetCustomCurve(AudioSourceCurveType.CustomRolloff).Evaluate(GetDistanceFromPlayer());
                m_AudioSource.volume = m_Volume * blend ;
            }


        }
    }

    public float Play()
    {
        m_AudioSource.Play();
        return Length;
    }
}
