using System;
namespace ZooManager
{
    public class GardenKeeper : Animal
    {
        public GardenKeeper(string name)
        {
            emoji = "🚶‍♂️";
            species = "gardenKeeper";
            this.name = name;
            reactionTime = new Random().Next(6, 10); // reaction time 6 to 9 (slow)
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
                    if (occ != null && occ is Wolf)
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


        override public void Move()
        {

            for (int i = 0; i < speed; i++)
            {
                if (!alive || !active)
                {
                    return;
                }
                // Find the closest flower
                Point target = FindTarget();
                if (target.x == -1 && target.y == -1) return; // No target found

                // Determine the direction to move
                int dx = target.x - location.x;
                int dy = target.y - location.y;

                Direction direction = DetermineDirection(dx, dy);

                // Move the mouse one step in the chosen direction
                int x = location.x;
                int y = location.y;

                switch (direction)
                {
                    case Direction.up:
                        if (y > 0) y--;
                        break;
                    case Direction.down:
                        if (y < Game.numCellsY - 1) y++;
                        break;
                    case Direction.left:
                        if (x > 0) x--;
                        break;
                    case Direction.right:
                        if (x < Game.numCellsX - 1) x++;
                        break;
                }

                if (!Game.IsGardenZone(y, x))
                {
                    // not move to the new place if out of bound
                    y = location.y;
                    x = location.x;
                }


                Game.interact(this, y, x);

            }


        }


    }
}



