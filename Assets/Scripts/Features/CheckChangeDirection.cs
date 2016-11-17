using UnityEngine;

namespace GG
{
    public class CheckChangeDirection
    {
        public static bool Check(FaceDirection _faceDirection, Vector2 position, float offsetY, string tag)
        {
            var direction = new Vector2(_faceDirection.Direction, 0);
            var finalPosition = new Vector2(position.x, position.y + offsetY);

            var hits = Physics2D.RaycastAll(finalPosition, direction, 0.5f);

            Debug.DrawRay(finalPosition, direction * 0.5f, Color.magenta);

            for (int i = 0; i < hits.Length; i++)
            {
                var hit = hits[i];

                if (hit.transform.tag == tag)
                {
                    _faceDirection.SetDirection(-(int)direction.x);
                    return true;
                }
            }

            return false;
        }
    }
}