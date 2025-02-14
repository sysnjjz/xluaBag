using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 子控件：单独一个上阵英雄格子的视图逻辑
/// </summary>
public class HeroDownWindow : MonoBehaviour
{
    //自身属性
    public int ButtonID;
    //UI控件
    public Image UIImage;
    private Button UIButton;
    private Image Image;
    private Text ATK;

    //控制器脚本
    public BagController bagController;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        UIImage = transform .GetComponent<Image>();
        UIButton = transform.GetComponent<Button>();
        ATK = transform.Find("ATK").GetComponent<Text>();
        Image = transform.Find("Image").GetComponent<Image>();

        UIButton.onClick.AddListener(OnClickButton);
    }

    //更改当前选中物体
    public void OnClickButton()
    {
        bagController.chooseBid = ButtonID;
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
    public void Refresh(HeroLocalItem heroLocalData)
    {
        //显示的英雄
        Hero hero = HeroStaticData.Instance.GetHeroById(heroLocalData.id);
        //显示图片
        Texture2D pic = (Texture2D)Resources.Load(hero.ImgPath);
        Sprite tmp = Sprite.Create(pic, new Rect(0, 0, pic.width, pic.height), new Vector2(0, 0));
        Image.sprite = tmp;
        Color ncolor = Image.color;
        ncolor.a = 1;
        Image.color = ncolor;
        //显示战力
        ATK.text = heroLocalData.ATK.ToString();
    }
}
