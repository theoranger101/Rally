using System;
using UnityEngine;
using Utility;

public class CarController : MonoBehaviour
{
    public int PlayerIndex;
        
    public float CurrentMoveSpeed;

    [SerializeField]
    private Camera m_Cam;

    private GeneralSettings m_GeneralSettings => GeneralSettings.Get();

    private float m_InitialMoveSpeed;

    private bool m_Powerup;

    private float m_PowerupDamp;

    private OSC m_Osc;

#if UNITY_EDITOR
    private void OnValidate()
    {
        m_Cam = GetComponentInChildren<Camera>();
        m_Osc = FindObjectOfType<OSC>();
    }
#endif

    private void Start()
    {
        m_InitialMoveSpeed = m_GeneralSettings.CarMoveSpeed;
        CurrentMoveSpeed = m_InitialMoveSpeed;

        if (m_GeneralSettings.UseOSC)
        {
            if (PlayerIndex == 1)
            {
                m_Osc.SetAddressHandler("/CubeX", HandleOSC);
            }
            else
            {
                m_Osc.SetAddressHandler("/CubeY", HandleOSC);
            }
        }
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * CurrentMoveSpeed * Time.deltaTime);

        if (m_GeneralSettings.UseOSC)
        {
            
        }
        else
        {
            GetInput();
        }
        
        if (m_Powerup)
        {
            CurrentMoveSpeed = Mathf.SmoothDamp(CurrentMoveSpeed, m_InitialMoveSpeed, ref m_PowerupDamp,
                m_GeneralSettings.PowerupTimer);
        }
    }

    private void GetInput()
    {
        Vector3 mousePosition = Input.mousePosition;

        // Calculate the normalized x position of the mouse
        float normalizedX = mousePosition.x / Screen.width;

        // Determine the player based on the normalized x position
        int player;
        if (normalizedX < 0.5f)
            player = 1; // Player 1
        else
            player = 2; // Player 2

        // Calculate the target position based on the player
        float targetX;
        if (player == 1)
        {
            targetX = Mathf.Lerp(m_GeneralSettings.LeftBoundary, 0f, normalizedX * 2);
        }
        else
        {
            targetX = Mathf.Lerp(0f, m_GeneralSettings.RightBoundary, (normalizedX - 0.5f) * 2);
        }

        var direction = targetX > 0 ? Vector3.right : Vector3.left;

        if (Physics.Raycast(transform.position, direction, 2.5f))
        {
            
        }
        else
        {
            // Set the car's position
            transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
        }
    }

    private void HandleOSC(OscMessage message)
    {
        float targetX;
        if (PlayerIndex == 1)
        {
            targetX = Mathf.Lerp(m_GeneralSettings.LeftBoundary, 0f, message.GetFloat(0) * 2);
        }
        else
        {
            targetX = Mathf.Lerp(0f, m_GeneralSettings.RightBoundary, (message.GetFloat(0) - 0.5f) * 2);
        }

        var direction = targetX > 0 ? Vector3.right : Vector3.left;

        if (Physics.Raycast(transform.position, direction, 2.5f))
        {
            
        }
        else
        {
            // Set the car's position
            transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // if powerup
        
        // if finish
    }

    private void OnPowerup()
    {
        m_Powerup = true;
        CurrentMoveSpeed *= m_GeneralSettings.PowerupMultiplier;

        if (CurrentMoveSpeed > m_GeneralSettings.MaxCarSpeed)
        {
            CurrentMoveSpeed = m_GeneralSettings.MaxCarSpeed;
        }
    }

    private void OnLevelFinish()
    {
        
    }
}