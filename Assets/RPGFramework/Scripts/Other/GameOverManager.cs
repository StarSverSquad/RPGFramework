using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [SerializeField]
    private RectTransform heart;
    [SerializeField]
    private RectTransform leftheart;
    [SerializeField]
    private RectTransform rightheart;

    [SerializeField]
    private AnimationCurve SpeedCurve;

    [SerializeField]
    private AudioSource music;
    [SerializeField]
    private AudioSource heartbreak;

    [SerializeField]
    private Image bg;

    private void Start()
    {
        StartCoroutine(GameOverCoroutine());
    }

    private IEnumerator GameOverCoroutine()
    {
        Vector2 deadPosition = new Vector2(PlayerPrefs.GetFloat("DeadX"), PlayerPrefs.GetFloat("DeadY"));

        heart.transform.position = deadPosition;

        bg.color = new Color(1, 1, 1, 0);

        yield return new WaitForSeconds(.5f);

        // Я такой даун, въебал 40 минут на то что бы самому разработать анимацию и забыл что у меня есть DoTween
        var anim0 = heart.transform.DOMove(Vector3.zero, 2).SetEase(Ease.InOutCubic).SetLoops(0).Play();

        yield return new WaitWhile(() => anim0.active);

        yield return new WaitForSeconds(.5f);

        leftheart.transform.position = Vector3.zero;
        rightheart.transform.position = Vector3.zero;

        heart.gameObject.SetActive(false);

        heartbreak.Play();

        var anim1 = leftheart.transform.DOMove(new Vector3(-2, 0, 0), 0.5f).SetEase(Ease.OutExpo).SetLoops(0).Play();
        var anim2 = rightheart.transform.DOMove(new Vector3(2, 0, 0), 0.5f).SetEase(Ease.OutExpo).SetLoops(0).Play();

        leftheart.GetComponent<Image>().DOColor(new Color(1, 1, 1, 0), 0.5f).SetLoops(0).Play();
        rightheart.GetComponent<Image>().DOColor(new Color(1, 1, 1, 0), 0.5f).SetLoops(0).Play();

        yield return new WaitWhile(() => anim1.active || anim2.active);

        yield return new WaitForSeconds(.25f);

        music.Play();

        bg.GetComponent<Image>().DOColor(new Color(1, 1, 1, 1), 2f).SetLoops(0).Play();

        yield return new WaitForSeconds(2);

        yield return new WaitUntil(() => Input.GetKeyDown(GameManager.Instance.GameConfig.Accept));

        bg.GetComponent<Image>().DOColor(new Color(1, 1, 1, 0), 2f).SetLoops(0).Play();

        yield return new WaitForSeconds(2.25f);

        GameManager.Instance.LoadGame(0);
    }
}
