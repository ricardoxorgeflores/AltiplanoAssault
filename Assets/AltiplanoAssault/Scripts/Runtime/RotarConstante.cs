using UnityEngine;

// Gira el objeto de forma constante (monedas, rotores del drone).
public class RotarConstante : MonoBehaviour
{
    public Vector3 gradosPorSegundo = new Vector3(0, 90, 0);

    void Update()
    {
        transform.Rotate(gradosPorSegundo * Time.deltaTime, Space.Self);
    }
}
