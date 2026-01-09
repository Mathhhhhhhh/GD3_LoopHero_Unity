using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MovementTimer _movementTimer;
    [SerializeField] private Light _sunLight;

    [Header("Time Settings")]
    [SerializeField] private float _startTimeInHours = 12f;
    [SerializeField] private bool _reverseTime = false;

    [Header("Sun Rotation Settings")]
    [SerializeField] private float _sunriseRotation = 0f;
    [SerializeField] private float _noonRotation = 90f;
    [SerializeField] private float _sunsetRotation = 180f;
    [SerializeField] private float _midnightRotation = 270f;

    [Header("Light Settings")]
    [SerializeField] private Gradient _lightColorGradient;
    [SerializeField] private AnimationCurve _lightIntensityCurve;
    [SerializeField] private float _maxIntensity = 1.5f;
    [SerializeField] private float _minIntensity = 0.1f;

    private float _totalHours;
    private float _initialHours;

    private void Start()
    {
        if (_movementTimer != null)
        {
            _totalHours = _movementTimer.MaxMoves;
            _initialHours = _movementTimer.RemainingMoves;
        }

        SetupDefaultGradientAndCurve();
        UpdateDayNightCycle();
    }

    private void Update()
    {
        UpdateDayNightCycle();
    }

    private void UpdateDayNightCycle()
    {
        if (_movementTimer == null || _sunLight == null)
            return;

        float timeProgress = CalculateTimeProgress();
        float rotationAngle = CalculateSunRotation(timeProgress);

        RotateSun(rotationAngle);
        UpdateLightProperties(timeProgress);
    }

    private float CalculateTimeProgress()
    {
        if (_totalHours <= 0)
            return 0f;

        float hoursElapsed = _initialHours - _movementTimer.RemainingMoves;

        if (_reverseTime)
        {
            hoursElapsed = _totalHours - hoursElapsed;
        }

        return Mathf.Clamp01(hoursElapsed / _totalHours);
    }

    private float CalculateSunRotation(float timeProgress)
    {
        float totalRotation = 360f;
        float startOffset = (_startTimeInHours / 24f) * 360f;

        return (timeProgress * totalRotation + startOffset) % 360f;
    }

    private void RotateSun(float angle)
    {
        _sunLight.transform.rotation = Quaternion.Euler(angle - 90f, 170f, 0f);
    }

    private void UpdateLightProperties(float timeProgress)
    {
        if (_lightColorGradient != null && _lightColorGradient.colorKeys.Length > 0)
        {
            _sunLight.color = _lightColorGradient.Evaluate(timeProgress);
        }

        if (_lightIntensityCurve != null && _lightIntensityCurve.length > 0)
        {
            float curveValue = _lightIntensityCurve.Evaluate(timeProgress);
            _sunLight.intensity = Mathf.Lerp(_minIntensity, _maxIntensity, curveValue);
        }
    }

    private void SetupDefaultGradientAndCurve()
    {
        if (_lightColorGradient == null || _lightColorGradient.colorKeys.Length == 0)
        {
            _lightColorGradient = new Gradient();

            GradientColorKey[] colorKeys = new GradientColorKey[5];
            colorKeys[0] = new GradientColorKey(new Color(0.3f, 0.3f, 0.5f), 0f);
            colorKeys[1] = new GradientColorKey(new Color(1f, 0.6f, 0.4f), 0.23f);
            colorKeys[2] = new GradientColorKey(new Color(1f, 1f, 0.9f), 0.5f);
            colorKeys[3] = new GradientColorKey(new Color(1f, 0.5f, 0.3f), 0.73f);
            colorKeys[4] = new GradientColorKey(new Color(0.2f, 0.2f, 0.4f), 1f);

            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
            alphaKeys[0] = new GradientAlphaKey(1f, 0f);
            alphaKeys[1] = new GradientAlphaKey(1f, 1f);

            _lightColorGradient.SetKeys(colorKeys, alphaKeys);
        }

        if (_lightIntensityCurve == null || _lightIntensityCurve.length == 0)
        {
            _lightIntensityCurve = new AnimationCurve();
            _lightIntensityCurve.AddKey(0f, 0.1f);
            _lightIntensityCurve.AddKey(0.25f, 0.8f);
            _lightIntensityCurve.AddKey(0.5f, 1f);
            _lightIntensityCurve.AddKey(0.75f, 0.8f);
            _lightIntensityCurve.AddKey(1f, 0.1f);
        }
    }

    public float GetCurrentTimeOfDay()
    {
        float timeProgress = CalculateTimeProgress();
        return (_startTimeInHours + (timeProgress * 24f)) % 24f;
    }
}
