using UnityEngine;

public class BoxNum : MonoBehaviour
{
    public enum Color
    {
        red, green, black,
    }
    [Header("Ç±ÇÃÉ}ÉXÇÃî‘çÜ")]
    public string pocketNumber = "0";
    public Color color = Color.red;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Bool ball = other.GetComponent<Bool>();

            if (ball != null)
            {
                ball.SetPocket(pocketNumber,color.ToString());
            }
        }
    }
}
