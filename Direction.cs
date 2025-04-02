using System;

namespace SnakeGame;

public class Direction
{

 public readonly static Direction Left = new Direction(0, -1);
 public readonly static Direction Right = new Direction(0, 1);
 public readonly static Direction Up = new Direction(-1, 0);
 public readonly static Direction Down = new Direction(1, 0);
 
 public int RowOffset { get; }   
 public int ColOffset { get; }

 private Direction(int rowOffset, int colOffset)
 {
  RowOffset = rowOffset;
  ColOffset = colOffset;
 }

 public Direction Opposite()
 {
  return new Direction(-RowOffset, -ColOffset);
 }
 protected bool Equals(Direction other)
 {
  return RowOffset == other.RowOffset && ColOffset == other.ColOffset;
 }

 public override bool Equals(object obj)
 {
  if (obj is null) return false;
  if (ReferenceEquals(this, obj)) return true;
  if (obj.GetType() != GetType()) return false;
  return Equals((Direction)obj);
 }

 public override int GetHashCode()
 {
  return HashCode.Combine(RowOffset, ColOffset);
 }

 public static bool operator ==(Direction left, Direction right)
 {
  return Equals(left, right);
 }

 public static bool operator !=(Direction left, Direction right)
 {
  return !Equals(left, right);
 }
}