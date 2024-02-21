using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    [SerializeField]
    private CinemachineVirtualCamera _cam;

    [SerializeField]
    private float _shakeDuration = 0f;

    [SerializeField]
    private float _amplitudeGain = 0.5f;
    [SerializeField]
    private float _frequencyGain = 5.0f;

    CinemachineBasicMultiChannelPerlin noiseProfile;

    float originalAmplitude;
    float originalFrequency;

    private void Awake()
    {
        instance = this;

        MessagingCenter.Subscribe<PlayerManager>(this, PlayerManager.MessageOnHpChanged, (sender) =>
        {
            _shakeDuration = 0.2f;
        });
    }

    private void OnDestroy()
    {
        MessagingCenter.Unsubscribe<PlayerManager>(this, PlayerManager.MessageOnHpChanged);
    }

    void Start()
    {
        if (_cam == null)
        {
            Debug.LogErrorFormat("{0} doesn't has virtual camera.", this.gameObject.name);
            return;
        }

        noiseProfile = _cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        originalAmplitude = noiseProfile.m_AmplitudeGain;
        originalFrequency = noiseProfile.m_FrequencyGain;
    }

    void Update()
    {
        if (_shakeDuration > 0)
        {
            noiseProfile.m_AmplitudeGain = _amplitudeGain;
            noiseProfile.m_FrequencyGain = _frequencyGain;

            _shakeDuration -= Time.deltaTime;
        }
        else
        {
            _shakeDuration = 0f;
            noiseProfile.m_AmplitudeGain = originalAmplitude;
            noiseProfile.m_FrequencyGain = originalFrequency;
        }
    }

    public void InstantShake(float duration)
    {
        _shakeDuration = duration;
    }
}