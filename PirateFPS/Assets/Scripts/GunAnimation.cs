using UnityEngine;

public class GunAnimation : MonoBehaviour
{
    [Header("Idle Bob")]
    public float bobAmount = 20f;
    public float bobSpeed = 3f;

    [Header("Recoil")]
    public float recoilDistance = 100f;
    public float recoilSpeed = 45f;
    public float returnSpeed = 10f;

    private RectTransform rect;
    private Vector2 startPos;
    private Vector2 recoilOffset;
    private float bobTimer;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        startPos = rect.anchoredPosition;
    }

    void Update()
    {
        // Idle bob
        bobTimer += Time.deltaTime * bobSpeed;
        float bobY = Mathf.Sin(bobTimer) * bobAmount;

        // Smooth recoil return
        recoilOffset = Vector2.Lerp(
            recoilOffset,
            Vector2.zero,
            returnSpeed * Time.deltaTime
        );

        rect.anchoredPosition = startPos + recoilOffset + new Vector2(0f, bobY);
    }

    /// Call this when the gun fires
    public void PlayRecoil()
    {
        recoilOffset += new Vector2(0f, -recoilDistance);
    }
}
