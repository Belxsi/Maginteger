using UnityEngine;
using UnityEngine.UI;
public class SetTimeScaleSlider : MonoBehaviour
{
    public Slider slider;

    public void Awake()
    {
        slider.value = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (!GeneratorLevel.mapbuild)
        {
            GeneratorLevel.randomskeep = slider.value;
        }
    }
}
