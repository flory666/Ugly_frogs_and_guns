using UnityEngine;
using UnityEngine.UI;
public class healthbar : MonoBehaviour
{
    public Slider slider;
    public GameObject scale;
public void sethealth(int health)
    { slider.value = health; }
    public void spin()
    {
        Vector3 theScale = scale.transform.localScale;
        theScale.x *= -1;
        scale.transform.localScale = theScale;
    }
}