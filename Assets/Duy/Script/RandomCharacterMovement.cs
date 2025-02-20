using System.Collections;
using UnityEngine;

public class RandomCharacterMovement : MonoBehaviour
{
    public CharacterMove character1;  // Link đến script của nhân vật 1
    public CharacterMove1 character2;  // Link đến script của nhân vật 2

    public float minTime = 2f;  // Thời gian ngẫu nhiên tối thiểu giữa các lần di chuyển
    public float maxTime = 5f;  // Thời gian ngẫu nhiên tối đa giữa các lần di chuyển

    private void Start()
    {
        StartCoroutine(RandomMovement());
    }

    private IEnumerator RandomMovement()
    {
        while (true)
        {
            // Lựa chọn ngẫu nhiên xem có phải nhân vật 1, nhân vật 2 hoặc cả 2 cùng di chuyển
            float randomTime = Random.Range(minTime, maxTime);

            // Chờ trong khoảng thời gian ngẫu nhiên
            yield return new WaitForSeconds(randomTime);

            // Random quyết định xem nhân vật nào di chuyển
            int randomCharacter = Random.Range(0, 3);  // 0 - không ai di chuyển, 1 - nhân vật 1 di chuyển, 2 - nhân vật 2 di chuyển, 3 - cả 2 di chuyển

            switch (randomCharacter)
            {
                case 0:
                    // Không ai di chuyển
                    break;
                case 1:
                    // Chỉ nhân vật 1 di chuyển
                    character1.StartMovement();
                    break;
                case 2:
                    // Chỉ nhân vật 2 di chuyển
                    character2.StartMovement();
                    break;
                case 3:
                    // Cả 2 nhân vật di chuyển
                    character1.StartMovement();
                    character2.StartMovement();
                    break;
            }
        }
    }
}
