using System.Collections;
using System;
using TMPro;
using UnityEngine;

public class TextTypewriter : MonoSingleton<TextTypewriter>
{
    public IEnumerator TypeText(
        TMP_Text target,
        string message,
        float secondsPerCharacter = 0.06f,
        Func<float> speedMultiplier = null,
        Func<bool> skipTyping = null)
    {
        if (target == null)
            yield break;

        target.text = message ?? string.Empty;
        target.ForceMeshUpdate();
        FitBackplate(target);
        int characterCount = target.textInfo.characterCount;
        target.maxVisibleCharacters = 0;

        float interval = Mathf.Max(0f, secondsPerCharacter);
        for (int visibleCharacters = 1; visibleCharacters <= characterCount; visibleCharacters++)
        {
            float elapsed = 0f;
            while (elapsed < interval)
            {
                if (target == null)
                    yield break;

                if (skipTyping != null && skipTyping())
                {
                    target.maxVisibleCharacters = characterCount;
                    yield break;
                }

                float multiplier = speedMultiplier != null ? Mathf.Max(0.01f, speedMultiplier()) : 1f;
                elapsed += Time.deltaTime * multiplier;
                yield return null;
            }

            if (target == null)
                yield break;

            if (skipTyping != null && skipTyping())
            {
                target.maxVisibleCharacters = characterCount;
                yield break;
            }

            target.maxVisibleCharacters = visibleCharacters;
        }
    }

    private static Sprite _backplateSprite;

    private static void FitBackplate(TMP_Text target)
    {
        if (!(target is TextMeshPro))
            return;

        Transform backplateTransform = target.transform.Find("Typewriter Backplate");
        if (backplateTransform == null)
        {
            GameObject backplateObject = new GameObject("Typewriter Backplate", typeof(SpriteRenderer));
            backplateTransform = backplateObject.transform;
            backplateTransform.SetParent(target.transform, false);
        }

        if (_backplateSprite == null)
        {
            _backplateSprite = Sprite.Create(
                Texture2D.whiteTexture,
                new Rect(0f, 0f, 1f, 1f),
                new Vector2(0.5f, 0.5f),
                1f);
            _backplateSprite.name = "Typewriter Backplate Sprite";
        }

        SpriteRenderer backplate = backplateTransform.GetComponent<SpriteRenderer>();
        backplate.sprite = _backplateSprite;
        backplate.color = new Color(0f, 0f, 0f, 0.8f);
        backplateTransform.localPosition = new Vector3(0f, 0f, 0.02f);
        backplateTransform.localScale = new Vector3(
            target.textBounds.size.x + 0.5f,
            target.textBounds.size.y + 0.25f,
            1f);

        Renderer textRenderer = target.GetComponent<Renderer>();
        if (textRenderer != null)
        {
            backplate.sortingLayerID = textRenderer.sortingLayerID;
            backplate.sortingOrder = textRenderer.sortingOrder - 1;
        }
    }
}
