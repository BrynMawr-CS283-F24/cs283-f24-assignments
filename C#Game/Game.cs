using System;
using System.Drawing;
using System.Windows.Forms;

public class Game
{
    private Car playerCar;
    private Obstacle[] obstacles;
    private int obstaclesPassed = 0;
    private bool gameOver = false;
    private bool gameWin = false;
    private Image backgroundImage;
    private bool creditBox = false;

    //call once before the game loop starts
    public void Setup()
    {
        backgroundImage = Image.FromFile("background image.jpg");

        // Generate 20 random obstacles
        obstacles = new Obstacle[10];
        Random random = new Random();
        for (int i = 0; i < 10; i++)
        {
            int x = random.Next(50, Window.width - 50);
            int y = i * -100;                         
            obstacles[i] = new Obstacle("obstacle.png", x, y); 
        }

        playerCar = new Car("car image.png", 
            Window.width / 2 - 25, Window.height - 80);


    }

    //call onece per frame with the elapsed time in seconds
    public void Update(float dt)
    {
 
        if (!gameOver && !gameWin)
        {
            // Obstacles continue to move down
            for (int i = 0; i < obstacles.Length; i++)
            {
                obstacles[i].obstaclePosition.Y += 5;

                // Check for collisions 
                if (playerCar.CheckCollision(obstacles[i]))
                {
                    gameOver = true;
                    break;
                }

                // Passes, point ++
                if (obstacles[i].obstaclePosition.Y > playerCar.carPosition.Y)
                {
                    obstaclesPassed++;
                    obstacles[i].obstaclePosition.Y = -100;  // Reset obstacle 
                    obstacles[i].obstaclePosition.X = new Random().Next(50, Window.width - 50);  // Randomize new X position
                }
            }

            // Check if player passed all the obstacles
            if (obstaclesPassed >= 20 && !gameOver)
            {
                gameWin = true;
            }
        }
    }

    //called when the window is refreshed
    public void Draw(Graphics g)
    {
        // Draw the background 
        g.DrawImage(backgroundImage, 0, 0, Window.width, Window.height);

        // Draw the obstacles
        for (int i = 0; i < obstacles.Length; i++)
        {
            obstacles[i].Draw(g);
        }

        // Draw the car
        playerCar.Draw(g);

        Font font = new Font("Arial", 20);
        SolidBrush fontBrush = new SolidBrush(Color.White);

        StringFormat format = new StringFormat();
        format.LineAlignment = StringAlignment.Center;
        format.Alignment = StringAlignment.Center;
        g.DrawString("Your score is " + obstaclesPassed, font, fontBrush, (float)(Window.width * 0.15),
      (float)(Window.height * 0.1), format);
       
        if (creditBox)
        {
            g.DrawString("Ranty Wang, 2026, Car God ", font, fontBrush, (float)(Window.width * 0.22),
(float)(Window.height * 0.9), format);

        }

        // Display win or lose messages
        if (gameOver)
        {
            g.DrawString("Fail!", new Font("Arial", 45), Brushes.Red, new Point(Window.width / 2 - 50, Window.height / 2 - 50));
        }
        else if (gameWin)
        {
            g.DrawString("You're Car God!", new Font("Arial", 45), Brushes.White, new Point(Window.width / 2 - 230, Window.height / 2 - 50));
        }

    }

    
     //called when user clicks a mouse button
    public void MouseClick(MouseEventArgs mouse)
    {
        if (mouse.Button == MouseButtons.Left)
        {
            System.Console.WriteLine(mouse.Location.X + ", " + mouse.Location.Y);
        }
    }
   

    public void KeyDown(KeyEventArgs key)
    {
        if (!gameOver && !gameWin)
        {
            if (key.KeyCode == Keys.A || key.KeyCode == Keys.Left)
            {
                playerCar.MoveLeft();
            }
            else if (key.KeyCode == Keys.D || key.KeyCode == Keys.Right)
            {
                playerCar.MoveRight();
            }
        }
        if (key.KeyCode == Keys.Oemplus)
        {
            creditBox = true;
        }
        {

        }
    }


}
    
