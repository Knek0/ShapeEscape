using UnityEngine;
public class camera : MonoBehaviour
{
    //reference to player object
    public GameObject player;

    //offset between camera and player
    private Vector3 offset;

    private void Start()
    {
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update() 
    {
        // Follow player
        transform.position = player.transform.position + offset;
    }
}
