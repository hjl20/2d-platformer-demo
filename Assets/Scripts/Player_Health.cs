
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Health : MonoBehaviour
{
    private bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.y < -0.7f)
        {
            isDead = true;
        }
        //Restart scene on death
        if (isDead)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }



}
