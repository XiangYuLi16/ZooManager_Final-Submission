using System;

namespace ZooManager
{
    public class Mouse : Animal
    {
        private static Random rnd = new Random();

        public Mouse(string name)
        {
            emoji = "🐭";
            species = "mouse";
            this.name = name;
            reactionTime = 3; // reaction time of 1 (fast) to 3
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
                    if (occ != null && occ is Flowers)
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



        //private void EatFlower(Point location)
        //{
        //    if (Game.animalZones[location.y][location.x].occupant is Flowers)
        //    {
        //        Game.animalZones[location.y][location.x].occupant = null;
        //        Console.WriteLine($"Mouse ate the flower at {location.x},{location.y}");
        //    }
        //}
    }
}



