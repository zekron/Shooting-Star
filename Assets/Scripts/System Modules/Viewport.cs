using UnityEngine;

public class Viewport
{
    private static float minX;
    private static float maxX;
    private static float minY;
    private static float maxY;
    private static float middleX;
    private static float middleY;

    public static float MinX => minX;
    public static float MaxX => maxX;
    public static float MinY => minY;
    public static float MaxY => maxY;

    public static void Initialize()
    {
        Camera mainCamera = Camera.main;

        Vector2 bottomLeft = mainCamera.ViewportToWorldPoint(Vector3.zero);
        Vector2 topRight = mainCamera.ViewportToWorldPoint(Vector3.one);

        middleX = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0f)).x;
        middleY = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0.5f)).y;

        minX = bottomLeft.x;
        minY = bottomLeft.y;
        maxX = topRight.x;
        maxY = topRight.y;
    }

    public static Vector3 PlayerMoveablePosition(Vector3 playerPosition, float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;

        position.x = Mathf.Clamp(playerPosition.x, minX + paddingX, maxX - paddingX);
        position.y = Mathf.Clamp(playerPosition.y, minY + paddingY, maxY - paddingY);

        return position;
    }

    public static Vector3 RandomEnemySpawnPosition(float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;

        position.x = Random.Range(minX + paddingX, maxX - paddingX);
        position.y = maxY + paddingY;

        return position;
    }

    public static Vector3 RandomTopHalfPosition(float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;

        position.x = Random.Range(minX + paddingX, maxX - paddingX);
        position.y = Random.Range(middleY, maxY - paddingY);

        return position;
    }

    public static Vector3 RandomPosition(float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;

        position.x = Random.Range(minX + paddingX, maxX - paddingX);
        position.y = Random.Range(minY + paddingY, maxY - paddingY);

        return position;
    }

    public static Vector3 RandomEnemyMovePosition(float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;

        position.x = Random.Range(minX + paddingX, maxX - paddingX);
        position.y = Random.Range(minY + paddingY, maxY - paddingY);

        return position;
    }
}