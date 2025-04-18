using System;
using System.Collections.Generic;
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
    public MainWindow()
    {
        InitializeComponent();
        gridImages = SetupGrid();
        gameState = new GameState(rows, cols);
    }

    private void Window_Opened(object? sender, EventArgs e)
    {
        Draw();
    }

    private void Window_KeyDown(object? sender, KeyEventArgs e)
    {
        // Handle arrow keys here
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
        DrawGrid();
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