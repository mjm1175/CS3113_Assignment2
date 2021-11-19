using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

public class TransitionManager : MonoBehaviour
{
    public AudioSource PoisonedSound;
    public AudioSource NauseaedSound;
    public AudioSource DrinkingSound;
    public AudioSource DeathSound;
    public AudioSource DoorOpening;
    public AudioSource AlertSound;
    public AudioSource PickupPaperSound;
    public AudioSource WireCutSound;

    public AudioSource RegularMusic;
    public AudioSource CombatMusic;
    public AudioSource AlertMusic;
    public AudioSource ScaryMusic;
    public AudioSource WinMusic;

    public RawImage ScreenOverlay;

    private AudioSource _currentMusic;
    private AudioSource _transitionMusic;

    private bool _isTransiting;
    private float _endVolume;
    private float _fadeInRate;
    private float _fadeOutRate;

    private bool _isOverlayFading;
    private float _overlayAlpha;
    private float _overlayFadingRate;
    private string _sceneToLoad;
    private string _startScene;
    private CinemachineVirtualCamera _vcam;

    public void CrossFadeTo(AudioSource audioSource, float duration)
    {
        if (_currentMusic == audioSource) return;
        if (_isTransiting) FinishAudioFade();

        _isTransiting = true;
        _endVolume = audioSource.volume;
        _fadeInRate = _endVolume / duration;
        _fadeOutRate = _currentMusic.volume / duration;
        _transitionMusic = audioSource;

        audioSource.volume = 0;
        audioSource.Play();
    }

    public void FadeToScene(string scene, float duration)
    {
        if (_isOverlayFading) FinishFade(true);

        _isOverlayFading = true;
        ScreenOverlay.gameObject.SetActive(true);
        ScreenOverlay.color = new Color(0, 0, 0, 0);
        _overlayFadingRate = 1 / duration;
        _overlayAlpha = 0;
        _sceneToLoad = scene;
        _startScene = SceneManager.GetActiveScene().name;
    }

    public void FadeOut(float duration)
    {
        if (_isOverlayFading) FinishFade(true);

        _isOverlayFading = true;
        ScreenOverlay.gameObject.SetActive(true);
        ScreenOverlay.color = new Color(0, 0, 0, 1f);
        _overlayFadingRate = -1 / duration;
        _overlayAlpha = 1;
        _sceneToLoad = null;
        _startScene = SceneManager.GetActiveScene().name;
    }

    public void PositionCamera(CineConfig config)
    {
        if (_vcam == null)
        {
            Debug.LogWarning("Cannot locate the vcam object");
            return;
        }

        CinemachineTransposer transposer = _vcam.GetCinemachineComponent<CinemachineTransposer>();
        Vector3 followOffset = transposer.m_FollowOffset;
        Vector3 camRot = _vcam.transform.rotation.eulerAngles;

        transposer.m_FollowOffset = new Vector3(config.OffsetX, followOffset.y, config.OffsetZ);
        _vcam.transform.rotation = Quaternion.Euler(camRot.x, config.RotationY, camRot.z);
    }

    private void Awake()
    {
        if (PublicVars.TransitionManager != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);
        PublicVars.TransitionManager = this;

        UpdateVcam();
        _currentMusic = RegularMusic;
        RegularMusic.Play();
    }

    private void FixedUpdate()
    {
        if (_isTransiting && _transitionMusic != null)
        {
            _currentMusic.volume -= _fadeOutRate * Time.deltaTime;
            _transitionMusic.volume += _fadeInRate * Time.deltaTime;
            if (_transitionMusic.volume >= _endVolume)
            {
                FinishAudioFade();
            }
        }

        if (_startScene != SceneManager.GetActiveScene().name) _isOverlayFading = false;

        if (_isOverlayFading)
        {
            _overlayAlpha += _overlayFadingRate * Time.deltaTime;
            ScreenOverlay.color = new Color(0, 0, 0, _overlayAlpha > 0 ? (_overlayAlpha < 1 ? _overlayAlpha : 1) : 0);
            if (_overlayAlpha > 1 || _overlayAlpha < 0)
            {
                FinishFade(false);
            }
        }
    }

    private void FinishAudioFade()
    {
        _isTransiting = false;
        _currentMusic.Stop();
        _currentMusic.volume = _endVolume;
        _transitionMusic.volume = _endVolume;

        _currentMusic = _transitionMusic;
        _transitionMusic = null;
    }

    private void FinishFade(bool quickLoad)
    {
        _isOverlayFading = false;
        if (_sceneToLoad != null)
        {
            if (quickLoad)
            {
                SceneManager.LoadScene(_sceneToLoad);
                UpdateVcam();
            }
            else SceneManager.LoadSceneAsync(_sceneToLoad).completed += HandleComplete;
        }
        else if (_overlayAlpha < 0) ScreenOverlay.gameObject.SetActive(false);
    }

    private void UpdateVcam()
    {
        Debug.Log(PublicVars.CameraConfig.OffsetX);
        Debug.Log(PublicVars.CameraConfig.OffsetZ);
        Debug.Log(PublicVars.CameraConfig.RotationY);
        _vcam = GameObject.FindWithTag("VCam")?.GetComponent<CinemachineVirtualCamera>();
        PositionCamera(PublicVars.CameraConfig);
        if (_vcam == null) Debug.LogWarning($"TransitionManager cannot find an instance of CinemachineVirtualCamera in {SceneManager.GetActiveScene().name}");
    }

    void HandleComplete(AsyncOperation operation)
    {
        UpdateVcam();
        FadeOut(PublicVars.GENERAL_FADE_TIME);
    }
}
