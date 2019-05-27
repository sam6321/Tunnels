using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Globalization;

public class CountdownTimer : MonoBehaviour
{
    // Start, end
    public class OnTimerExpired : UnityEvent<float, float> { };

    [SerializeField]
    private float period = 30.0f;

    [SerializeField]
    private bool startOnSpawn = false;

    [SerializeField]
    private OnTimerExpired onTimerExpired;

    private NumberFormatInfo format = new NumberFormatInfo()
    {
        NumberDecimalSeparator = ":"
    };

    private Text text;
    private float start = 0;
    private float end = 0;
    private bool expiredFired = false;

    public float Period { get => period; set => period = value; }
    public float TimeLeft => Mathf.Max(end - Time.time, 0);
    public float Progress => Mathf.InverseLerp(start, end, Time.time);
    public bool Expired => Time.time > end;
    public bool ExpiredFired => expiredFired;

    void Start()
    {
        text = GetComponent<Text>();
        if (startOnSpawn)
        {
            Restart();
        }
    }

    void Restart()
    {
        start = Time.time;
        end = start + period;
        expiredFired = false;
    }

    void Update()
    {
        if(Expired)
        {
            if (!ExpiredFired)
            {
                text.text = "0";
                onTimerExpired.Invoke(start, end);
                expiredFired = true;
            }
        }
        else
        {
            text.text = TimeLeft.ToString("F2", format);
        }
    }
}
