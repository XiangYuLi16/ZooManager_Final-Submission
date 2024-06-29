using System;
using System.Collections.Generic;

namespace ZooManager
{
    public static class Game
    {
        static public int numCellsX = 11;
        static public int numCellsY = 11;
        static private int maxCellsX = 11;
        static private int maxCellsY = 11;
        static private int roundCounter = 0;
        static public bool gameWon = false;

        static public bool gameLost = false;

        // 
        public const int catCDTime = 2;
        static public int catRemainingRound = 0;

        public const int keeperCDTime = 4;
        static public int keeperRemainingRound = 1;

        public const int trapCDTime = 1;
        static public int trapRemainingRound = 0;

        public const int mouseCDTime = 2;
        static public int mouseRemainingRound = 0;

        public const int wolfCDTime = 4;
        static public int wolfRemainingRound = 3;

        public const int flowerCDTime = 2;
        static public int flowerRemainingRound = 0;


        static public List<List<Zone>> animalZones = new List<List<Zone>>();
        static public Zone holdingPen = new Zone(-1, -1, null);
        static private List<Mouse> mouses = new List<Mouse>();
        static private List<Wolf> wolves = new List<Wolf>();

        static private List<Cat> cats = new List<Cat>();

        static private List<GardenKeeper> keepers = new List<GardenKeeper>();

        static public void SetUpGame()
        {
            gameWon = false;
            gameLost = false;
            animalZones.Clear();
            for (var y = 0; y < numCellsY; y++)
            {
                List<Zone> rowList = new List<Zone>();
                for (var x = 0; x < numCellsX; x++) rowList.Add(new Zone(x, y, null));
                animalZones.Add(rowList);
            }

            // Place the initial flower in the middle of the 3x3 grid
            //animalZones[5][5].occupant = new Flowers();
            //Console.WriteLine("Placed initial flower at 5,5");

            //// Spawn the initial mouse at round 0
            //mouse = new Mouse("Mouse1");
            //animalZones[0][0].occupant = mouse;
            //mouse.location = new Point { x = 0, y = 0 };
            mouses.Clear();
            cats.Clear();
            wolves.Clear();
            keepers.Clear();

            catRemainingRound = 0;


            keeperRemainingRound = 3;


            trapRemainingRound = 0;


            mouseRemainingRound = 0;


            wolfRemainingRound = 3;


            flowerRemainingRound = 0;
            NextRound();

        }

        static public void AddZones(Direction d)
        {
            if (d == Direction.down || d == Direction.up)
            {
                if (numCellsY >= maxCellsY) return; // hit maximum height!
                List<Zone> rowList = new List<Zone>();
                for (var x = 0; x < numCellsX; x++)
                {
                    rowList.Add(new Zone(x, numCellsY, null));
                }
                numCellsY++;
                if (d == Direction.down) animalZones.Add(rowList);
            }
            else
            {
                if (numCellsX >= maxCellsX) return; // hit maximum width!
                for (var y = 0; y < numCellsY; y++)
                {
                    var rowList = animalZones[y];
                    if (d == Direction.right) rowList.Add(new Zone(numCellsX, y, null));
                }
                numCellsX++;
            }
        }

        static public void HoldingZoneClick()
        {
            if (holdingPen.occupant != null)
            {
                holdingPen.occupant = null;

            }

        }

        static private void UpdateCDTime(Occupant occupant)
        {
            if (occupant is Cat)
            {
                cats.Add((Cat)occupant);
                catRemainingRound = catCDTime;
            }

            if (occupant is GardenKeeper)
            {
                keepers.Add((GardenKeeper)occupant);
                keeperRemainingRound = keeperCDTime;
            }

            if (occupant is Trap)
            {
                trapRemainingRound = trapCDTime;
            }


        }

        static public void ZoneClick(Zone clickedZone)
        {
            if (gameWon) return;

            Console.Write("Got animal ");
            Console.WriteLine(clickedZone.emoji == "" ? "none" : clickedZone.emoji);
            Console.Write("Held animal is ");
            Console.WriteLine(holdingPen.emoji == "" ? "none" : holdingPen.emoji);
            if (clickedZone.occupant != null) clickedZone.occupant.ReportLocation();


            if (holdingPen.occupant != null && clickedZone.occupant == null)
            {
                Console.WriteLine("Placing " + holdingPen.emoji);
                clickedZone.occupant = holdingPen.occupant;
                clickedZone.occupant.location = clickedZone.location;
                holdingPen.occupant = null;
                Console.WriteLine("Empty spot now holds: " + clickedZone.emoji);
                UpdateCDTime(clickedZone.occupant);

                ActivateAnimals();
            }
            else if (holdingPen.occupant != null && clickedZone.occupant != null)
            {
                Console.WriteLine("Could not place animal.");
            }
            else
            {
                Console.WriteLine("Holding pen is empty, place nothing.");
            }
        }

