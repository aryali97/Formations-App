using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRepr {
    public float m;
    public float b;
    public Vector2? start;
    public Vector2? end;
    public int lineId;

    public LineRepr(float b) {
        this.m = float.PositiveInfinity;
        this.b = b;
        start = null;
        end = null;
    }

    public LineRepr(float m, float b) {
        this.m = m;
        this.b = b;
        start = null;
        end = null;
    }

    public LineRepr(float b, float start, float end) {
        this.m = float.PositiveInfinity;
        this.b = b;
        if (start <= end) {
            this.start = new Vector2(b, start);
            this.end = new Vector2(b, end);
        } else {
            this.end = new Vector2(b, start);
            this.start = new Vector2(b, end);
        }
    }

    public LineRepr(
        float m,
        float b,
        Vector2 start,
        Vector2 end) {
        this.m = m;
        this.b = b;
        if (start.x <= end.x) {
            this.start = start;
            this.end = end;
        } else {
            this.start = end;
            this.end = start;
        }
    }

    public Vector2 GetStart() {
        if (this.start == null) {
            return Vector2.zero;
        }
        return (Vector2)start;
    }

    public Vector2 GetEnd() {
        if (this.end == null) {
            return Vector2.zero;
        }
        return (Vector2)end;
    }

    public Vector2 ClosestPoint(Vector2 point) {
        if (this.m == float.PositiveInfinity) {
            if (this.start != null) {
                if (point.y < GetStart().y) {
                    return GetStart();
                } else if (point.y > GetEnd().y) {
                    return GetEnd();
                }
            }
            return new Vector2(this.b, point.y);
        } else if (this.m == 0.0f) {
            if (this.start != null) {
                if (point.x < GetStart().x) {
                    return GetStart();
                } else if (point.x > GetEnd().x) {
                    return GetEnd();
                }
            }
            return new Vector2(point.x, this.b);
        } else {
            // (Y - y) = m2 (X - x)
            // Y = m2*X + (y - m2*x)
            float m2 = -1.0f/this.m;
            float b2 = point.y - m2 * point.x;
            Vector2 intersection = new Vector2();
            if (Intersection(
                new LineRepr(this.m, this.b),
                new LineRepr(m2, b2),
                ref intersection)) {
            } else {
                Debug.LogError("Couldn't find intersection of lines");
            }
            if (this.start != null) {
                if (intersection.x < GetStart().x) {
                    return GetStart();
                } else if (intersection.x > GetEnd().x) {
                    return GetEnd();
                }
            }
            return intersection;
        }
    }

    public static bool Intersection(
        LineRepr l1,
        LineRepr l2,
        ref Vector2 intersection) {
        if (l1.m == float.PositiveInfinity &&
            l2.m == float.PositiveInfinity) {
            return false;
        } else if (l1.m != float.PositiveInfinity &&
            l2.m != float.PositiveInfinity) {
            if (l1.m == l2.m) {
                return false;
            }
            float posX = (l2.b - l1.b)/(l1.m - l2.m);
            if ((l1.start != null &&
                 (posX < l1.GetStart().x ||
                  posX > l1.GetEnd().x)) ||
                (l2.start != null &&
                 (posX < l2.GetStart().x ||
                  posX > l2.GetEnd().x))) {
                return false;
            }
            float posY = l1.m * posX + l1.b;
            intersection = new Vector2(posX, posY);
        } else {
            LineRepr vertL;
            LineRepr normL;
            if (l1.m == float.PositiveInfinity) {
                vertL = l1;
                normL = l2;
            } else {
                vertL = l2;
                normL = l1;
            }
            if (normL.start != null &&
                (vertL.b < normL.GetStart().x ||
                 vertL.b > normL.GetEnd().x)) {
                return false;
            }
            float posY = vertL.b * normL.m + normL.b;
            if (vertL.start != null &&
                (posY < vertL.GetStart().y ||
                 posY > vertL.GetEnd().y)) {
                return false;
            }
            intersection = new Vector2(vertL.b, posY);
        }
        return true;
    }
}
