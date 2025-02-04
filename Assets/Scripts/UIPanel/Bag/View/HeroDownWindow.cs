using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeployButtonView : MonoBehaviour
{
    //自身属性
    public int ButtonID;
    //UI控件
    public Image UIImage;
    private Button UIButton;
    private Transform Image;
    private Transform ATK;

    //数据存储
    private HeroLocalItem HeroLocalData;
    private Hero HeroTableData;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        UIImage = this.GetComponent<Image>();
        UIButton = this.GetComponent<Button>();
        ATK = transform.Find("ATK");
        Image = transform.Find("Image");

        UIButton.onClick.AddListener(OnClickButton);
    }

    //更改当前选中物体
    private void OnClickButton()
    {
        transform.parent.parent.parent.GetComponent<BackPackCtrl>().chooseBid = ButtonID;
    }

    //设置自身不发光
    public void StopLighting()
    {
        UIImage.color = new Color(0, 0, 0);
    }
    //设置自身发光
    public void Lighting()
    {
        UIImage.color = new Color(1, 1, 1);
    }

    //更改显示对象
    public void Refresh(HeroLocalItem HeroLocalData)
    {
        this.HeroLocalData = HeroLocalData;
        this.HeroTableData = GameManager.Instance.GetHeroById(HeroLocalData.id);
        //显示图片
        Texture2D pic = (Texture2D)Resources.Load(this.HeroTableData.ImgPath);
        Sprite tmp = Sprite.Create(pic, new Rect(0, 0, pic.width, pic.height), new Vector2(0, 0));
        transform.Find("Image").GetComponent<Image>().sprite = tmp;
        Color ncolor = transform.Find("Image").GetComponent<Image>().color;
        ncolor.a = 1;
        transform.Find("Image").GetComponent<Image>().color = ncolor;
        //显示战力
        transform.Find("ATK").GetComponent<Text>().text = this.HeroLocalData.ATK.ToString();
    }
}
