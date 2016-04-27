using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {
    public float m_speed = 2f;
    public float m_rotateSpeed = 10f;
	public float m_rotationSmoothness = 0.2f;
    public Joystick m_joystick;

    public Player m_player;
    
    private float _horizontal = 0;
    private float _vertical = 0;
    private Rigidbody m_rigidBody;

    public enum Joystick
    {
        NONE = -1,
        ONE,
        TWO,
        THREE,
        FOUR,
        KEYBOARD_ONE
    }
	
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
    }

	void FixedUpdate ()
    {
        if(m_joystick == Joystick.KEYBOARD_ONE)
            UpdateKeyboard();
        else
            UpdateJoystick();
    }

    private string GetJoystick(Joystick joystick)
    {
        switch(joystick)
        {
            case Joystick.ONE:
                return "Joystick_One_";
            case Joystick.TWO:
                return "Joystick_Two_";
            case Joystick.THREE:
                return "Joystick_Three_";
            case Joystick.FOUR:
                return "Joystick_Four_";
            default:
                return "";
        }
    }

    private void UpdateJoystick()
    {
        if (m_joystick != Joystick.NONE)
        {
            string joystickName = GetJoystick(m_joystick);
            Debug.Log("Joystick name: " + joystickName);
            _horizontal = Input.GetAxis(joystickName + "Horizontal");
            _vertical = Input.GetAxis(joystickName + "Vertical");
            
            if (m_rigidBody)
            {
                m_rigidBody.AddForce(Vector3.forward * _vertical * m_speed);
                m_rigidBody.AddForce(Vector3.right * _horizontal * m_speed);
            }

            // Right Analog Stick
			float rightTriggerHorizontalAxis = Input.GetAxis(joystickName + "RHorizontal");
			float rightTriggerVerticalAxis = Input.GetAxis(joystickName + "RVertical");

			if (rightTriggerHorizontalAxis != 0.0f || rightTriggerVerticalAxis != 0.0f)
			{
				Quaternion prevOrientation = transform.rotation;

				Vector2 axis = new Vector2(rightTriggerHorizontalAxis, -rightTriggerVerticalAxis);
				axis.Normalize();

				float angle = Mathf.Atan2(axis.x, axis.y) * Mathf.Rad2Deg;
				transform.eulerAngles = new Vector3(transform.eulerAngles.x, angle, transform.eulerAngles.z);
				transform.rotation = Quaternion.Slerp(prevOrientation, transform.rotation, m_rotationSmoothness);
			}

            //transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(transform.eulerAngles.x, newHeading, transform.eulerAngles.z), Time.fixedDeltaTime);
            
            if (Input.GetAxis(joystickName + "Trigger") >= 0.25f || Input.GetAxis(joystickName + "Trigger") <= -0.25f)
            {
                GunManager.Instance.Shoot(m_player);
            }
        }
    }
    private void UpdateKeyboard()
    {

        if (Input.GetKey(KeyCode.A))
        {
            if (m_rigidBody)
            {
                m_rigidBody.AddForce(-Vector3.right * m_speed);
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (m_rigidBody)
            {
                m_rigidBody.AddForce(Vector3.right * m_speed);
            }
        }

        if (Input.GetKey(KeyCode.W))
        {
            if (m_rigidBody)
            {
                m_rigidBody.AddForce(Vector3.forward * m_speed);
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            if (m_rigidBody)
            {
                m_rigidBody.AddForce(-Vector3.forward * m_speed);
            }
        }
        float h = 0;
        float v = 0;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            h = -1;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            h = 1;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            v = -1;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            v = 1;
        }
        UpdateRotation(h, v);

        if (Input.GetKey(KeyCode.Space))
        {
            GunManager.Instance.Shoot(m_player);
        }

       
    }

    public void UpdateRotation(float horizontal, float vertical)
    {
        // Right Analog Stick
        float rightTriggerHorizontalAxis = horizontal;
        float rightTriggerVerticalAxis = vertical;

        if (rightTriggerHorizontalAxis != 0.0f || rightTriggerVerticalAxis != 0.0f)
        {
            Quaternion prevOrientation = transform.rotation;

            Vector2 axis = new Vector2(rightTriggerHorizontalAxis, -rightTriggerVerticalAxis);
            axis.Normalize();

            float angle = Mathf.Atan2(axis.x, axis.y) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, angle, transform.eulerAngles.z);
            transform.rotation = Quaternion.Slerp(prevOrientation, transform.rotation, m_rotationSmoothness);
        }

    }
}