        static public void AddToHolding(string occupantType)
        {
            if (gameWon) return;

            if (holdingPen.occupant != null) return;
            if (occupantType == "flowers") return; // prevent players from adding flowers manually
            if (occupantType == "cat")
            {
                holdingPen.occupant = new Cat("cat");
            }
            else if (occupantType == "rabbit")
            {
                holdingPen.occupant = new Rabbit();
            }
            else if (occupantType == "trap")
            {
                holdingPen.occupant = new Trap();
            }
            else if (occupantType == "gardenKeeper")
            {
                holdingPen.occupant = new GardenKeeper("keeper");
            }
            Console.WriteLine($"Holding pen occupant at {holdingPen.occupant.location.x},{holdingPen.occupant.location.y}");
        }

        static public void ActivateAnimals()
        {
            for (var r = 1; r < 11; r++)
            {
                for (var y = 0; y < numCellsY; y++)
                {
                    for (var x = 0; x < numCellsX; x++)
                    {
                        var zone = animalZones[y][x];
                        if (zone.occupant as Animal != null && ((Animal)zone.occupant).reactionTime == r)
                        {
                            ((Animal)zone.occupant).Activate();
                        }
                    }
                }
            }
        }

        static public bool Seek(int x, int y, Direction d, string target)
        {
            switch (d)
            {
                case Direction.up:
                    y--;
                    break;
                case Direction.down:
                    y++;
                    break;
                case Direction.left:
                    x--;
                    break;
                case Direction.right:
                    x++;
                    break;
            }
            if (y < 0 || x < 0 || y > numCellsY - 1 || x > numCellsX - 1) return false;
            if (animalZones[y][x].occupant == null) return false;
            if (animalZones[y][x].occupant.species == target)
            {
                return true;
            }
            return false;
        }

        static public void Attack(Animal attacker, Direction d)
        {
            if (gameWon) return;

            Console.WriteLine($"{attacker.name} is attacking {d.ToString()}");
            int x = attacker.location.x;
            int y = attacker.location.y;

            switch (d)
            {
                case Direction.up:
                    animalZones[y - 1][x].occupant = attacker;
                    break;
                case Direction.down:
                    animalZones[y + 1][x].occupant = attacker;
                    break;
                case Direction.left:
                    animalZones[y][x - 1].occupant = attacker;
                    break;
                case Direction.right:
                    animalZones[y][x + 1].occupant = attacker;
                    break;
            }
            animalZones[y][x].occupant = null;
        }

        static public bool Retreat(Animal runner, Direction d)
        {
            if (gameWon) return false;

            Console.WriteLine($"{runner.name} is retreating {d.ToString()}");
            int x = runner.location.x;
            int y = runner.location.y;

            switch (d)
            {
                case Direction.up:
                    if (y > 0 && animalZones[y - 1][x].occupant == null)
                    {
                        animalZones[y - 1][x].occupant = runner;
                        animalZones[y][x].occupant = null;
                        return true;
                    }
                    return false;
                case Direction.down:
                    if (y < numCellsY - 1 && animalZones[y + 1][x].occupant == null)
                    {
                        animalZones[y + 1][x].occupant = runner;
                        animalZones[y][x].occupant = null;
                        return true;
                    }
                    return false;
                case Direction.left:
                    if (x > 0 && animalZones[y][x - 1].occupant == null)
                    {
                        animalZones[y][x - 1].occupant = runner;
                        animalZones[y][x].occupant = null;
                        return true;
                    }
                    return false;
                case Direction.right:
                    if (x < numCellsX - 1 && animalZones[y][x + 1].occupant == null)
                    {
                        animalZones[y][x + 1].occupant = runner;
                        animalZones[y][x].occupant = null;
                        return true;
                    }
                    return false;
            }
            return false;
        }

        static private void PlaceMouse()
        {
            Random rnd = new Random();
            List<Point> points = new List<Point>();
            for (int r = 0; r < numCellsY; r++)
            {
                for (int c = 0; c < numCellsX; c++)
                {

                    if (!IsGardenZone(r, c) && animalZones[r][c].occupant == null)
                    {
                        points.Add(new Point { y = r, x = c });
                    }
                }
            }
            if (points.Count == 0)
            {
                return;
            }

            int index = rnd.Next(0, points.Count);
            Point target = points[index];
            animalZones[target.y][target.x].occupant = new Mouse("m")
            {
                location = target
            };

            mouses.Add((Mouse)animalZones[target.y][target.x].occupant);

        }

