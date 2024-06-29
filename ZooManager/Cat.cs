using System;

namespace ZooManager
{
    public class Cat : Animal
    {
        public Cat(string name)
        {
            emoji = "🐱";
            species = "cat";
            this.name = name;
            this.speed = 3; // speed
        }


        override public Point FindTarget()
        {
            Point closest = new Point { x = -1, y = -1 };
            double minDistance = double.MaxValue;

            for (int y = 0; y < Game.numCellsY; y++)
            {
                for (int x = 0; x < Game.numCellsX; x++)
                {
                    Occupant occ = Game.animalZones[y][x].occupant;
                    if (occ != null && occ is Mouse)
                    {
                        double distance = BoardDistance(new Point { x = x, y = y }, occ.location);
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




