using System;
namespace ZooManager
{
    public class Animal : Occupant
    {
        public string name;
        public int reactionTime = 5; // default reaction time for animals (1 - 10)

        public int speed = 2;  // default speed

        public bool alive = true; // alive or not

        public bool active = true;

        public Direction direction = Direction.up;


        virtual public void Activate()
        {
            Console.WriteLine($"Animal {name} at {location.x},{location.y} activated");
        }

        protected Direction DetermineDirection(int dx, int dy)
        {
            if (Math.Abs(dx) > Math.Abs(dy))
            {
                return dx > 0 ? Direction.right : Direction.left;
            }
            else
            {
                return dy > 0 ? Direction.down : Direction.up;
            }
        }

        virtual public Point FindTarget()
        {
            return new Point { x = -1, y = -1 };
        }

        virtual public void Move()
        {

            for (int i = 0; i < speed; i++)
            {
                if (!alive || !active)
                {
                    return;
                }
                // Find the closest flower
                Point target = FindTarget();
                if (target.x == -1 && target.y == -1) return; // No flower found

                // Determine the direction to move
                int dx = target.x - location.x;
                int dy = target.y - location.y;

                Direction direction = DetermineDirection(dx, dy);

                // Move the mouse one step in the chosen direction
                MoveOneStep(direction);

            }


        }

        protected void MoveOneStep(Direction direction)
        {
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


            Game.interact(this, y, x);
        }

        override public string ToString()
        {
            return species + "(" + location + ")";
        }

        public void NewRound()
        {
            if (alive)
            {
                active = true;
            }
        }

        static public int BoardDistance(Point location1, Point location2)
        {
            return Math.Abs(location1.x - location2.x) + Math.Abs(location1.y - location2.y);
        }
    }
}