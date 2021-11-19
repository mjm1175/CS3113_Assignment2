using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public AudioSource PoisonedSound;
    public AudioSource NauseaedSound;
    public AudioSource DrinkingSound;
    public AudioSource DeathSound;
    public AudioSource DoorOpening;
    public AudioSource AlertSound;

    public AudioSource RegularMusic;
    public AudioSource CombatMusic;
    public AudioSource AlertMusic;
    public AudioSource ScaryMusic;

    public RawImage ScreenOverlay;

    private AudioSource _currentMusic;
    private AudioSource _transitionMusic;

    private bool _isTransiting;
    private float _endVolume;
    private float _fadeInRate;
    private float _fadeOutRate;

    private bool _isOverlayFading;
    private float _overlayFadingRate;
    private string _sceneToLoad;

    public void CrossFadeTo(AudioSource audioSource, float duration)
    {
        if (_isTransiting || _currentMusic == audioSource) return;

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
        if (_isOverlayFading) return;

        _isOverlayFading = true;
        ScreenOverlay.gameObject.SetActive(true);
        ScreenOverlay.color = new Color(0, 0, 0, 0);
        _overlayFadingRate = 1 / duration;
        _sceneToLoad = scene;
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
                _isTransiting = false;
                _currentMusic.Stop();
                _currentMusic.volume = _endVolume;
                _currentMusic = _transitionMusic;
                _transitionMusic = null;
            }
        }
        if (_isOverlayFading && _sceneToLoad != null)
        {
            float alpha = ScreenOverlay.color.a + _overlayFadingRate * Time.deltaTime;
            ScreenOverlay.color = new Color(0, 0, 0, alpha < 1 ? alpha : 1);
            if (alpha >= 1)
            {
                _isOverlayFading = false;
                SceneManager.LoadSceneAsync(_sceneToLoad);
            }
        }
    }
}
