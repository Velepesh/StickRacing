using UnityEngine;

public class LineSmoother
{
    public static Vector3[] SmoothLine(Vector3[] inputPoints, float segmentSize)
    {
        AnimationCurve curveX = new AnimationCurve();
        AnimationCurve curveY = new AnimationCurve();
        AnimationCurve curveZ = new AnimationCurve();

        Keyframe[] keysX = new Keyframe[inputPoints.Length];
        Keyframe[] keysY = new Keyframe[inputPoints.Length];
        Keyframe[] keysZ = new Keyframe[inputPoints.Length];

        for (int i = 0; i < inputPoints.Length; i++)
        {
            keysX[i] = new Keyframe(i, inputPoints[i].x);
            keysY[i] = new Keyframe(i, inputPoints[i].y);
            keysZ[i] = new Keyframe(i, inputPoints[i].z);
        }

        curveX.keys = keysX;
        curveY.keys = keysY;
        curveZ.keys = keysZ;

        for (int i = 0; i < inputPoints.Length; i++)
        {
            curveX.SmoothTangents(i, 0);
            curveY.SmoothTangents(i, 0);
            curveZ.SmoothTangents(i, 0);
        }

        int segmentsCount = (int)(Vector3.Distance(inputPoints[0], inputPoints[inputPoints.Length - 1]) / segmentSize);
        Vector3[] lineSegments2 = new Vector3[segmentsCount];

        for (float i = 0; i < segmentsCount; i++)
        {
            float time = i / (segmentsCount / inputPoints.Length);
            Vector3 newSegment = new Vector3(curveX.Evaluate(time), curveY.Evaluate(time), curveZ.Evaluate(time));
            lineSegments2[(int)i] = newSegment;
        }

        return lineSegments2;
    }
}
