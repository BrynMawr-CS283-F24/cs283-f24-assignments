using System;
using System.Drawing;

public class Car
{
    public Image carImage;
    public Point carPosition;
    public int speed = 50;

    public Car(string carImagePath, int startx, 
        int starty)
        //initialization the image and position
    {
        carImage = Image.FromFile(carImagePath);
        carPosition = new Point(startx, starty);
    }

    public void Draw (Graphics g)
    {
        g.DrawImage(carImage, carPosition.X, 
            carPosition.Y, 100, 100);

    }

    public void MoveLeft()
    {
        carPosition.X -= speed;
    }

    public void MoveRight()
    {
        carPosition.X += speed;             
    }

    // Check if the car collides with an obstacle
    public bool CheckCollision(Obstacle obstacle)
    {
        float distance  = (carPosition.X - obstacle.obstaclePosition.X)* 
            (carPosition.X - 
            obstacle.obstaclePosition.X) + (carPosition.Y - 
            obstacle.obstaclePosition.Y)*(carPosition.Y -
            obstacle.obstaclePosition.Y);
        distance = (float)Math.Sqrt(distance);
        return (distance <=60);
    }
}