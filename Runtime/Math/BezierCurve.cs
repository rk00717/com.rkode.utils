using System.Collections.Generic;
using UnityEngine;

namespace RKode.Utils.Math {
public class BezierCurve {
    public List<Vector3> points;
    private bool _isClosed = false;

    public int TotalPoints => points.Count;
    public int TotalSegment => points.Count / 3;
    private float _resolution = 100f;

    public Vector3 this[int i] => points[i];

    public float Resolution {
        get => _resolution;
        set => _resolution = value;
    }

    public bool IsClosed {
        get => _isClosed;
        set {
            if(_isClosed != value) {
                _isClosed = value;

                if(_isClosed) {
                    points.Add(points[points.Count - 1] * 2 - points[points.Count - 2]);
                    points.Add(points[0] * 2 - points[1]);
                }else {
                    points.RemoveRange(points.Count - 2, 2);
                }
            }
        }
    }

    public BezierCurve(List<Vector3> points, float resolution) {
        this.points = points;
        _resolution = resolution;
    }

    public void SetPosition(int index, Vector3 position) {
        points[index] = position;
    }

    public void AddSegment(Vector3 anchorPosition) {
        points.Add(points[points.Count - 1] * 2 - points[points.Count - 2]);
        points.Add((points[points.Count-1] + anchorPosition) * .5f);
        points.Add(anchorPosition);
    }

    public void SplitSegment(Vector3 anchorPos, int segmentIndex) {
        points.InsertRange(segmentIndex * 3 + 2, new Vector3[] {
            Vector2.zero, anchorPos, Vector2.zero
        });
    }

    public void DeleteSegment(int anchorIndex) {
        if(TotalSegment > 2 || !_isClosed && TotalSegment > 1) {
            if(anchorIndex == 0) {
                if(_isClosed) {
                    points[points.Count - 1] = points[2];
                }

                points.RemoveRange(0, 3);
            }else if(anchorIndex == points.Count - 1 && !_isClosed) {
                points.RemoveRange(anchorIndex - 2, 3);
            }else {
                points.RemoveRange(anchorIndex - 1, 3);
            }
        }
    }

    public Vector3[] GetPointsInSegment(int index) {
        if(index >= points.Count-1 || index < 0) {
            Debug.LogWarning("Not Enough Points to get the segment");
            return null;
        }

        return new Vector3[] {
            points[index * 3],
            points[index * 3 + 1],
            points[index * 3 + 2],
            points[LoopIndex(index * 3 + 3)]
        };
    }

    public int LoopIndex(int i) {
        return (i + points.Count) % points.Count;
    }

    public List<Vector3> GetSamplePointsOnCurve() {
        List<Vector3> sampledPoints = new List<Vector3>();

        for (int i = 0; i < TotalSegment; i++) {
            Vector3[] p = GetPointsInSegment(i);
            int divisions = Mathf.CeilToInt(_resolution * 100f);

            for (int j = 0; j <= divisions; j++) {
                float t = j / (float)divisions;
                sampledPoints.Add(EvaluateCubicBezier(p[0], p[1], p[2], p[3], t));
            }
        }

        return sampledPoints;
    }

    public float CalculateTotalLength(List<Vector3> sampledPoints) {
        float totalLength = 0f;
        for (int i = 1; i < sampledPoints.Count; i++) {
            totalLength += Vector3.Distance(sampledPoints[i - 1], sampledPoints[i]);
        }

        return totalLength;
    }

    public List<Vector3> CalculateEvenlySpacedPoints(float spacing) {
        var sampledPoints = GetSamplePointsOnCurve();
        List<Vector3> evenlySpacedPoints = new List<Vector3>{sampledPoints[0]};
        Vector3 previousPoint = sampledPoints[0];
        float distanceSinceLastPoint = 0;

        // for(int i = 0; i < TotalSegment; i++) {
        //     Vector3[] p = GetPointsInSegment(i);
        //     float controlNetLength = Vector3.Distance(p[0], p[1]) + Vector3.Distance(p[1], p[2]) + Vector3.Distance(p[2], p[3]);
        //     float estimatedCurveLength = Vector3.Distance(p[0], p[3]) + controlNetLength * .5f;
        //     int divisions = Mathf.CeilToInt(estimatedCurveLength * (_resolution * 10));

        //     float t = 0;
        //     while (t <= 1) {
        //         t += 1f/divisions;
        //         Vector3 pointOnCurve = EvaluateCubicBezier(p[0], p[1], p[2], p[3], t);
        //         distanceSinceLastEvenPoint += Vector3.Distance(previousPoint, pointOnCurve);
        //         if(distanceSinceLastEvenPoint >= spacing) {
        //             float overShootDistance = distanceSinceLastEvenPoint - spacing;
        //             Vector3 newEvenlySpacedPoint = pointOnCurve + (previousPoint - pointOnCurve).normalized * overShootDistance;

        //             evenlySpacedPoints.Add(newEvenlySpacedPoint);

        //             distanceSinceLastEvenPoint = overShootDistance;
        //             previousPoint = newEvenlySpacedPoint;
        //         }
        //     }
        // }

        for (int i = 1; i < sampledPoints.Count; i++) {
            float segmentLength = Vector3.Distance(previousPoint, sampledPoints[i]);
            distanceSinceLastPoint += segmentLength;

            if (distanceSinceLastPoint >= spacing) {
                float overshoot = distanceSinceLastPoint - spacing;
                Vector3 direction = (sampledPoints[i] - sampledPoints[i - 1]).normalized;
                Vector3 newPoint = sampledPoints[i] - direction * overshoot;

                evenlySpacedPoints.Add(newPoint);
                previousPoint = newPoint;
                distanceSinceLastPoint = overshoot;
            } else {
                previousPoint = sampledPoints[i];
            }
        }

        return evenlySpacedPoints;
    }

    
    // Quateratic Curve => (1-t)^2A + 2(1-t)tB + t^2C
    public Vector3 EvaluateQuadraticBezier(Vector3 pt0, Vector3 pt1, Vector3 pt2, float t) {
        float n = 1 - t;
        return (
            (n * n) * pt0 + 
            2 * n * t * pt1 +
            (t * t) * pt2
        );
    }

    // Cubic Curve => (1-t)^3A + 3(1-t)^2tB + 3(1-t)t^2C + t^3D
    public Vector3 EvaluateCubicBezier(Vector3 pt0, Vector3 pt1, Vector3 pt2, Vector3 pt3, float t) {
        float n = 1 - t;
        return (
            (n * n * n) * pt0 + 
            3 * (n * n) * t * pt1 +
            3 * n * (t * t) * pt2 +
            (t * t * t) * pt3
        );
    }
}
}