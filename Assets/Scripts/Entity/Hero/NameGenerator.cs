﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class NameGenerator
{
    static string[] names = new string[]
    {
        "Jens",
        "Jörgen",
        "Lisa",
        "Ludwig",
        "Gustav",
        "Niklas",
        "Mattias",
        "Viktor",
        "Zimone",
        "Rebecca",
        "Anna",
        "Bert",
        "Bob",
        "Klas",
        "Britta",
        "Stina",
        "Sven",
        "Jon",
        "Ube",
        "Tove",
        "Julia",
        "Junia",
        "Max",
        "May",
        "Oscar",
        "Pia",
        "Tommy",
        "John",
        "Dismas",
        "Reynauld",
        "Butch",
        "Rob",
        "Zen",
        "Julius",
        "Caesar",
        "Stefan",
        "Steffe",
        "Lövve",
        "Quin",
        "Quinton",
        "Alk",
        "Guzu",
        "Mon",
        "Moon Moon",
        "Sebastian",
        "Forse",
        "Pep",
        "Pe-pe",
        "Ygg",
        "Mal",
        "Luci",
        "Felix",
        "Cal",
        "Nei"
    };

    public static string GetRandom() => names.Random();
}
