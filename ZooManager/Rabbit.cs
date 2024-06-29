using System;
namespace ZooManager
{
    public class Rabbit : Animal
    {
        public Rabbit()
        {
            this.emoji = "🐇";
            this.species = "rabbit";
        }


        override public Point FindTarget()
        {
            Point closest = new Point { x = -1, y = -1 };
            double minDistance = double.MaxValue;

            for (int y = 0; y < Game.numCellsY; y++)
            {
                for (int x = 0; x < Game.numCellsX; x++)
                {
                    if (Game.animalZones[y][x].occupant is Cat)
                    {
                        double distance = Math.Sqrt(Math.Pow(x - location.x, 2) + Math.Pow(y - location.y, 2));
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            closest = new Point { x = x, y = y };
                        }
                    }
                }
            }

            return closest;
        }
    }
}