        static private void PlaceWolf()
        {
            Random rnd = new Random();
            List<Point> points = new List<Point>();
            for (int r = 0; r < numCellsY; r++)
            {
                for (int c = 0; c < numCellsX; c++)
                {

                    if (!IsGardenZone(r, c) && animalZones[r][c].occupant == null)
                    {
                        points.Add(new Point { y = r, x = c });
                    }
                }
            }
            if (points.Count == 0)
            {
                return;
            }

            int index = rnd.Next(0, points.Count);
            Point target = points[index];
            animalZones[target.y][target.x].occupant = new Wolf("w")
            {
                location = target
            };

            wolves.Add((Wolf)animalZones[target.y][target.x].occupant);

        }

        static public void NextRound()
        {
            if (gameWon || gameLost) return;

            holdingPen.occupant = null;

            roundCounter++;


            // Move the mouse towards the flower and eat it if possible

            foreach (var mouse in mouses)
            {
                if (mouse.alive)
                    mouse.Move();
                // break;
            }

            foreach (var wolf in wolves)
            {
                if (wolf.alive)
                    wolf.Move();

            }

            foreach (var cat in cats)
            {
                if (cat.alive)
                    cat.Move();

            }

            foreach (var keeper in keepers)
            {
                if (keeper.alive)
                    keeper.Move();

            }


            if (flowerRemainingRound == 0)
            {
                PlaceFlowerInCenter();
                flowerRemainingRound = flowerCDTime;
            }

            if (wolfRemainingRound == 0)
            {
                PlaceWolf();
                wolfRemainingRound = wolfCDTime;
            }

            if (mouseRemainingRound == 0)
            {
                PlaceMouse();
                mouseRemainingRound = mouseCDTime;
            }

            CheckWinCondition();





            if (catRemainingRound > 0)
                catRemainingRound = (catRemainingRound + catCDTime - 1) % catCDTime;
            if (trapRemainingRound > 0)
                trapRemainingRound = (trapRemainingRound + trapCDTime - 1) % trapCDTime;

            if (keeperRemainingRound > 0)
                keeperRemainingRound = (keeperRemainingRound + keeperCDTime - 1) % keeperCDTime;

            flowerRemainingRound = (flowerRemainingRound + flowerCDTime - 1) % flowerCDTime;
            mouseRemainingRound = (mouseRemainingRound + mouseCDTime - 1) % mouseCDTime;
            wolfRemainingRound = (wolfRemainingRound + wolfCDTime - 1) % wolfCDTime;


            if (gameWon || gameLost)
            {
                return;
            }
            foreach (var mouse in mouses)
            {
                if (mouse.alive)
                    mouse.NewRound();
            }

            foreach (var wolf in wolves)
            {
                if (wolf.alive)
                    wolf.NewRound();
            }
            foreach (var cat in cats)
            {
                if (cat.alive)
                    cat.NewRound();

            }

            foreach (var keeper in keepers)
            {
                if (keeper.alive)
                    keeper.NewRound();

            }
        }

        static private void PlaceFlowerInCenter()
        {
            for (var y = 4; y <= 6; y++)
            {
                for (var x = 4; x <= 6; x++)
                {
                    if (animalZones[y][x].occupant == null)
                    {
                        animalZones[y][x].occupant = new Flowers();
                        Console.WriteLine($"Placed a flower at {x},{y}");
                        return;
                    }
                }
            }
        }

        static private void CheckWinCondition()
        {
            int flowerCount = 0;
            for (var y = 4; y <= 6; y++)
            {
                for (var x = 4; x <= 6; x++)
                {
                    if (animalZones[y][x].occupant is Flowers)
                    {
                        flowerCount++;
                    }
                }
            }

            if (flowerCount >= 9)
            {
                gameWon = true;
                Console.WriteLine("You win! 9 flowers placed in the center.");
            }
            else if (flowerCount == 0)
            {
                gameLost = true;
                Console.WriteLine("You Lose! No flowers remaining.");
            }
        }

        static public bool IsGardenZone(int row, int col)
        {
            return Math.Abs(row - 5) <= 2 && Math.Abs(col - 5) <= 2;
        }

        static public bool IsFlowerPotZone(int row, int col)
        {
            return Math.Abs(row - 5) <= 1 && Math.Abs(col - 5) <= 1;
        }


