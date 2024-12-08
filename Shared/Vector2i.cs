namespace Shared;

public struct Vector2i(int x, int y) : IEquatable<Vector2i>
{
    public int X = x;
    public int Y = y;

    public static Vector2i operator +(Vector2i left, Vector2i right)
    {
        return new Vector2i(left.X + right.X, left.Y + right.Y);
    }

    public static Vector2i operator -(Vector2i left, Vector2i right)
    {
        return new Vector2i(left.X - right.X, left.Y - right.Y);
    }

    public static Vector2i operator *(Vector2i left, int right)
    {
        return new Vector2i(left.X * right, left.Y * right);
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }

    public bool Equals(Vector2i other)
    {
        return X == other.X && Y == other.Y;
    }

    public override bool Equals(object? obj)
    {
        return obj is Vector2i other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public static bool operator ==(Vector2i left, Vector2i right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Vector2i left, Vector2i right)
    {
        return !left.Equals(right);
    }
}
