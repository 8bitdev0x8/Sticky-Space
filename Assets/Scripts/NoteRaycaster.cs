using UnityEngine;

public class NoteRaycaster : MonoBehaviour
{
    public Transform controllerTransform;
    public GameObject stickyNotePrefab;

    void Update()
    {
        bool triggerPressed =
            OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) ||
            Input.GetMouseButtonDown(0);

        bool deleteButtonPressed =
            OVRInput.GetDown(OVRInput.Button.Two) ||
            Input.GetKeyDown(KeyCode.X);

        if (deleteButtonPressed)
        {
            Debug.Log("Delete button pressed");

            Ray ray = new Ray(controllerTransform.position, controllerTransform.forward);
            Debug.DrawRay(controllerTransform.position, controllerTransform.forward * 5f, Color.blue);

            if (Physics.Raycast(ray, out RaycastHit hit, 5f))
            {
                Debug.Log("Delete raycast hit: " + hit.collider.name);

                StickyNote note = hit.collider.GetComponentInParent<StickyNote>();
                if (note != null)
                {
                    Debug.Log("StickyNote deleted: " + note.gameObject.name);
                    Destroy(note.gameObject);
                    return;
                }
                else
                {
                    Debug.Log("Delete raycast did NOT hit a sticky note");
                }
            }
            else
            {
                Debug.Log("Delete raycast hit NOTHING");
            }
        }

        if (triggerPressed)
        {
            Debug.Log("Trigger pressed");

            Ray ray = new Ray(controllerTransform.position, controllerTransform.forward);
            Debug.DrawRay(controllerTransform.position, controllerTransform.forward * 5f, Color.red);

            if (Physics.Raycast(ray, out RaycastHit hit, 5f))
            {
                Debug.Log("Ray hit: " + hit.collider.name);

                StickyNote note = hit.collider.GetComponentInParent<StickyNote>();
                if (note != null)
                {
                    Debug.Log("Editing StickyNote: " + note.gameObject.name);
                    note.EditNote();
                    return;
                }

                Debug.Log("Spawning new sticky note");
                Instantiate(stickyNotePrefab, hit.point,
                    Quaternion.LookRotation(-hit.normal));
            }
            else
            {
                Debug.Log("Trigger raycast hit NOTHING");
            }
        }
    }
}
