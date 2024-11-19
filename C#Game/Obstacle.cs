using System;
using System.Drawing;

public class Obstacle
{
    public Image obstacleImage;    
    public Point  obstaclePosition;         

    // initializes obstacle 
    public Obstacle(string obstacleImagePath, int startX, int startY)
    {
        obstacleImage = Image.FromFile(obstacleImagePath);
        obstaclePosition = new Point(startX, startY);   // Starting position
    }

    // Draw the obstacle on the screen
    public void Draw(Graphics g)
    {
        g.DrawImage(obstacleImage, obstaclePosition.X, obstaclePosition.Y, 50, 50);   
    }
}
