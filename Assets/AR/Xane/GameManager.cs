using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SoundManager.PlayLoopingSound(SoundType.ASGORE_PASSIVE, 0.1f);
    }
}