﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class EnemyLibrary
{
    private static EnemyLibrary instance;
    private Random rng = new Random();
    public List<Actor> meleeList = new List<Actor>();
    public List<Actor> rangedList = new List<Actor>();
    public List<Actor> bossList = new List<Actor>();

    public void Init()
    {
        meleeList.Add(new Actor("e_basemelee", 5, new Weapon(ItemLibrary.Get().enemyweaponList[0]), new Armor(ItemLibrary.Get().enemyarmorList[0])));

        rangedList.Add(new Actor("e_baseranged", 3, new Weapon(ItemLibrary.Get().enemyweaponList[1]), new Armor(ItemLibrary.Get().enemyarmorList[0])));

        bossList.Add(new Actor("e_cyberbear", 12, new Weapon(ItemLibrary.Get().enemyweaponList[0]), new Armor(ItemLibrary.Get().enemyarmorList[1])));
    }

    public static EnemyLibrary Get()
    {
        if (instance == null)
        {
            instance = new EnemyLibrary();
            instance.Init();
        }

        return instance;
    }
}
