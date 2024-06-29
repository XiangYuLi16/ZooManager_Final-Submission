using System;

namespace ZooManager
{
    public class Wolf : Animal
    {
        private static Random rnd = new Random();

        public Wolf(string name)
        {
            emoji = "🐺";
            species = "wolf";
            this.name = name;
            this.speed = 4; 
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
                    if (occ != null && occ is Cat)
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



