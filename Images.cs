using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;

namespace SnakeGame;

public static class Images
{
    public readonly static Bitmap Empty = LoadImage("Empty.png");
    public readonly static Bitmap Body = LoadImage("Body.png");
    public readonly static Bitmap Head = LoadImage("Head.png");
    public readonly static Bitmap Food = LoadImage("Food.png");
    public readonly static Bitmap DeadBody = LoadImage("DeadBody.png");
    public readonly static Bitmap DeadHead = LoadImage("DeadHead.png");

    private static Bitmap LoadImage(string fileName)
    {
        var uri = new Uri($"avares://SnakeGame/Assets/{fileName}");
        return new Bitmap(AssetLoader.Open(uri));
    }
}