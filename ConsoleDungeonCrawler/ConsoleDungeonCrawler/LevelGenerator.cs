
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class LevelGenerator : ILevelBuilder
{

    int pickUpCount = 5;
    int enemyCount = 3;

    int meleeCount = 0;
    int rangedCount = 0;
    int bossCount = 0;

    int maxMeleeCount = 10;
    int maxRangedCount = 10;
    int maxBossCount = 1;

    private Random rng = new Random();

    public LevelGenerator()
    {
    }

    public Level Generate()
    {
        Level levelGen = new Level();
        LevelFromImage lfi = new LevelFromImage();
        levelGen.structure = lfi.BuildStructure();
        lfi.AddObjectsFromImage(levelGen);

        //levelGen.playerSpawnPoints = SetPlayerSpawnPoints();
        //levelGen.pickupSpawnPoints = SetPickupSpawnPoints();
        //levelGen.enemySpawnPoints = SetEnemySpawnPoints();

        pickUpCount = ((levelGen.structure.GetLength(0) * levelGen.structure.GetLength(1)) / 100);
        //int factor = ((levelGen.structure.GetLength(0) + levelGen.structure.GetLength(1) / 100));
        int factor = 1;
        pickUpCount = pickUpCount + (rng.Next(0, 10) * factor);

        enemyCount = pickUpCount * 2;
        maxMeleeCount = enemyCount / 2;
        maxRangedCount = (enemyCount / 2) - 1;
        maxBossCount = 1;
        /**/

        /*
        for (int i = 0; i < enemyCount; i++)
        {
            if (rng.Next(0, 2) == 0 && !(maxMeleeCount >= enemyCount / 2 + (rng.Next(-5, 5))) && !(maxMeleeCount + maxRangedCount + maxBossCount >= enemyCount)) maxMeleeCount++;
            if (rng.Next(0, 2) == 1 && !(maxRangedCount >= enemyCount / 2 + (rng.Next(-5, 5))) && !(maxMeleeCount + maxRangedCount + maxBossCount >= enemyCount)) maxRangedCount++;
            if (rng.Next(0, 2) == 2 && !(maxBossCount >= enemyCount/20) && !(maxMeleeCount + maxRangedCount + maxBossCount >= enemyCount)) maxBossCount++;
        }
        /**/


        for (int i = 0; i < enemyCount; i++)
        {
            //int current = rng.Next(0, levelGen.enemySpawnPoints.Count);
            levelGen.enemies.Add(SpawnEnemy(levelGen));
            //levelGen.enemySpawnPoints.RemoveAt(current);
        }

        for (int i = 0; i < pickUpCount; i++)
        {
            //int current = rng.Next(0, levelGen.pickupSpawnPoints.Count);
            levelGen.pickUps.Add(SpawnPickup(levelGen, rng.Next(0, ItemLibrary.Get().generics.Count)));
            //levelGen.pickupSpawnPoints.RemoveAt(current);
        }

        /**/
        //Debugging win field
        levelGen.trigger.Add(new TriggerObject("endoflevel", new Vector2(0, 19)));

        return levelGen;
    }

    private Tile[,] BuildStructure()
    {
        Tile[,] levelGenStructure = new Tile[32, 32];

        for (int i = 0; i < levelGenStructure.GetLength(0); i++)
        {
            for (int j = 0; j < levelGenStructure.GetLength(1); j++)
            {
                levelGenStructure[i, j] = new Tile("floor", ClipType.FLOOR);
            }
        }

        return levelGenStructure;
    }

    private PickUp SpawnPickup(Level level, int index)
    {
        PickUp pickUp = new PickUp(ItemLibrary.Get().generics[index], 1); //rng.Next(1,3));
        bool found = false;
        //enemy position

        //Console.WriteLine("ATTEMPTING TO SPAWN PICKUP");

        while (!found)
        {
            int a = rng.Next(0, (level.structure.GetLength(0)));
            int b = rng.Next(0, (level.structure.GetLength(1)));

            if (level.structure[a, b].substance == ClipType.FLOOR)
            {
                if (level.pickUps.Count > 0)
                {
                    for (int i = 0; i < level.pickUps.Count; i++)
                    {
                        if (!(level.pickUps[i].position.x == a && level.pickUps[i].position.y == b))
                        {
                            found = true;
                        }
                        else
                        {
                            found = false;
                            break;
                        }

                    }
                }
                else
                {
                    found = true;
                }

                if (found)
                {
                    pickUp.position = new Vector2(a, b);
                }
            }
        }

        return pickUp;
    }
    /**/

    private Actor SpawnEnemy(Level level)
    {
        Actor enemy = new Actor();
        bool found = false;
        //bool spawn = false;

        while (true)
        {
            int index = rng.Next(0, 3);
            if (index == 0 && meleeCount < maxMeleeCount)
            {
                //enemy = EnemyLibrary.Get().meleeList[rng.Next(0, EnemyLibrary.Get().meleeList.Count)];
                enemy = new Actor(EnemyLibrary.Get().meleeList[rng.Next(0, EnemyLibrary.Get().meleeList.Count)]);
                meleeCount++;
                break;
            }
            if (index == 1 && rangedCount < maxRangedCount)
            {
                //enemy = EnemyLibrary.Get().meleeList[rng.Next(0, EnemyLibrary.Get().meleeList.Count)];
                enemy = new Actor(EnemyLibrary.Get().rangedList[rng.Next(0, EnemyLibrary.Get().rangedList.Count)]);
                rangedCount++;
                break;
            }
            if (index == 2 && bossCount < maxBossCount)
            {
                //enemy = EnemyLibrary.Get().meleeList[rng.Next(0, EnemyLibrary.Get().meleeList.Count)];
                enemy = new Actor(EnemyLibrary.Get().bossList[rng.Next(0, EnemyLibrary.Get().bossList.Count)]);
                bossCount++;
                break;
            }
        }
        /***/
        //enemy = new Actor(EnemyLibrary.Get().meleeList[0]);

        //Console.WriteLine("ATTEMPTING TO SPAWN ENEMY");
        //enemy position
        while (!found)
        {
            int a = rng.Next(1, (level.structure.GetLength(0)));
            int b = rng.Next(1, (level.structure.GetLength(1)));

            //Console.WriteLine(level.structure[a, b].substance + " " + a + ", " + b);

            if (level.structure[a, b].substance == ClipType.FLOOR)
            {
                //Console.WriteLine("FOUND FLOOR");

                if (level.enemies.Count > 0)
                {
                    for (int i = 0; i < level.enemies.Count; i++)
                    {
                        //Console.WriteLine("" + level.enemies[i].position.x + ", " + level.enemies[i].position.y + " " + a + ", " + b);
                        if (!(level.enemies[i].position.x == a && level.enemies[i].position.y == b))
                        {
                            //Console.WriteLine("FLOOR NOT TAKEN");
                            found = true;
                        }
                        else
                        {
                            //Console.WriteLine("FLOOR TAKEN");
                            found = false;
                            break;
                        }

                    }
                }
                else
                {
                    //Console.WriteLine("SKIPPED SOME STUFF BRO");
                    found = true;
                }


                if (found)
                {
                    //Console.WriteLine("ATTEMPTING TO SET ENEMY POSITION");
                    enemy.position = new Vector2(a, b);
                    //Console.Write("| " + enemy.position.x + " " + enemy.position.y);
                }
            }
        }

        //enemy.position = new Vector2(19, 2);
        Application.GetData().collision.Add(enemy);
        //Console.WriteLine(enemy.name);

        return enemy;
    }
    /**/
    private List<Vector2> SetPlayerSpawnPoints()
    {
        List<Vector2> spawns = new List<Vector2>();

        spawns.Add(new Vector2(0, 0));
        spawns.Add(new Vector2(0, 19));
        spawns.Add(new Vector2(19, 0));
        spawns.Add(new Vector2(19, 19));

        return spawns;
    }

    private List<Vector2> SetPickupSpawnPoints()
    {
        List<Vector2> spawns = new List<Vector2>();

        /*
        spawns.Add(new Vector2(5, 5));
        spawns.Add(new Vector2(5, 15));
        spawns.Add(new Vector2(15, 5));
        spawns.Add(new Vector2(15, 15));
        spawns.Add(new Vector2(5, 10));
        spawns.Add(new Vector2(10, 5));
        spawns.Add(new Vector2(10, 10));
        /**/

        spawns.Add(new Vector2(18, 2));
        spawns.Add(new Vector2(18, 1));
        spawns.Add(new Vector2(19, 1));
        spawns.Add(new Vector2(19, 2));
        spawns.Add(new Vector2(17, 2));
        spawns.Add(new Vector2(18, 3));
        spawns.Add(new Vector2(17, 3));

        return spawns;
    }

    private List<Vector2> SetEnemySpawnPoints()
    {
        List<Vector2> spawns = new List<Vector2>();

        spawns.Add(new Vector2(15, 3));
        spawns.Add(new Vector2(15, 6));
        spawns.Add(new Vector2(15, 9));

        return spawns;
    }
}