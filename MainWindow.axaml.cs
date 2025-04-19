using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        Overlay.IsVisible = false;
        this.Focus();
        Draw();

        await Task.Yield(); 
        await GameLoop();
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
                    Source = Images.Empty
                };
                
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
            }
        }
    }
}