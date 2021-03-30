<?php

/** @var \Illuminate\Database\Eloquent\Factory $factory */

use App\Tile;
use Faker\Generator as Faker;

$factory->define(Tile::class, function (Faker $faker) {

    $tileNb = Tile::count();

    $names = array("empty", "path", "wall", "mud", "trap", "start", "end", "solution");
    $effects = array(null, "slowdown", "death");
    $sprites = array("tile01", "tile02", "tile03", "tile04", "tile05", "tile06", "tile07", "tile08");
    $name = $faker->lastName;

    $name = $names[$tileNb];
    $sprite = $sprites[$tileNb];
    $walkable = true;
    $effect = null;
    $min = 0;
    $max = 360;

    switch ($tileNb) {
        case 0:
            $walkable = false;
            break;
        case 1:
            break;
        case 2:
            $walkable = false;
            break;
        case 3:
            $effect = $effects[1];
            break;
        case 4:
            $effect = $effects[2];
            break;
        case 5:
            $min = 1;
            $max = 1;
            break;
        case 6:
            $min = 1;
            $max = 1;
            break;
        case 7:
            break;
    }

    return [
        'name' => $name,
        'walkable' => $walkable,
        'effect' => $effect,
        'sprite' => $sprite,
        'min' => $min,
        'max' => $max,
    ];
});