        static public void interact(Animal animal, int y, int x)
        {
            Point location = animal.location;
            // move to a empty zone
            if (animalZones[y][x].occupant == null)
            {
                animalZones[location.y][location.x].occupant = null;
                location.x = x;
                location.y = y;
                animalZones[y][x].occupant = animal;
                Console.WriteLine(animal.species + " moves to (row=" + y + ", col=" + x + ")");
                return;
            }

            var other = animalZones[y][x].occupant;

            if (animal is Mouse)
            {
                if (other is Trap)
                {
                    // mouse will be killed by trap
                    animalZones[location.y][location.x].occupant = null;
                    animalZones[y][x].occupant = null;
                    animal.alive = false;

                    Console.WriteLine(animal + " is trapped and dead");
                }
                else if (other is Flowers)
                {
                    // mouse will eat flower and stop
                    animalZones[location.y][location.x].occupant = null;
                    location.x = x;
                    location.y = y;
                    animalZones[y][x].occupant = animal;
                    animal.active = false;
                    Console.WriteLine($"Mouse ate the flower at {x},{y}");


                }
                else if (other is Cat)
                {
                    // mouse will eat flower and stop
                    Console.WriteLine($"Cat ate mouse at {x},{y}");

                    animalZones[location.y][location.x].occupant = null;
                    //animalZones[y][x].occupant = null;
                    animal.alive = false;

                    Console.WriteLine($"Mouse ate the flower at {x},{y}");
                }
                else
                {

                    Console.WriteLine($"Mouse is stuck at {x},{y}");

                    //animalZones[location.y][location.x].occupant = null;
                    //animalZones[y][x].occupant = null;
                    animal.alive = false;


                }
            }

            if (animal is Cat)
            {
                if (other is Trap)
                {
                    // mouse will be killed by trap
                    animalZones[location.y][location.x].occupant = null;
                    animalZones[y][x].occupant = null;
                    animal.alive = false;

                    Console.WriteLine(animal + " is trapped and dead");
                }
                else if (other is Mouse)
                {
                    // mouse will eat mouse and stop
                    animalZones[location.y][location.x].occupant = null;
                    location.x = x;
                    location.y = y;
                    animalZones[y][x].occupant = animal;
                    animal.active = false;
                    Console.WriteLine($"Cat ate the mouse at {x},{y}");


                }
                else if (other is Wolf)
                {
                    // mouse will eat flower and stop
                    Console.WriteLine($"Cat was eaten by wolf at {x},{y}");

                    animalZones[location.y][location.x].occupant = null;
                    //animalZones[y][x].occupant = null;
                    animal.alive = false;

                }
                else
                {

                    Console.WriteLine($"Cat is stuck at {x},{y}");

                    //animalZones[location.y][location.x].occupant = null;
                    //animalZones[y][x].occupant = null;
                    animal.alive = false;
                }
            }

            if (animal is Wolf)
            {
                if (other is Trap)
                {
                    // mouse will be killed by trap
                    animalZones[location.y][location.x].occupant = null;
                    animalZones[y][x].occupant = animal;
                    animal.active = false;
                    animal.speed -= 1;
                    if (animal.speed < 0)
                    {
                        animal.speed = 0;
                    }

                    Console.WriteLine(animal + " is trapped.");
                }
                else if (other is Mouse)
                {
                    // mouse will eat by wolf
                    animalZones[location.y][location.x].occupant = null;
                    location.x = x;
                    location.y = y;
                    animalZones[y][x].occupant = animal;
                    animal.active = false;
                    Console.WriteLine($"Wolf ate the mouse at {x},{y}");


                }
                else if (other is GardenKeeper)
                {
                    // wolf will be shot by keeper
                    Console.WriteLine($"Wolves was shot by keeper at {x},{y}");

                    animalZones[location.y][location.x].occupant = null;
                    animalZones[y][x].occupant = other;
                    location.x = x;
                    location.y = y;
                    animal.alive = false;

                }
                else if (other is Flowers)
                {
                    // flower will be step and be destroyed by wolves
                    animalZones[location.y][location.x].occupant = null;
                    location.x = x;
                    location.y = y;
                    animalZones[y][x].occupant = animal;

                    Console.WriteLine($"Wolf destroyed the flower at {x},{y}");


                }
            }

            if (animal is GardenKeeper)
            {
                if (other is Trap)
                {
                    // keeper will be release trap and no harm
                    animalZones[location.y][location.x].occupant = null;
                    animalZones[y][x].occupant = animal;
                    animal.active = false;


                    Console.WriteLine($"keeper released the trapped at {x} {y}");
                }
                else if (other is Mouse)
                {
                    // mouse will be trampled to death by keeper
                    animalZones[location.y][location.x].occupant = null;
                    location.x = x;
                    location.y = y;
                    animalZones[y][x].occupant = animal;
                    animal.active = false;
                    Console.WriteLine($"Keeper trampled the mouse to death at {x},{y}");


                }
                else if (other is Wolf)
                {
                    // wolf will be shot by keeper
                    Console.WriteLine($"Keeper shot by wolf at {x},{y}");

                    animalZones[location.y][location.x].occupant = null;
                    animalZones[y][x].occupant = animal;
                    location.x = x;
                    location.y = y;
                    ((Animal)other).alive = false;

                }
                else
                {
                    Console.WriteLine($"Cat cannot move to {x},{y}");

                    //animalZones[location.y][location.x].occupant = null;
                    //animalZones[y][x].occupant = null;
                    animal.alive = false;


                }
            }

        }
    }


}



