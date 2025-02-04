///
/// ¿ª·¢ÖÐ to-do
///
using UnityEngine;
using UnityEngine.UI;

public class PullDetail:MonoBehaviour
{
    private Transform UIButton;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        UIButton = transform.Find("arrow");

        UIButton.GetComponent<Button>().onClick.AddListener(PullPanel);
    }

    private void PullPanel()
    {
        Debug.Log(this.GetComponent<RectTransform>().position.x);
        this.GetComponent<RectTransform>().position=new Vector2(this.GetComponent<RectTransform>().position.x-4,this.GetComponent<RectTransform>().position.y);

    }
}
