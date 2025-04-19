using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace SnakeGame;

public partial class MainWindow : Window
{
    private readonly Dictionary<GridValue, IImage> gridValToImage = new()
    {
        { GridValue.Empty, Images.Empty },
        { GridValue.Snake, Images.Body },
        { GridValue.Food, Images.Food }
    };

    private readonly Dictionary<Direction, int> dirToRotation = new()
    {
        { Direction.Up, 0 },
        { Direction.Right, 90 },
        { Direction.Down, 180 },
        { Direction.Left, 270 }
    };
    
    private readonly int rows = 15, cols = 15;
    private readonly Image[,] gridImages;
    private GameState gameState;
    private bool gameRunning;
    public MainWindow()
    {
        InitializeComponent();
        gridImages = SetupGrid();
        gameState = new GameState(rows, cols);
        this.AddHandler(KeyDownEvent, Window_PreviewKeyDown, RoutingStrategies.Tunnel);
    }

    private async Task RunGame()
    {
        if (Design.IsDesignMode)
        {
            return;
        }

        Overlay.IsVisible = true;
        OverlayText.Text = "";
        this.Focus();
        Draw();

        await Task.Yield(); 
        await ShowCountDown();

        Overlay.IsVisible = false;

        await GameLoop();
        await ShowGameOver();
        gameState = new GameState(rows, cols);
    }

    private async void Window_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (!gameRunning)
        {
            gameRunning = true;
            await RunGame();
            gameRunning = false;
            return;
        }

        if (Overlay.IsVisible)
        {
            e.Handled = true;
        }
    }

    private void Window_KeyDown(object? sender, KeyEventArgs e)
    {
        if (gameState.GameOver)
        {
            return;
        }

        switch (e.Key)      
        {   
           case Key.Left:
               gameState.ChangeDirection(Direction.Left);
               break;
           case Key.Right:
               gameState.ChangeDirection(Direction.Right);
               break;
           case Key.Up:
               gameState.ChangeDirection(Direction.Up);
               break;
           case Key.Down:
               gameState.ChangeDirection(Direction.Down);
               break;
        }
    }

    private async Task GameLoop()
    {
        if (Design.IsDesignMode || gameState == null)
        {
            return;
        }

        while (!gameState.GameOver)
        {
            await Task.Delay(100);
            gameState.Move();
            Draw();
        }
    }
    private Image[,] SetupGrid()
    {
        Image[,] images = new Image[rows, cols];
        GameGrid.Rows = rows;
        GameGrid.Columns = cols;

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                Image image = new Image
                {
                    Source = Images.Empty,
                    RenderTransformOrigin = new RelativePoint(0.5, 0.5, RelativeUnit.Relative)                };
                
                images[r, c] = image;
                GameGrid.Children.Add(image);
            }
        }
        
        return images;
        
    }

    private void Draw()
    {
        if (Design.IsDesignMode || gameState == null)
        {
            return;
        }

        DrawGrid();
        DrawSnakeHead();
        ScoreText.Text = $"SCORE: {gameState.Score}";
    }

    private void DrawGrid()
    {
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                GridValue gridVal = gameState.Grid[r, c];
                gridImages[r, c].Source = gridValToImage[gridVal];
                gridImages[r, c].RenderTransform = null;
            }
        }
    }

    private void DrawSnakeHead()
    {
        Position headPos = gameState.HeadPosition();
        Image image = gridImages[headPos.Row, headPos.Col];
        image.Source = Images.Head;
        
        int rotation = dirToRotation[gameState.Dir];
        image.RenderTransform = new RotateTransform(rotation);
    }

    private async Task DrawDeadSnake()
    {
        List<Position> positions = new List<Position>(gameState.SnakePositions());

        for (int i = 0; i < positions.Count; i++)
        {
            Position pos = positions[i];
            IImage source = (i == 0) ? Images.DeadHead : Images.DeadBody;
            gridImages[pos.Row, pos.Col].Source = source;
            await Task.Delay(50);
        }
    }
    private async Task ShowCountDown()
    {
        for (int i = 3; i >= 1; i--)
        {
            OverlayText.Text = i.ToString();
            await Task.Delay(500);
        }
    }

    private async Task ShowGameOver()
    {
        await DrawDeadSnake();
        await Task.Delay(1000);
        Overlay.IsVisible = true;
        OverlayText.Text = "PRESS ANY KEY TO START";
    }
}