using UnityEngine;
using UnityEngine.UI;

public class SoundEffectsManager : MonoBehaviour
{
    private static SoundEffectsManager Instance;
    private static AudioSource audioSource;
    private static SoundEffectsLibrary soundEffectsLibrary;
    [SerializeField] private Slider sfxSlider;

    private void Awake()
    {
        // Ensure that there is only one instance of SoundEffectsManager
        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            soundEffectsLibrary = GetComponent<SoundEffectsLibrary>();
            DontDestroyOnLoad(gameObject); // Keep this object across scene loads
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    public static void Play(string soundName)
    {

        AudioClip audioClip = soundEffectsLibrary.GetRandomClip(soundName);
        if (audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sfxSlider.onValueChanged.AddListener(delegate { OnValueChanged(); });
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
    public void OnValueChanged()
    {
        SetVolume(sfxSlider.value);
    }
}
