using System;

namespace SnakeGame;

public class Position
{
    public int Row { get; }
    public int Col { get; }

    public Position(int row, int col)
    {
        Row = row;
        Col = col;
    }

    public Position Translate(Direction dir)
    {
        return new Position(Row + dir.RowOffset, Col + dir.ColOffset);
    }

    protected bool Equals(Position other)
    {
        return Row == other.Row && Col == other.Col;
    }

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Position)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Row, Col);
    }

    public static bool operator ==(Position left, Position right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Position left, Position right)
    {
        return !Equals(left, right);
    }
}