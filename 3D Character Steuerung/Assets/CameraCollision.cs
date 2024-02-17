using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public Transform target; // Der Charakter, dem die Kamera folgt
    public LayerMask collisionLayers; // Die Layer der Objekte, die Kollisionen verursachen sollen

    private Vector3 cameraOffset = new Vector3(0, 2, -5); // Standard-Offset der Kamera
    private float cameraSpeed = 5f; // Die Geschwindigkeit, mit der die Kamera sich anpasst

    void LateUpdate()
    {
        // Berechnung der gewünschten Kameraposition
        Vector3 desiredPosition = target.position + cameraOffset;

        // Überprüfen, ob es Kollisionen auf dem Weg zur gewünschten Position gibt
        RaycastHit hit;
        if (Physics.Linecast(target.position, desiredPosition, out hit, collisionLayers))
        {
            // Anpassen der Kameraposition basierend auf der Kollision
            Vector3 adjustedPosition = hit.point + hit.normal * 0.5f; // Offset von der Kollisionsstelle
            transform.position = Vector3.Lerp(transform.position, adjustedPosition, Time.deltaTime * cameraSpeed);
        }
        else
        {
            // Wenn keine Kollision vorliegt, bewege die Kamera zur gewünschten Position
            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * cameraSpeed);
        }

        // Kamera auf den Charakter ausrichten
        transform.LookAt(target.position);
    }
}
