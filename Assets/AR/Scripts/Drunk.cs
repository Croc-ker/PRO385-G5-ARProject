using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Drunk : MonoBehaviour
{
    [SerializeField] private Volume drunkVolume;
    void Start()
    {
        drunkVolume.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnClick()
    {
        drunkVolume.enabled = true;
        Invoke("DisableDrunk", 5f);
    }
    private void DisableDrunk()
    {
        drunkVolume.enabled = false;
    }
}
