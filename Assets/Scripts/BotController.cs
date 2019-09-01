using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 2 1
// 3 4

public class BotController : MonoBehaviour
{

    private Rigidbody rb;

    public Material red, norm;

    public float speed, rotSpeed;

    public ModuleController mod1, mod2, mod3, mod4;

    public Text speedomT;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void FixedUpdate()
    {
        float f = Input.GetAxisRaw("Vertical");
        float s = Input.GetAxisRaw("Horizontal");
        float r = -Input.GetAxisRaw("Vertical2");

        print(f + " " + s + " " + r);

        holonomicMixer(f, s, r);

        float modifier = getMod(Mathf.Atan2(f, s));

        float angle = getAngle() / 180 * Mathf.PI;
        float temp = f * Mathf.Cos(angle) + s * Mathf.Sin(angle);
        s = -f * Mathf.Sin(angle) + s * Mathf.Cos(angle);
        f = temp;

        speedomT.text = "Speed Mod: " + modifier;

        Vector3 translate = (new Vector3(s, 0, f).normalized) * speed;
        transform.Translate(translate);
        transform.Rotate(new Vector3(0, r * rotSpeed, 0));
    }

    public float getAngle()
    {
        return transform.rotation.eulerAngles.y;
    }

    public void holonomicMixer(float forward, float strafe, float rotation)
    {

        float angle = getAngle() / 180 * Mathf.PI;
        float temp = forward * Mathf.Cos(angle) + strafe * Mathf.Sin(angle);
        strafe = -forward * Mathf.Sin(angle) + strafe * Mathf.Cos(angle);
        forward = temp;

        float L = 3f; // WheelBase
        float W = 2.5f; // TrackWidth
        float R = Mathf.Sqrt((L * L) + (W * W));

        float A = strafe - rotation * (L / R);
        float B = strafe + rotation * (L / R);
        float C = forward - rotation * (W / R);
        float D = forward + rotation * (W / R);

        print(A + ", " + B + ", " + C + ", " + D);

        float[] speeds = new float[] {
      Mathf.Sqrt((B * B) + (C * C)),
      Mathf.Sqrt((B * B) + (D * D)),
      Mathf.Sqrt((A * A) + (D * D)),
      Mathf.Sqrt((A * A) + (C * C))
    };

        float[] angles = new float[] {
      Mathf.Atan2(B, C) * 180 / Mathf.PI,
      Mathf.Atan2(B, D) * 180 / Mathf.PI,
      Mathf.Atan2(A, D) * 180 / Mathf.PI,
      Mathf.Atan2(A, C) * 180 / Mathf.PI
    };

        float max = 1;

        if (speeds[0] != 0)
        {
            max = speeds[0];
        }

        for (int i = 1; i < speeds.Length; i++)
        {
            if (max < speeds[i])
            {
                max = speeds[i];
            }
        }

        float factor = 1 / max;

        mod1.setAngle(angles[0]);
        mod2.setAngle(angles[1]);
        mod3.setAngle(angles[2]);
        mod4.setAngle(angles[3]);

        print(speeds[0]);
        print(speeds[1]);
        print(speeds[2]);
        print(speeds[3]);

        mod1.setSpeed(speeds[0] * factor);
        mod2.setSpeed(speeds[1] * factor);
        mod3.setSpeed(speeds[2] * factor);
        mod4.setSpeed(speeds[3] * factor);
    }

    public float getMod(float aimedAngle)
    {
        float best = 0;

        if (mod1.speed * Mathf.Cos(mod1.angle - aimedAngle) > best)
        {
            best = mod1.speed * Mathf.Cos(mod1.angle - aimedAngle);
        }

        if (mod2.speed * Mathf.Cos(mod2.angle - aimedAngle) > best)
        {
            best = mod2.speed * Mathf.Cos(mod2.angle - aimedAngle);
        }

        if (mod3.speed * Mathf.Cos(mod3.angle - aimedAngle) > best)
        {
            best = mod3.speed * Mathf.Cos(mod3.angle - aimedAngle);
        }

        if (mod4.speed * Mathf.Cos(mod4.angle - aimedAngle) > best)
        {
            best = mod4.speed * Mathf.Cos(mod4.angle - aimedAngle);
        }

        return best;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Renderer>().material = red;
    }

    private void OnTriggerExit(Collider other)
    {
        other.GetComponent<Renderer>().material = norm;
    }

}