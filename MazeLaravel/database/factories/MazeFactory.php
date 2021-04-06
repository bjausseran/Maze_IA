<?php

/** @var \Illuminate\Database\Eloquent\Factory $factory */

use App\Maze;
use App\User;
use Faker\Generator as Faker;

$factory->define(Maze::class, function (Faker $faker) {

    $userId = $faker->randomElement(User::pluck('id')->all());
    $name = $faker->lastName."'s map";

    $width = $faker->numberBetween($min = 10, $max = 35);
    $height = $faker->numberBetween($min = 5, $max = 24);
    
    $start = "{\"Items\":[";
    $type = "{\"type\":";
    $xpos = ",\"x\":";
    $ypos = ",\"y\":";
    $endTile = "}";
    $end = "]}";

    
    $xStart = $faker->numberBetween($min = 0, $max = $width - 1);
    $yStart = $faker->numberBetween($min = 0, $max = $height - 1);

    $xEnd = $faker->numberBetween($min = 0, $max = $width - 1);
    $yEnd = $faker->numberBetween($min = 0, $max = $height - 1);

    $composition = $start;

    for ($x=0; $x < $width; $x++) { 
        for ($y=0; $y < $height; $y++) { 
            $composition = $composition.$type;

            if($x == $xStart && $y == $yStart) 
            {
                $composition = $composition.strval(5);
            }
            else if($x == $xEnd && $y == $yEnd) 
            {
                $composition = $composition.strval(6);
            }
            else 
            {
                $composition = $composition.strval($faker->randomElement($rndTile = array(0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 4)));
            }
            $composition = $composition.$xpos.strval($x).$ypos.strval($y).$endTile;
            if(!($y == $height - 1 && $x == $width - 1)) $composition = $composition.",";
        }
    }

    $composition = $composition.$end;

    return [
        'name' => $name,
        'user_id' => $userId,
        'composition' => $composition,
    ];
});
