using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RandomizeSpriteFlip : MonoBehaviour
{
    private enum FlipState
    {
        FlipX,
        FlipY,
        FlipBoth
    }

    [SerializeField] private FlipState currentFlipState;
    [SerializeField] SpriteRenderer spriteToMatch;
    private SpriteRenderer _sRend;

    private void Awake() => _sRend = GetComponent<SpriteRenderer>();

    private void OnEnable()
    {
        bool flip = Random.value >= .5f;
        switch (currentFlipState)
        {
            case FlipState.FlipX:
                _sRend.flipX = FlipSprite(flip);
                if (spriteToMatch != null)
                    spriteToMatch.flipX = _sRend.flipX;
                break;

            case FlipState.FlipY:
                    _sRend.flipY = FlipSprite(flip);
                if (spriteToMatch != null)
                    spriteToMatch.flipY = _sRend.flipY;
                break;

            case FlipState.FlipBoth:
                if (spriteToMatch != null) return;
                bool flipOther = Random.value >= .5f;
                _sRend.flipX = FlipSprite(flip);
                _sRend.flipY = FlipSprite(flipOther);
                break;
        }
    }

    private bool FlipSprite(bool flipValue)
    {
        if (flipValue)
            return true;
        return false;
    }
}