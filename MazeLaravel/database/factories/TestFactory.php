<?php

/** @var \Illuminate\Database\Eloquent\Factory $factory */

use App\Test;
use App\Maze;
use Faker\Generator as Faker;

$factory->define(Test::class, function (Faker $faker) {
    
    $mazeId = $faker->randomElement(Maze::pluck('id')->all());
    $result = $faker->randomElement(array(true, false));
    $date = $faker->dateTimeBetween($startDate = '-1 weeks', $endDate = 'now -1 days', $timezone = null);

        return [
            'maze_id' => $mazeId,
            'result' => $result,
            'test_at' => $date,
        ];
});
