
using UnityEngine;

public class Camera_System : MonoBehaviour
{
    private GameObject player;

    //Level boundary coordinates for camera
    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    //Usually better for cameras vs Update or FixedUpdate
    private void LateUpdate()
    {
        float x = Mathf.Clamp(player.transform.position.x, xMin, xMax);
        float y = Mathf.Clamp(player.transform.position.y, yMin, yMax);

        //Makes camera follow 'Player'
        gameObject.transform.position = new Vector3(x, y, gameObject.transform.position.z);

    }
}
