using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class FPSCounter : MonoBehaviour
{

    public float timer, refresh, avgFramerate;
    string display = "{0} FPS";
    private TextMeshProUGUI m_Text;

    public UnityEvent Next = new();

    public UnityEvent CheatRes = new();


    private void Start()
    {
        m_Text = GetComponent<TextMeshProUGUI>();
#if !UNITY_EDITOR
        gameObject.SetActive(false);

#endif
    }


    private void Update()
    {

        //Change smoothDeltaTime to deltaTime or fixedDeltaTime to see the difference
        float timelapse = Time.smoothDeltaTime;
        timer = timer <= 0 ? refresh : timer -= timelapse;

        if (timer <= 0) avgFramerate = (int)(1f / timelapse);
        m_Text.text = string.Format(display, avgFramerate.ToString());

        Time.timeScale = Keyboard.current.spaceKey.isPressed ? 0.01f : 1f;

        if (Keyboard.current.nKey.wasPressedThisFrame)
        {
            Next.Invoke();
        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            CheatRes.Invoke();
        }
    }
}