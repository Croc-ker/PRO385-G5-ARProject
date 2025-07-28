using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.HighDefinition;

public class Drunk : MonoBehaviour
{
    [SerializeField] private Volume drunkVolume;
    [SerializeField] private Transform playerTransform;
    private LensDistortion lensDistortion;
    private VolumeProfile profile;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        profile = drunkVolume.sharedProfile;
    }

    // Update is called once per frame
    void Update()
    {
        if(profile.TryGet<LensDistortion>(out lensDistortion))
        {
            float minx = 0.3f;
            float maxx = 0.7f;
        }
    }

    public void OnClick()
    {
        Instantiate(drunkVolume, playerTransform.position, Quaternion.identity);
    }
}
