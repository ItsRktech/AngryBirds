using UnityEngine;
using DG.Tweening;
using System.Collections;

public class RandomizedPath : MonoBehaviour
{
    [SerializeField] private Vector3[] _pathPoints = new Vector3[3];
    [SerializeField] private float _duration = 2f;

    // Store the original points so the randomness doesn't "drift" too far away
    private Vector3[] _basePoints;

    void Start()
    {
        // Clone the original points so we always have a reference to the 'ideal' path
        _basePoints = (Vector3[])_pathPoints.Clone();

        LaunchPath();
    }

    void LaunchPath()
    {
        // 1. Randomize every point in the array (except the start point at index 0)
        for (int i = 0; i < _pathPoints.Length; i++)
        {
            float randX = Random.Range(-3f, 3f);
            float randY = Random.Range(-3f, 3f);

            // Apply the random offset to the original base point
            _pathPoints[i] = new Vector3(_basePoints[i].x + randX, _basePoints[i].y + randY, 0);
        }

        // 2. Execute the path
        transform.DOPath(_pathPoints, _duration, PathType.CatmullRom)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                // 3. Reset position to start and loop manually
                ResetPosition();
            });
    }

    void ResetPosition()
    {
        // This acts like "LoopType.Restart"
        // If it's a UI element, you might need to store the starting AnchoredPosition
        transform.localPosition = _pathPoints[0];
        DOVirtual.DelayedCall(10.0f, LaunchPath);
    }
}